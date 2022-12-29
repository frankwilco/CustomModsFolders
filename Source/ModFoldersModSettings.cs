using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace ModFolders
{
    public class ModFoldersModSettings : ModSettings
    {
        public bool enabled = true;
        public List<ModFolder> modFolders = new List<ModFolder>();
        private string _newPath = "";

        public override void ExposeData()
        {
            Scribe_Values.Look(ref enabled, "enabled", true);
            Scribe_Collections.Look(ref modFolders, "modFolders", LookMode.Deep);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            var mainList = new Listing_Standard();
            mainList.Begin(inRect);

            mainList.Label("mdf.prefs.scan_on_startup".Translate());
            var subList = mainList.BeginSection(450f);
            ModFolder folderToRemove = null;
            foreach (var folder in modFolders)
            {
                bool currentValue = folder.active;
                bool newValue = folder.active;
                subList.CheckboxLabeled(folder.path, ref newValue);
                if (newValue != currentValue)
                {
                    folderToRemove = folder;
                    break;
                }
            }
            if (folderToRemove != null)
            {
                modFolders.Remove(folderToRemove);
            }
            mainList.EndSection(subList);

            mainList.Label("mdf.prefs.input_path.label".Translate());
            _newPath = mainList.TextEntry(_newPath);
            
            if (mainList.ButtonText("mdf.prefs.add_path.label".Translate()))
            {
                bool directoryExists = Directory.Exists(_newPath);
                bool entryExists = false;
                foreach (var folder in modFolders)
                {
                    if (folder.path.ToLowerInvariant() == _newPath.ToLowerInvariant())
                    {
                        entryExists = true;
                        break;
                    }
                }

                if (!entryExists && directoryExists)
                {
                    ModFolder modFolder = new ModFolder(_newPath);
                    modFolders.Add(modFolder);
                    _newPath = "";
                }
                else
                {
                    string message = "";
                    if (entryExists)
                    {
                        message = "mdf.dialog.entry_exists".Translate();
                    }
                    else if (!directoryExists)
                    {
                        message = "mdf.dialog.directory_missing".Translate();
                    }
                    var dialog = new Dialog_MessageBox(
                        message, title: "mdf.dialog.add_path.title".Translate());
                    Find.WindowStack.Add(dialog);
                }
            }

            mainList.End();
        }
    }
}