using System;
using System.Collections.Generic;
using System.Text;

namespace MTGBot.Models
{
    class ScryFallAutoCompleteModel
    {
        public string @object { get; set; }
        public int total_values { get; set; }
        public List<string> data { get; set; }

    }
}
