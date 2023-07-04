using System.IO;
using UnityEngine;
using Verse;

namespace FrankWilco.RimWorld
{
    public class CustomModsFoldersModSettings : ModSettings
    {
        private string _newPath = "";
        private bool _warningShown = false;
        private Vector2 scrollPosition = Vector2.zero;

        public override void ExposeData()
        {
            ModLoaderData.Save();
            base.ExposeData();
        }

        // XXX: remove if RimWorld has an equivalent method.
        private Rect OffsetRect(
            Rect rect,
            float? x = null,
            float? y = null,
            float? newWidth = null,
            float? newHeight = null)
        {
            var newRect = new Rect(rect);
            if (x != null)
            {
                newRect.x += x.Value;
                newRect.width -= newWidth.Value;
            }
            if (y != null)
            {
                newRect.y += y.Value;
                newRect.height -= y.Value;
            }
            if (newWidth != null)
            {
                newRect.width = newWidth.Value;
            }
            if (newHeight != null)
            {
                newRect.height = newHeight.Value;
            }
            return newRect;
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            if (!Doorstop.Entrypoint.IsPatched && !_warningShown)
            {
                var dialog = new Dialog_MessageBox(
                    "mdf.dialog.unpatched.desc".Translate(
                        "mdf.prefs.title".Translate()),
                    title: "mdf.dialog.unpatched.title".Translate());
                Find.WindowStack.Add(dialog);
                _warningShown = true;
            }

            var topList = new Listing_Standard();
            topList.Begin(inRect);
            topList.Label("mdf.prefs.scan_on_startup".Translate());
            topList.GapLine();
            topList.End();

            var scrollBoxRect = OffsetRect(
                inRect, y: topList.CurHeight, newHeight: 450f);
            var scrollViewRect = new Rect(
                0, 0, inRect.width - 24f,
                ModLoaderData.Current.ModsFolders.Count * 26f + 8f);
            var modList = new Listing_Standard();
            Widgets.BeginScrollView(
                scrollBoxRect, ref scrollPosition, scrollViewRect);
            modList.Begin(scrollViewRect);
            ModsFolder folderToRemove = null;
            foreach (var folder in ModLoaderData.Current.ModsFolders)
            {
                bool currentValue = folder.active;
                bool newValue = folder.active;
                modList.CheckboxLabeled(folder.path, ref newValue);
                if (newValue != currentValue)
                {
                    folderToRemove = folder;
                    break;
                }
            }
            if (folderToRemove != null)
            {
                ModLoaderData.Current.ModsFolders.Remove(folderToRemove);
            }
            modList.End();
            Widgets.EndScrollView();

            var bottomList = new Listing_Standard();
            bottomList.Begin(
                OffsetRect(inRect, y: topList.CurHeight + scrollBoxRect.height));
            bottomList.Label("mdf.prefs.input_path.label".Translate());
            _newPath = bottomList.TextEntry(_newPath);
            if (bottomList.ButtonText("mdf.prefs.add_path.label".Translate()))
            {
                bool directoryExists = Directory.Exists(_newPath);
                bool entryExists = false;
                foreach (var folder in ModLoaderData.Current.ModsFolders)
                {
                    if (folder.path.ToLowerInvariant() == _newPath.ToLowerInvariant())
                    {
                        entryExists = true;
                        break;
                    }
                }

                if (!entryExists && directoryExists)
                {
                    ModsFolder modFolder = new ModsFolder(_newPath);
                    ModLoaderData.Current.ModsFolders.Add(modFolder);
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
            bottomList.End();
        }
    }
}