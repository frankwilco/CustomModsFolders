using UnityEngine;
using Verse;

namespace FrankWilco.RimWorld
{
    public class CustomModsFoldersMod : Mod
    {
        public static string ConfigFileOverride { get; internal set; }

        public static CustomModsFoldersModSettings settings;

        public CustomModsFoldersMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<CustomModsFoldersModSettings>();
        }

        public override string SettingsCategory()
        {
            return "mdf.prefs.title".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoSettingsWindowContents(inRect);
        }
    }
}
