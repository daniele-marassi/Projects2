using Additional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AllowModificationDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            Utility utility = new Utility();
            foreach (var arg in args)
            {
                utility.SetFolderPermission(arg, FileSystemRights.Modify);
            }
        }
    }
}
