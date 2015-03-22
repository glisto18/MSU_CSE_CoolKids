
using System;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

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

        public DisplayArtifact(Artifact artifact)
        {
            _artifact = artifact;
        }

        public async Task<bool> DoubleTap()
        {
            var fileStore = new FileStore();
            var artifactFile = await fileStore.GetArtifact(_artifact.Path);

            await Windows.System.Launcher.LaunchFileAsync(artifactFile);
            return false;
        }
    }
}
