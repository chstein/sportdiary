using System.Collections.Generic;

namespace Sporty.Business.IO.Gpx
{
    public class GpxTrack
    {
        public string Name { get; set; }
        public List<GpxSegs> Segs { get; set; }
    }
}