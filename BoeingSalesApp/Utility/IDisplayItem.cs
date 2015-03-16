
namespace BoeingSalesApp.Utility
{
    /// <summary>
    /// Interface for GridView Items to have so that each Item has the required Display Properties.
    /// </summary>
    interface IDisplayItem
    {
        string DisplayName { get; set; }

        string DisplayInfo { get; set; }
    }
}
