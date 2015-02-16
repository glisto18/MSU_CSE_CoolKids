using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoeingSalesApp.Utility
{
    static class TempSettings
    {
        public static string DbName = "DB_test_1";

        public static string DbPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + DbName;

   
    }
}
