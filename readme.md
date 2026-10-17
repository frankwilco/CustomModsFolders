![Logo](/About/Preview.png)

# Custom Mods Folders
This mod allows loading mods from directories outside the traditional `Mods` directory where the game resides.

Particularly useful if you want to:
- Organize mods and separate them by author, category, or whichever way you like.
- Separate mods made by other people from mods made by yourself, especially if you're a mod developer.
- Quickly add and remove a set of mods that you'd want to test.

## Usage
- Go to this mod's options.
- Enter in the path from which mods will be loaded, and click Add Mods Folder.
- The path will show up in a list above, which will also show previously added mods folders.
    * You can remove a path by clicking either on the path itself on the list or the check box.
- Changes are saved immediately upon closing the options window.
    * If the mod's options was opened from the mods configuration window, you have to close that window too.

## Compatibility
- This is incompatible with any mod that also requires the use of UnityDoorstop.
    * This mod requires UnityDoorstop because it has to modify the game's mod loading process, which cannot be achieved by a regular mod.
- This was not tested on non-Windows platforms.

## Building from source
Clone the repository or extract a downloaded ZIP copy.
- Requirements
    * Visual Studio 2022 or higher OR Visual Studio Code with C#/.NET extensions
    * .NET 10

## Download
Check the [releases](https://github.com/frankwilco/CustomModsFolders/releases) page of this repository. **This is not your ordinary RimWorld mod and setting it up is slightly more involved.**

### 1.7 and above
1. Download the latest release using the link above.
2. Extract the contents of the ZIP file to the `Mods` directory, located where RimWorld is installed. This may vary depending on where you purchased the game.
3. Update RimWorld's launch options in Steam or through your distributor with the following:

    **Windows**: `DOTNET_STARTUP_HOOKS=<replace with path to RimWorld>\Mods\CustomModsFolders\Assemblies\FrankWilco.CustomModsFolders.dll %command%`

    **Linux:** `DOTNET_STARTUP_HOOKS=<replace with path to RimWorld>/Mods/CustomModsFolders/Assemblies/FrankWilco.CustomModsFolders.dll %command%`

4. Return to the game.
5. Enable the mod and make sure it loads in the following order: after Harmony, before Core.
6. Save and apply changes.
   - You can confirm that it was installed properly if you go the mod's options and no warning prompt shows up.
   - You can also enable verbose logging (Development mode has to be turned on) in the Options menu and search the log file for "custom mods folders" to verify if mods from other directories are loaded.

### 1.6 and lower
UnityDoorstop is already bundled with these releases and do not have to be downloaded separately, unless you're building from source.

1. Download the latest release using the link above.
2. Launch RimWorld.
3. Extract the contents of the ZIP file to the directory where RimWorld is installed. This may vary depending on where you purchased the game.
    * Make sure that the ZIP's contents are not extracted somewhere else (e.g., a subdirectory). `winhttp.dll` and `doorstop_config.ini` must be in the same directory as `RimWorldWin64.exe`, and `CustomModsFolders` is placed inside the `Mods` directory.
4. Return to the game.
5. Enable the mod and make sure it loads in the following order: after Harmony, before Core.
6. Save and apply changes.
   - You can confirm that it was installed properly if you go the mod's options and no warning prompt shows up.
   - You can also enable verbose logging (Development mode has to be turned on) in the Options menu and search the log file for "custom mods folders" to verify if mods from other directories are loaded.
