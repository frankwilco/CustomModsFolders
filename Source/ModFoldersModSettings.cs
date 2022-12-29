using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;

namespace ModFolders
{
    public class ModFoldersModSettings : ModSettings
    {
        private string _newPath = "";

        public override void ExposeData()
        {
            ModLoader.Save();
            base.ExposeData();
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            var mainList = new Listing_Standard();
            mainList.Begin(inRect);

            mainList.Label("mdf.prefs.scan_on_startup".Translate());
            var subList = mainList.BeginSection(450f);
            ModFolder folderToRemove = null;
            foreach (var folder in ModLoader.Data.ModFolders)
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
                ModLoader.Data.ModFolders.Remove(folderToRemove);
            }
            mainList.EndSection(subList);

            mainList.Label("mdf.prefs.input_path.label".Translate());
            _newPath = mainList.TextEntry(_newPath);
            
            if (mainList.ButtonText("mdf.prefs.add_path.label".Translate()))
            {
                bool directoryExists = Directory.Exists(_newPath);
                bool entryExists = false;
                foreach (var folder in ModLoader.Data.ModFolders)
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
                    ModLoader.Data.ModFolders.Add(modFolder);
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