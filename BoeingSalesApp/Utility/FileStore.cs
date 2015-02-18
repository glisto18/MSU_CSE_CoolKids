using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;
using Windows.Storage.Pickers;
using Windows.Storage.AccessCache;
using BoeingSalesApp.DataAccess.Repository;

namespace BoeingSalesApp.Utility
{
    class FileStore
    {
        private string _directoryPath;

        private string _token = string.Empty;

        private IFolderTokenRepository _tokenRepo;

        public  FileStore()
        {
            _directoryPath = TempSettings.ArtifactsContainingFolderPath;
            _tokenRepo = new FolderTokenRepository();

            //CreateArtifactFolder();
        }

        public async Task ConfigureDownloadsFolder()
        {


            if (_token == string.Empty)
            {
                var picker = new FolderPicker();
                picker.FileTypeFilter.Add("*");
                var folder = await picker.PickSingleFolderAsync();
                //foreach (var file in await folder.GetFilesAsync())
                //{
                //    // do something with each file
                //}


                _token = StorageApplicationPermissions.MostRecentlyUsedList.Add(folder);

                
                await _tokenRepo.Put(_token);
            }
           

            
            
        }

        public async Task CreateTestFile()
        {

            var folderToken = await _tokenRepo.Get();
            if (folderToken != null)
            {
                _token = folderToken.ToString();
            }
            
            //await CreateArtifactFolder();
           // await folder.CreateFileAsync("I'm another file!");

            if (_token != string.Empty)
            {
                var folder = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(_token);
                var folderContents = await folder.GetFilesAsync();
                var count = folderContents.Count;
            }
            else
            {
                await ConfigureDownloadsFolder();
            }
            

        }


    }
}
