using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.Models
{
    public class ScryFallCardRulingsModel
    {
        public class Datum
        {
            public string @object { get; set; }
            public string oracle_id { get; set; }
            public string source { get; set; }
            public string published_at { get; set; }
            public string comment { get; set; }
        }
            public string @object { get; set; }
            public bool has_more { get; set; }
            public List<Datum> data { get; set; }        
    }
}
