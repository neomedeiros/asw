using System.Collections.Generic;

namespace ASW.Models
{
    public class DiffResultModel
    {
        public DiffResultModel()
        {
            DiffInsights = new List<string>();
        }

        public long Id { get; set; }        
        public string Left { get; set; }
        public string Right { get; set; }
        public bool AreEqual { get; set; }
        public bool HaveSameSize { get; set; }
        public List<string> DiffInsights { get; set; }
    }
}