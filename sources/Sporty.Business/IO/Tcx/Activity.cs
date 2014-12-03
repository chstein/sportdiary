using System.Collections.Generic;

namespace Sporty.Business.IO.Tcx
{
    public class Activity
    {
        public string Id { set; get; }

        public string Sport { set; get; }

        public List<Lap> Laps { set; get; }
    }
}