using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.AccessCache;
using BoeingSalesApp.DataAccess.Repository;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.Utility
{
    class FileStore
    {
        #region private members

        private string _directoryPath;

        private string _token = string.Empty;

        private IFolderTokenRepository _tokenRepo;

        private IArtifactRepository _artifactRepo;

        private ICategoryRepository _categoryRepository;

        private IArtifact_CategoryRepository _artifactCategoryRepository;

        private StorageFolder _artifactFolder;

        #endregion

        public FileStore()
        {
            _directoryPath = TempSettings.ArtifactsContainingFolderPath;
            _tokenRepo = new FolderTokenRepository();
            _artifactRepo = new ArtifactRepository();
            _categoryRepository = new CategoryRepository();
            _artifactCategoryRepository = new Artifact_CategoryRepository();

            //CreateArtifactFolder();
        }

        /// <summary>
        /// Checks if any new Artifacts have been inserted into the monitored folder for insert into the DB.
        /// </summary>
        public async Task<List<Guid>> CheckForNewArtifacts()
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
                else
                {
                    // user cancelled picking a folder, return empty list
                    return new List<Guid>();
                }
            }
            else 
            {
                // folder has already been selected, need to get folder from app storage
                _artifactFolder = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(_token);
            }

            // check the folder for new artifacts
            var folderArtifacts = await _artifactFolder.GetFilesAsync();
            var newArtifacts =  await CheckForNewArtifacts(folderArtifacts);

            var subFolders = await _artifactFolder.GetFoldersAsync();
            foreach (StorageFolder folder in subFolders)
            {
                 newArtifacts.AddRange(await CheckSubFolders(folder));
            }

            return newArtifacts;
        }

        private async Task<List<Guid>> CheckSubFolders(StorageFolder subFolder)
        {
            var artifactsInserted = new List<Guid>();
            Category category;
            if (!await _categoryRepository.DoesExist(subFolder.Name))
            {
                // category does not exist, create it
                category = new Category
                {
                    Name = subFolder.Name,
                    Active = true
                };
                await _categoryRepository.SaveAsync(category);
            }
            else
            {
                category = await _categoryRepository.Get(subFolder.Name);
            }

            var subFolderArtifacts = await subFolder.GetFilesAsync();
            foreach (StorageFile artifact in subFolderArtifacts)
            {
                Artifact newArtifact;
                if (!await _artifactRepo.DoesExist(artifact.Name))
                {
                    // artifact doesn't exist, create one
                    newArtifact = await InsertArtifact(artifact);
                    artifactsInserted.Add(newArtifact.ID);
                }
                else
                {
                    newArtifact = await _artifactRepo.GetArtifactByFileName(artifact.Name);
                }
                if (!await _artifactCategoryRepository.DoesExist(newArtifact, category))
                {
                    // need to add the artifact to the newly created category
                    await _artifactCategoryRepository.AddRelationship(newArtifact, category);
                }
            }

            var moreSubFolders = await subFolder.GetFoldersAsync();
            foreach (StorageFolder anotherSubFolder in moreSubFolders)
            {
                //TODO add sub category Relationship here
                artifactsInserted.AddRange(await CheckSubFolders(anotherSubFolder));
            }
            
            return artifactsInserted;
        }

        public async Task<StorageFile> GetArtifact(string path)
        {
            var folderToken = await _tokenRepo.Get();
            _token = folderToken.Token;
            if (_token != null)
            {
                _artifactFolder = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(_token);
                // get the Relative path to the _artifactFolder
                // artifact folder path ==> C\downloads\foldername
                // path of the artifact ==> c\downloads\foldername\category\file     
                int index = path.IndexOf(_artifactFolder.Path);
                string cleanPath = (index < 0)
                    ? path
                    : path.Remove(index, _artifactFolder.Path.Length+1);

                return await _artifactFolder.GetFileAsync(cleanPath);
            }
            else
            {
                // need to let user pick folder
                await CheckForNewArtifacts();
            }
            return null;
        }

        public async Task<StorageFolder> GetArtifactFolder()
        {
            var folderToken = await _tokenRepo.Get();
            _token = folderToken.Token;
            if (_token != null)
            {
                _artifactFolder = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(_token);
                return _artifactFolder;
            }
            else
            {
                // need to let user pick folder
                await CheckForNewArtifacts();
                return await GetArtifactFolder();
            }
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
        /// Method to insert a new Artifact based on the attributes of the storeageFile passed in.
        /// Returns the new Artifact
        /// </summary>
        /// <param name="artifact"></param>
        /// <returns></returns>
        private async Task<Artifact> InsertArtifact(StorageFile artifact)
        {   
                var newArtifact = new Artifact
                {
                    Path = artifact.Path,
                    Title = GetDefaultArtifactTitle(artifact.Name, artifact.FileType),
                    FileType = artifact.FileType,
                    FileName = artifact.Name,
                    DateAdded = DateTime.Now,
                    Active = true
                };

                await _artifactRepo.SaveAsync(newArtifact);
                return newArtifact;          
        }

        /// <summary>
        /// Checks the files passed into this function to see if any need to be inserted into the DB.
        /// Returns list newly inserted artifact ids.
        /// </summary>
        /// <param name="folderArtifacts"></param>
        /// <returns></returns>
        private async Task<List<Guid>> CheckForNewArtifacts(IReadOnlyList<StorageFile> folderArtifacts)
        {
            var artifactsInserted = new List<Guid>();
            foreach (StorageFile folderArtifact in folderArtifacts)
            {
                if (!await _artifactRepo.DoesExist(folderArtifact.Name))
                {
                    var newArtifact = await InsertArtifact(folderArtifact);
                    artifactsInserted.Add(newArtifact.ID);
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
