using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BoeingSalesApp.Utility
{
    static class TempSettings
    {
        public static string DbName = "DB_test_4";

        public static StorageFolder LocalFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public static string DbPath = LocalFolder.Path + "\\" + DbName;

        public static string ArtifactContainingFolder = "BoeingSalesAppArtifacts";

        public static string ArtifactsContainingFolderPath = LocalFolder.Path + "\\" + ArtifactContainingFolder;
    }
}
