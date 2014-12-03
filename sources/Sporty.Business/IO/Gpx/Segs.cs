using System;

namespace Sporty.Business.IO.Gpx
{
    public class GpxSegs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime Time { get; set; }
        public double Distance { get; set; }
    }
}