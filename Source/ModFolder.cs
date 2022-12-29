using System.Collections.Generic;

namespace ModFolders
{
    public class ModLoaderData
    {
        public List<ModFolder> ModFolders = new List<ModFolder>();
    }

    public class ModFolder
    {
        public ModFolder(string path, bool active, bool markOfficial)
        {
            this.path = path;
            this.active = active;
            this.markOfficial = markOfficial;
        }

        public ModFolder(string path, bool active)
            : this(path, active, false)
        {
        }

        public ModFolder(string path)
            : this(path, true)
        {
        }

        public ModFolder() : this("", true, false)
        {
        }

        public string path;
        public bool active;
        public bool markOfficial;
    }
}
