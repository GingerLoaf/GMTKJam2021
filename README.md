# Welcome to GMTK Jam 2021!!!
## Dates: June 11-13
## https://itch.io/jam/gmtk-2021

# Here's what you need to do to get started.
## Join our discord
- https://discord.gg/fzgHn8XmrG
  
## Join Codecks
- Give @GingerLoaf#2628 the email you'd like to use for `codecks.io`.
- You will get an invite to `codecks.io` for an organization named `ging-gmtk2021`. Accept the invitation and get you some tasks!
- Once you join feel free to take any unassigned card that you think you can complete and communicate to your coworkers about this so we can allocate resources effeciently.
- Once you complete a card just click the checkbox at the top of the card to mark it as completed.

## Join Github
- Sign up for an account at `github.com`.
- Send the email used to @GingerLoaf#2628 so he can add you to the code repository.
- You will get an invite to the `Github` project. Accept the invite if you want to contribute!
  
## Install Github Desktop
- Install `Github` desktop from https://desktop.github.com
- Remember that git commits are **LOCAL ONLY** and are not shared with others. Push your changes in github desktop to share your changes with everyone else!
- All our work will be in the main branch (the default... so you wont have to change anything)
- Follow this gif guide to connect the codebase to your local machine

![Image of Yaktocat](readme-images/gmtk-20201-onboarding-1.gif)

## Install Unity
- Download Unity Hub (if you don't have it already): https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe
- After the Unity Hub is installed, open this link in a web browser to launch the installer for our specific editor version: <unityhub://2021.1.9f1/7a790e367ab3>
- During installation uncheck ALL boxes except for `Microsoft Visual Studio Community 2019` if you want to use that for editing code.
- After installation completes, open the unity project files at `<repository-root>\GMTKJam2021-Unity`

# Here are some tips for development

## Discord
You can use commands to easily create cards!
- `!bug <description>` - reports a bug
- `!art <description>` - creates a card for an artist
- `!code <description>` - creates a card for a programmer
- `!design <description>` - creates a card for a designer
- `!audio <description>` - creates a card for an audio engineer or musician
- `!team <description>` - creates a card for the entire team. Use this to bring up reminders for the team or a proposal for a change to team processes
  
## Unity

### Project Structure
```
Source
+---Animation
+---Art
|   +---GameReady: Game ready assets like materials, shaders, prefabs, etc...
|   \---Source~: The project files art assets come from such as .blend, .maya, or .3ds. The ~ character makes unity ignore this directory
+---Audio
|   +---GameReady: Game ready assets like mp3, wav, or ogg files
|   |   +---Music: songs only
|   |   \---SFX: sfx only
|   \---Source~: The project files audio assets come from such as .reason or .ableton. The ~ character makes unity ignore this directory
+---Code
|   +---Editor: Editor scripts like menus, windows, custom inspectors, etc...
|   \---Runtime: Code that will be shipped with the compiled game when a build is made.
+---Data: .asset files that contain game data
+---Scenes: all our game scenes
|   +---GameReady: Game ready scenes
|   +---Editor: Editor only scenes that will not be included in the final build
```

### External Tools
#### Unity Atoms
This tool allows every serialized field to be a "reference" to external data assets.
This decouples the inputs to game logic from the way that the data is used.
It also has the ability to make scripted events to entirely decouple systems from one another.
Read up on it [Here](https://github.com/unity-atoms/unity-atoms)!

### Project Validator (Wrist Slapper)
This tool attempts to ensure a clean project structure while reducing the time spent between humans discussing the structure (it's automatic).
When you modify the project, it will be validated and if errors are detected they will be presented to the user with some suggested fixes.

![Image of Yaktocat](readme-images/wristslapper.PNG)

If you find that an asset is incorrectly categorized or not categorized at all, you can add or edit a `.slp` file in any directory.
Simply add a list of extensions in the file (one per line) and the validator will use that as a recursive guide from that directory and deeper.
An example `.slp` file that enforces images only might be something like this:

```text
.png
.jpg
.jpeg
.gif
```

If those lines are placed into a `.slp` file in some directory then that directory and all directories beneath it will enforce that only those types of files can be placed inside.
`.slp` lines are recursively "stacked" on each other as the hierarchy is resolved to any given file.
The validator starts with the directory that the file is in and then recursively finds all `.slp` files in the parent directories and combines all of them to yield the final set of rules.
This allows global settings to be configured such as a rule for `.meta` files being allowed in any directory of the project.

### Additional Tutorials
- [Visual Scripting](GMTKJam2021-Unity/Assets/Source/Documentation/VisualScripting.md)