using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

internal class WristSlapperWindow : EditorWindow
{

    #region Private Methods

    [InitializeOnLoadMethod]
    private static void UnityEntryPoint()
    {
        WristSlapper.OnValidationCompleted += WristSlapperOnOnValidationCompleted;
    }

    private static void WristSlapperOnOnValidationCompleted(IReadOnlyCollection<string> invalidPaths)
    {
        SetDataAndShowIfNeeded(invalidPaths);
    }

    private void OnGUI()
    {
        m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);
        if (GUILayout.Button("Validate"))
        {
            WristSlapper.ValidateProject();
        }

        if (m_invalidEntries.Count > 0)
        {
            EditorGUILayout.HelpBox("Please open the project readme file and re-read the rules to find the correct file locations. You can add or modify a .slp file in any directory to change the validation rules.", MessageType.Error);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("The following files are in invalid locations:", EditorStyles.boldLabel);
            foreach (var (path, @object) in m_invalidEntries)
            {
                EditorGUILayout.LabelField(path);

                EditorGUILayout.ObjectField(@object, typeof(Object), false);

                var extension = Path.GetExtension(path);
                if (m_extensionToRelativePaths.TryGetValue(extension, out var relativePaths))
                {
                    GUILayout.BeginHorizontal();
                    foreach (var relativePath in relativePaths)
                    {
                        if (GUILayout.Button($"Move to {relativePath}"))
                        {
                            var currentFilePath = AssetDatabase.GetAssetPath(@object);
                            var filename = Path.GetFileName(currentFilePath);
                            var targetFilePath = Path.Combine(relativePath, filename);
                            var errorMessage = AssetDatabase.MoveAsset(currentFilePath, targetFilePath);
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                Debug.LogError(errorMessage);
                            }
                        }
                    }

                    GUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.LabelField($"Extension {extension} not recognized. Modify or add .slp files in the folders that should allow {extension} files.");
                }

                EditorGUILayout.Space();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("There are no issues", MessageType.Info);
        }

        GUILayout.EndScrollView();
    }

    private static string GetRelativePath(string absolutePath)
    {
        var parts = absolutePath.Split(new[] { "Assets/" }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            throw new InvalidOperationException($"{absolutePath} is not in the unity project");
        }

        var relativePart = "Assets/" + parts[1];
        return relativePart;
    }

    private static void SetDataAndShowIfNeeded(IReadOnlyCollection<string> invalidPaths)
    {
        _ = invalidPaths ?? throw new ArgumentException(nameof(invalidPaths));

        var invalidObjects = new List<(string, Object)>();
        foreach (var invalidPath in invalidPaths)
        {
            var relativePath = GetRelativePath(invalidPath);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(relativePath);
            invalidObjects.Add((relativePath, asset));
        }

        var windowInstance = GetWindow<WristSlapperWindow>("Project Errors", false);
        windowInstance.m_invalidEntries = invalidObjects;
        windowInstance.m_extensionToRelativePaths.Clear();
        var (_, extensionToAllowedDirectories) = WristSlapper.LoadFullConfiguration(Application.dataPath);
        foreach (var kvp in extensionToAllowedDirectories)
        {
            var paths = kvp.Value;
            for (var i = 0; i < paths.Length; i++)
            {
                paths[i] = GetRelativePath(paths[i]);
            }

            windowInstance.m_extensionToRelativePaths[kvp.Key] = paths;
        }

        if (invalidPaths.Count > 0)
        {
            windowInstance.Show(); 
            windowInstance.Focus();
        }
    }

    #endregion

    #region Private Fields

    private Vector2 m_scrollPosition = default;

    private readonly Dictionary<string, string[]> m_extensionToRelativePaths = new Dictionary<string, string[]>();

    [NotNull]
    private IReadOnlyCollection<(string path, Object @object)> m_invalidEntries = Array.Empty<(string path, Object @object)>();

    #endregion

}