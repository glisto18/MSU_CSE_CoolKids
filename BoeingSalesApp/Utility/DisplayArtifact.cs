
using System;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;
using Microsoft.Office.Core;

namespace BoeingSalesApp.Utility
{
    class DisplayArtifact: IDisplayItem
    {
        private Artifact _artifact;

        public Guid Id
        {
            get { return _artifact.ID; }
            set { }
        }

        public string DisplayName
        {
            get { return _artifact.Title; }
            set { }
        }

        public string DisplayInfo
        {
            get { return _artifact.FileName; }
            set { }
        }

        public Artifact GetArtifact()
        {
            return _artifact;
        }

        public string DisplayIcon { get; set; }

        public DisplayArtifact(Artifact artifact)
        {
            _artifact = artifact;
            SetDisplayIcon();
        }

        private void SetDisplayIcon()
        {
            switch (_artifact.FileType.ToLower())
            {
                case ".txt":
                    DisplayIcon = "Assets/txt100x100.png";
                    break;

                case ".doc":
                case ".docx":
                    DisplayIcon = "Assets/w100x100.png";
                    break;

                case ".ppt":
                case ".pptx":
                    DisplayIcon = "Assets/pp100x100.png";
                    break;

                case ".mov":
                case ".wmv":
                case ".mp4":
                    DisplayIcon = "Assets/video100x100.png";
                    break;

                case ".pdf":
                    DisplayIcon = "Assets/pdf100x100.png";
                    break;

                case ".png":
                case ".jpeg":
                case ".jpg":
                case ".gif":
                case ".tif":
                case ".tff":
                    DisplayIcon = "Assets/image100x100.png";
                    break;
                
                default:
                    DisplayIcon = "Assets/Artifact.png";
                    break;
            }
        }

        public async Task<bool> DoubleTap()
        {
            var fileStore = new FileStore();
            var artifactFile = await fileStore.GetArtifact(_artifact.Path);

            var options = new Windows.System.LauncherOptions();
            options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMinimum;

            await Windows.System.Launcher.LaunchFileAsync(artifactFile, options);
            return false;
        }

        public async Task UpdateTitle(string newName)
        {
            ArtifactRepository artRepo = new ArtifactRepository();
            await artRepo.UpdateTitle(_artifact.ID, newName);
        }
    }
}
