using System.Collections.Generic;

namespace DatingApp.API.Model
{
    public class ItemSet : Dictionary<List<string>, int>
    {
        public string Label { get; set; }
        public int Support { get; set; }
    }
}