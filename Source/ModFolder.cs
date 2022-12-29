using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ModFolders
{
    public class ModFolder : IExposable
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

        public string path = "";
        public bool active = true;
        public bool markOfficial = false;

        public void ExposeData()
        {
            Scribe_Values.Look(ref path, "path", "");
            Scribe_Values.Look(ref active, "active", true);
            Scribe_Values.Look(ref markOfficial, "markOfficial", false);
        }
    }
}
