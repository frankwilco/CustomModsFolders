using ModFolders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorstop
{
    public class Entrypoint
    {
        public static void Start()
        {
            ModFoldersMod.Patch();
        }
    }
}