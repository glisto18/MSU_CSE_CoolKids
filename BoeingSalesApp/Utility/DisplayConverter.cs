using System.Collections.Generic;
using System.Linq;
using BoeingSalesApp.DataAccess.Entities;
using System.Threading.Tasks;

namespace BoeingSalesApp.Utility
{
    static class DisplayConverter
    {

        public static List<DisplayArtifact> ToDisplayArtifacts(List<Artifact> artifacts)
        {
            return artifacts.Select(a => new DisplayArtifact(a)).ToList();
        }

        public static List<DisplayCategory> ToDisplayCategories(List<Category> categories)
        {
            return categories.Select(c => new DisplayCategory(c)).ToList();
        }

        public static List<DisplaySalesbag> ToDisplaySalebsag(List<SalesBag> bags)
        {
            return bags.Select(c => new DisplaySalesbag(c)).ToList();
        }

        public static async Task ToSetArtNums(List<DisplaySalesbag> bags)
        {
            foreach(var bag in bags)
            {
                await bag.getArtNum();
            }
        }

        public static List<DisplayMeeting> ToDisplayMeetings(List<Meeting> meetings)
        {
            return meetings.Select(c => new DisplayMeeting(c)).ToList();
        }

    }
}
