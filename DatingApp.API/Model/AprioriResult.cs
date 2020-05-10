using System.Collections.Generic;

namespace DatingApp.API.Model
{
    public class AprioriResult
    {
        public AprioriResult()
        {
            ItemSet = new List<Dictionary<string, int>>();
            AssociationRules = new List<List<AssociationRule>>();
            ItemSets = new List<List<ItemSet>>();
            AllLines = new List<string>();
        }

        public List<Dictionary<string, int>> ItemSet { get; set; }
        public List<List<AssociationRule>> AssociationRules { get; set; }
        public List<List<ItemSet>> ItemSets { get; set; }
        public List<string> AllLines { get; set; }
    }
}