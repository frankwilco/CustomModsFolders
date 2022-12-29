using System.Collections.Generic;

namespace FrankWilco.CustomModsFolders
{
    public class ModsFolder
    {
        public ModsFolder(string path, bool active, bool markOfficial)
        {
            this.path = path;
            this.active = active;
            this.markOfficial = markOfficial;
        }

        public ModsFolder(string path, bool active)
            : this(path, active, false)
        {
        }

        public ModsFolder(string path)
            : this(path, true)
        {
        }

        public ModsFolder() : this("", true, false)
        {
        }

        public string path;
        public bool active;
        public bool markOfficial;
    }
}
