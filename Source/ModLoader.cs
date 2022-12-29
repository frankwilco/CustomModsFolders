using HarmonyLib;
using System.IO;
using System.Linq;
using System.Reflection;
using Verse;

namespace FrankWilco.CustomModsFolders
{
    [HarmonyPatch(typeof(Verse.Steam.WorkshopItems),
        nameof(Verse.Steam.WorkshopItems.EnsureInit))]
    public static class ModLoader
    {
        private const string kModsFoldersDataFile = "ModsFolders.xml";

        private static readonly MethodInfo _tryAddMod =
            typeof(ModLister).GetMethod(
                "TryAddMod", BindingFlags.NonPublic | BindingFlags.Static);

        private static bool TryAddMod(ModMetaData mod)
        {
            return (bool)_tryAddMod.Invoke(null, new object[] { mod });
        }

        private static readonly string _dataFilePath =
            Path.Combine(GenFilePaths.ModsFolderPath, kModsFoldersDataFile);
        public static string DataFilePath
        {
            get { return _dataFilePath; }
        }

        private static ModLoaderData _data = null;
        public static ModLoaderData Data
        {
            get
            {
                if (_data == null)
                {
                    DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
                    _data = DirectXmlLoader.ItemFromXmlFile<ModLoaderData>(DataFilePath);
                }
                return _data;
            }
        }

        public static void Save()
        {
            DirectXmlSaver.SaveDataObject(Data, DataFilePath);
        }

        public static void Prefix()
        {
            string s = "Rebuilding mods list (custom mods folders)";

            foreach (ModsFolder folder in Data.ModsFolders)
            {
                if (!folder.active || folder.path.Trim() == "")
                {
                    continue;
                }
                s += $"\nAdding mods from custom folder: {folder.path}";
                var directoryNames = from d
                                     in new DirectoryInfo(folder.path).GetDirectories()
                                     select d.FullName;
                foreach (string item in directoryNames)
                {
                    ModMetaData modMetaData = new ModMetaData(
                        item, official: folder.markOfficial);
                    if (TryAddMod(modMetaData))
                    {
                        s = s + "\n  Adding " + modMetaData.ToStringLong();
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
    }
}
