using System.Collections.Generic;
using System.Linq;
using BoeingSalesApp.DataAccess.Entities;

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
    }
}
