using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

internal static class WristSlapper
{

    #region Public Events

    public static event Action<IReadOnlyCollection<string>> OnValidationCompleted;

    #endregion

    #region Private Constants

    private const string CONFIG_FILE_EXTENSION = ".slp";

    #endregion

    #region Private Methods

    private static string ToUnityPath(string sourcePath)
    {
        return sourcePath.Replace('\\', '/');
    }

    private static async void StartSlapperProcessAsync()
    {
        // Zack: give control back to calling thread right away so we don't process on that stack.
        //       We want to process later on!
        await Task.Yield();

        EditorApplication.projectChanged += ValidateProject;
        Debug.Log("Start File Monitor");
        var interval = TimeSpan.FromMinutes(5);
        while (!EditorApplication.isCompiling)
        {
            ValidateProject();
            await Task.Delay(interval, CancellationToken.None);
        }

        EditorApplication.projectChanged -= ValidateProject;
        Debug.Log("Stop File Monitor");
    }

    private static bool IsDirectory(string path)
    {
        var attr = File.GetAttributes(path);
        var isDirectory = attr.HasFlag(FileAttributes.Directory);
        return isDirectory;
    }

    private static string[] GetAllowedExtensionsAtPath(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath) ?? string.Empty;
        var parts = directory.Split('\\', '/');
        var allSlapFiles = new List<string>();
        var rebuiltPath = string.Empty;
        for (var i = 0; i < parts.Length; i++)
        {
            rebuiltPath += parts[i] + '/';
            var foundFiles = Directory.GetFiles(rebuiltPath, $"*{CONFIG_FILE_EXTENSION}", SearchOption.TopDirectoryOnly);
            allSlapFiles.AddRange(foundFiles);
        }

        var allowedExtensions = new List<string>();
        for (var i = 0; i < allSlapFiles.Count; i++)
        {
            var currentSlapFilePath = allSlapFiles[i];
            var lines = File.ReadAllLines(currentSlapFilePath);
            var extensions = lines.Where(l => !string.IsNullOrEmpty(l)).ToArray();
            allowedExtensions.AddRange(extensions);
        }

        return allowedExtensions.ToArray();
    }

    #endregion

    internal static (IReadOnlyDictionary<string, string[]> directoryToAllowedExtensions, IReadOnlyDictionary<string, string[]> extensionToAllowedDirectories) LoadFullConfiguration(string root)
    {
        var directoryToAllowedExtensionsMap = new Dictionary<string, string[]>();
        var extensionToAllowedDirectoriesMap = new Dictionary<string, string[]>();

        var allConfigurationFiles = Directory.GetFiles(root, CONFIG_FILE_EXTENSION, SearchOption.AllDirectories);
        for (var i = 0; i < allConfigurationFiles.Length; i++)
        {
            var currentFile = allConfigurationFiles[i];
            var allowedExtensions = GetAllowedExtensionsAtPath(currentFile);

            var currentDirectory = Path.GetDirectoryName(currentFile) ?? string.Empty;
            currentDirectory = ToUnityPath(currentDirectory);
            directoryToAllowedExtensionsMap[currentDirectory] = allowedExtensions;

            for (var j = 0; j < allowedExtensions.Length; j++)
            {
                var currentExtension = allowedExtensions[j];
                if (!extensionToAllowedDirectoriesMap.TryGetValue(currentExtension, out var pathList))
                {
                    extensionToAllowedDirectoriesMap[currentExtension] = pathList = new string[0];
                }

                ArrayUtility.Add(ref pathList, currentDirectory);
                extensionToAllowedDirectoriesMap[currentExtension] = pathList;
            }
        }

        return (directoryToAllowedExtensionsMap, extensionToAllowedDirectoriesMap);
    }

    internal static void ValidateProject()
    {
        var sourcePath = Path.Combine(Application.dataPath, @"Source");
        Debug.Log("Validating project structure");
        var invalidFilePaths = new List<string>();

        try
        {
            foreach (var fileSystemEntry in Directory.EnumerateFileSystemEntries(sourcePath, "*", SearchOption.AllDirectories))
            {
                if (IsDirectory(fileSystemEntry))
                {
                    continue;
                }

                var extension = Path.GetExtension(fileSystemEntry);
                if (extension.Equals(CONFIG_FILE_EXTENSION, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                var standardizedPath = ToUnityPath(fileSystemEntry);
                var allowedExtensions = GetAllowedExtensionsAtPath(fileSystemEntry);

                var isAllowed = allowedExtensions.Contains(extension, StringComparer.InvariantCultureIgnoreCase);
                if (!isAllowed)
                {
                    invalidFilePaths.Add(standardizedPath);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Debug.Log($"Validation completed with {invalidFilePaths.Count} errors");

        try
        {
            OnValidationCompleted?.Invoke(invalidFilePaths);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    [InitializeOnLoadMethod]
    internal static void UnityEntryPoint()
    {
        StartSlapperProcessAsync();
    }

}