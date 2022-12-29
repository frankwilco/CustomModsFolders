using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ModFolders
{
    public class ModFoldersMod : Mod
    {
        public static ModFoldersModSettings settings;

        public ModFoldersMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModFoldersModSettings>();
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
