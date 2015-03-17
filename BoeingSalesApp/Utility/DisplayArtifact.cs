

namespace BoeingSalesApp.Utility
{
    class DisplayArtifact: IDisplayItem
    {
        private DataAccess.Entities.Artifact _artifact;

        public string DisplayName
        {
            get { return _artifact.FileName; }
            set { }
        }

        public string DisplayInfo
        {
            get { return "Some Info about the artifact"; }
            set { }
        }

        public DisplayArtifact(DataAccess.Entities.Artifact artifact)
        {
            _artifact = artifact;
        }
    }
}
