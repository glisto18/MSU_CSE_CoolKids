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
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.Utility
{
    class FileStore
    {
        private string _directoryPath;

        private string _token = string.Empty;

        private IFolderTokenRepository _tokenRepo;

        private IArtifactRepository _artifactRepo;

        private StorageFolder _artifactFolder;

        public  FileStore()
        {
            _directoryPath = TempSettings.ArtifactsContainingFolderPath;
            _tokenRepo = new FolderTokenRepository();
            _artifactRepo = new ArtifactRepository();

            //CreateArtifactFolder();
        }

        /// <summary>
        /// Checks if any new Artifacts have been inserted into the monitored folder for insert into the DB.
        /// </summary>
        public async Task<List<int>> CheckForNewArtifacts()
        {
            var folderToken = await _tokenRepo.Get();
            if (folderToken != null)
            {
                _token = folderToken.Token;
            }
            
            
            if (_token == string.Empty) 
            {
                // user needs to pick a folder
                _artifactFolder = await UserSelectFolder();
                
                if (_artifactFolder != null)
                {
                    // save folder in app storage
                    _token = StorageApplicationPermissions.MostRecentlyUsedList.Add(_artifactFolder);
                    // save token for later use
                    await _tokenRepo.Put(_token);
                }
            }
            else 
            {
                // folder has already been selected, need to get folder from app storage
                _artifactFolder = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(_token);
            }

            // check the folder for new artifacts
            var folderArtifacts = await _artifactFolder.GetFilesAsync();
            return await CheckForNewArtifacts(folderArtifacts);


            
        }

        /// <summary>
        /// Removes the current Artifact folder and allows the user to select a new folder to Artifacts to be saved at.
        /// </summary>
        public void ChangeArtifactFolder()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// allows the user to upload new file
        /// </summary>
        public void UploadNewArtifact()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Checks the files contained in the artifact folder to see if any need to be inserted into the DB.
        /// </summary>
        /// <param name="folderArtifacts"></param>
        /// <returns></returns>
        private async Task<List<int>> CheckForNewArtifacts(IReadOnlyList<StorageFile> folderArtifacts)
        {
            var artifactsInserted = new List<int>();
            foreach (StorageFile folderArtifact in folderArtifacts)
            {
                if (!await _artifactRepo.DoesExist(folderArtifact.Name))
                {
                    var newArtifact = new Artifact
                    {
                        Path = folderArtifact.Path,
                        Title = GetDefaultArtifactTitle(folderArtifact.Name, folderArtifact.FileType),
                        FileType = folderArtifact.FileType,
                        FileName = folderArtifact.Name,
                        DateAdded = DateTime.Now,
                        Active = true
                    };

                    var newArtifactId = await _artifactRepo.SaveAsync(newArtifact);
                    artifactsInserted.Add(newArtifactId);
                }
            }

            return artifactsInserted;
        }


        /// <summary>
        /// Creates a default Title for an Artifact by substracting the filetype off of the filename.
        /// Artifacts should be able to have their titles changed at another time.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private string GetDefaultArtifactTitle(string fileName, string fileType)
        {
            int index = fileName.IndexOf(fileType);
            if(index < 0)
            {
                return fileName;
            }else
            {
                return fileName.Remove(index, fileType.Length);
            }
                
        }

        /// <summary>
        /// Allows user to select folder for Artifacts to be stored at.
        /// Returns folder selected.
        /// </summary>
        /// <returns></returns>
        private async Task<StorageFolder> UserSelectFolder()
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            var folder = await picker.PickSingleFolderAsync();
            return folder;
        }    

    }
}
