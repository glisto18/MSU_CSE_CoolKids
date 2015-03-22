

using System;
using System.Threading.Tasks;

namespace BoeingSalesApp.Utility
{
    /// <summary>
    /// Interface for GridView Items to have so that each Item has the required Display Properties.
    /// </summary>
    interface IDisplayItem
    {
        string DisplayName { get; set; }

        string DisplayInfo { get; set; }

        Guid Id { get; set; }

        // returns true or false, if true the caller will know to do an action, if false, the caller does nothing.
        Task<bool> DoubleTap();
    }
}
