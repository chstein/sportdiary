using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.ViewModel
{
    public class MaterialView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool InUsage { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? Lifetime { get; set; }

        public double? Milage { get; set; }

        public bool IsNew { get; set; }

        public string Filename { get; set; }
    }
}
