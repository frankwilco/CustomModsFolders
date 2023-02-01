using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Verse;

namespace FrankWilco.RimWorld
{
    public class ModLoaderData
    {
        public List<ModsFolder> ModsFolders = new List<ModsFolder>();

        public const string kDataFile = "ModsFolders.xml";
        private static string _filePath;
        public static string FilePath
        {
            get
            {
                if (_filePath == null)
                {
                    _filePath = Path.Combine(
                        GenFilePaths.ModsFolderPath, kDataFile);
                }
                return _filePath;
            }
        }

        private static ModLoaderData _current = null;
        public static ModLoaderData Current
        {
            get
            {
                if (_current == null)
                {
                    DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
                    _current = DirectXmlLoader.ItemFromXmlFile<ModLoaderData>(FilePath);
                }
                return _current;
            }
        }

        public static void Save()
        {
            DirectXmlSaver.SaveDataObject(Current, FilePath);
        }

    }
}
