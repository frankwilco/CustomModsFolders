using HarmonyLib;
using Steamworks;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Verse;
using Verse.Steam;

namespace FrankWilco.RimWorld
{
    [HarmonyPatch]
    public static class ModLoaderPatch
    {
        private static bool _workshopInitialized = false;

        // We have to parse all workshop items manually here because we've
        // accessed the WorkshopItems class too early. Keep this in sync with
        // Verse.Steam.WorkshopItems.RebuildItemsList().
        private static void WorkshopRebuildItemsList()
        {
            // This should return early on non-Steam builds, since the game's
            // Steam Manager is never initialized in those cases.
            if (_workshopInitialized || !SteamManager.Initialized)
            {
                return;
            }

            var subbedItems = (List<WorkshopItem>)AccessTools.Field(
                typeof(WorkshopItems), "subbedItems").GetValue(null);
            var downloadingItems = (List<WorkshopItem_Downloading>)AccessTools.Field(
                typeof(WorkshopItems), "downloadingItems").GetValue(null);
            var allSubscribedItems = (IEnumerable<PublishedFileId_t>)AccessTools.Method(
                typeof(Workshop), "AllSubscribedItems").Invoke(null, null);

            subbedItems.Clear();
            downloadingItems.Clear();
            // ... in Workshop.AllSubscribedItems()
            foreach (PublishedFileId_t item in allSubscribedItems)
            {
                WorkshopItem workshopItem = WorkshopItem.MakeFrom(item);
                if (workshopItem != null)
                {
                    subbedItems.Add(workshopItem);
                    if (workshopItem is WorkshopItem_Downloading)
                    {
                        downloadingItems.Add(workshopItem as WorkshopItem_Downloading);
                    }
                }
            }

            // We're done.
            _workshopInitialized = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(
            typeof(WorkshopItems),
            nameof(WorkshopItems.EnsureInit))]
        public static void WorkshopItems_EnsureInit_Postfix()
        {
            // XXX: Force re-initialization of workshop items.
            WorkshopRebuildItemsList();

            var TryAddMod = AccessTools.Method(typeof(ModLister), "TryAddMod");

            string s = "Rebuilding mods list (custom mods folders)";

            foreach (ModsFolder folder in ModLoaderData.Current.ModsFolders)
            {
                if (!folder.active || folder.path.Trim() == "")
                {
                    s += $"\nSkipping inactive or invalid custom folder: {folder.path}";
                    continue;
                }
                s += $"\nAdding mods from custom folder: {folder.path}";
                if (folder.markOfficial)
                {
                    s += " (marked as official content)";
                }
                var directoryNames = from d
                                     in new DirectoryInfo(folder.path).GetDirectories()
                                     select d.FullName;
                foreach (string item in directoryNames)
                {
                    ModMetaData modMetaData = new ModMetaData(
                        item,
                        official: folder.markOfficial
                    );
                    // TryAddMod(modMetaData)
                    bool isModValid = (bool)TryAddMod.Invoke(
                        null, new object[] { modMetaData });
                    if (isModValid)
                    {
                        s += "\n  Adding " + modMetaData.ToStringLong();
                    }
                    else
                    {
                        s += "\n  Invalid mod not added " + modMetaData.ToStringLong();
                    }
                    uint steamAppId = modMetaData.SteamAppId;
                    if (steamAppId != 0)
                    {
                        s += $" (steam_appid: {steamAppId})";
                    }
                }
            }
#if !DEBUG
            if (Prefs.LogVerbose)
#endif
            {
                Log.Message(s);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(
            typeof(GenFilePaths),
            nameof(GenFilePaths.ModsConfigFilePath),
            MethodType.Getter)]
        public static bool ModsConfigFilePath_Prefix(ref string __result)
        {
            if (GenCommandLine.TryGetCommandLineArg("configname", out var configName))
            {
                Log.Message($"Overriding mods config file path: {configName}");
                string configFilePath = Path.Combine(
                    GenFilePaths.ConfigFolderPath,
                    $"ModsConfig.{configName}.xml");
                CustomModsFoldersMod.ConfigFileOverride = configFilePath;
                __result = configFilePath;
                return false;
            }
            return true;
        }
    }
}
