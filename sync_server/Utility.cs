using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sync_server
{
    class Utility
    {

        public static String FilePathWithVers(String path, Int64 Version){
            if (Path.GetDirectoryName(path) =="\\")
                return '\\' + Path.GetFileNameWithoutExtension(path) + '_' + Version + Path.GetExtension(path) ;
            else  return Path.GetDirectoryName(path)+ '\\' + Path.GetFileNameWithoutExtension(path) + '_' + Version + Path.GetExtension(path) ;
        }
    }
}
