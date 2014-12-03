using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Sporty.Business.IO.Gpx;
using Sporty.Business.IO.Tcx;
using Sporty.DataModel;
using System.IO;

namespace Sporty.Business.IO
{
    public class GpxParser : ExerciseParser
    {
        private const int MAX_VALUE_COUNT = 200;

        private List<GpxTrack> gpxTracks;

        public override Exercise ParseExercise(string filePath)
        {
            return ParseExercise(filePath, new Exercise());
        }

        public override Exercise ParseExercise(string filePath, Exercise exercise)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            if (exercise == null)
                exercise = new Exercise();

            LoadTracks(filePath);
            //allPoints = geoCalculator.CalculateDistances(allPoints);

            exercise.Date = GetFirstDate();

            exercise.Duration = GetDuration();

            exercise.Distance = GetDistance();
            return exercise;
        }

        private DateTime GetFirstDate()
        {
            return gpxTracks.First().Segs.First().Time;
        }

        private DateTime GetLastDate()
        {
            return gpxTracks.Last().Segs.Last().Time;
        }

        private TimeSpan GetDuration()
        {
            DateTime startDate = GetFirstDate();
            DateTime endDate = GetLastDate();
            return endDate.Subtract(startDate);
        }

        private double GetDistance()
        {
            double distanceTotal = gpxTracks.Sum(gpxTrack => gpxTrack.Segs.Sum(t => t.Distance));
            return Math.Round(distanceTotal, 2);
        }

        /// <summary> 
        /// Load the Xml document for parsing 
        /// </summary> 
        /// <param name="sFile">Fully qualified file name (local)</param> 
        /// <returns>XDocument</returns> 
        private XDocument GetGpxDoc(string sFile)
        {
            XDocument gpxDoc = XDocument.Load(sFile);
            return gpxDoc;
        }

        /// <summary> 
        /// Load the namespace for a standard GPX document 
        /// </summary> 
        /// <returns></returns> 
        private XNamespace GetGpxNameSpace()
        {
            XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            return gpx;
        }

        /// <summary> 
        /// When passed a file, open it and parse all waypoints from it. 
        /// </summary> 
        /// <param name="sFile">Fully qualified file name (local)</param> 
        /// <returns>string containing line delimited waypoints from 
        /// the file (for test)</returns> 
        /// <remarks>Normally, this would be used to populate the 
        /// appropriate object model</remarks> 
        public string LoadGpxWaypoints(string sFile)
        {
            XDocument gpxDoc = GetGpxDoc(sFile);
            XNamespace gpx = GetGpxNameSpace();

            var waypoints = from waypoint in gpxDoc.Descendants(gpx + "wpt")
                            select new
                                       {
                                           Latitude = waypoint.Attribute("lat").Value,
                                           Longitude = waypoint.Attribute("lon").Value,
                                           Elevation = waypoint.Element(gpx + "ele") != null
                                                           ? waypoint.Element(gpx + "ele").Value
                                                           : null,
                                           Name = waypoint.Element(gpx + "name") != null
                                                      ? waypoint.Element(gpx + "name").Value
                                                      : null,
                                           Dt = waypoint.Element(gpx + "cmt") != null
                                                    ? waypoint.Element(gpx + "cmt").Value
                                                    : null
                                       };

            var sb = new StringBuilder();
            foreach (var wpt in waypoints)
            {
                // This is where we'd instantiate data 
                // containers for the information retrieved. 
                sb.Append(
                    string.Format("Name:{0} Latitude:{1} Longitude:{2} Elevation:{3} Date:{4}\n",
                                  wpt.Name, wpt.Latitude, wpt.Longitude,
                                  wpt.Elevation, wpt.Dt));
            }

            return sb.ToString();
        }

        /// <summary> 
        /// When passed a file, open it and parse all tracks 
        /// and track segments from it. 
        /// </summary> 
        /// <param name="sFile">Fully qualified file name (local)</param> 
        /// <returns>string containing line delimited waypoints from the 
        /// file (for test)</returns> 
        public IEnumerable<Activity> LoadTracks(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            XDocument gpxDoc = GetGpxDoc(filePath);
            XNamespace gpx = GetGpxNameSpace();
            gpxTracks = GetGpxTracks(gpx, gpxDoc);

            gpxTracks = GetReducedTracks(gpxTracks);

            var activity = new Activity { Laps = new List<Lap>() };
            double totalDistance = 0.0;

            foreach (GpxTrack gpxTrack in gpxTracks)
            {
                //einer runde wird ein ´tcxtrack zugeordnet, da es bei GPX nur tracks gibt
                // 
                var tcxlap = new Lap { Tracks = new List<Track>() };
                var tcxtrack = new Track { TrackPoints = new List<TrackPoint>() };
                for (int i = 0; i < gpxTrack.Segs.Count; i++)
                {
                    GpxSegs seg = gpxTrack.Segs[i];
                    var tp = new TrackPoint
                                 {
                                     AltitudeMeters = seg.Elevation,
                                     DistanceMeters = seg.Distance,
                                     Time = seg.Time,
                                     Positionx = new List<Position>
                                                     {
                                                         new Position
                                                             {
                                                                 LatitudeDegrees = seg.Latitude,
                                                                 LongitudeDegrees = seg.Longitude
                                                             }
                                                     }
                                 };
                    if (i > 0)
                    {
                        GpxSegs seg1 = gpxTrack.Segs[i - 1];
                        GpxSegs seg2 = seg;
                        double distance = CalculateDistance(seg1.Latitude, seg1.Longitude, seg2.Latitude,
                                                            seg2.Longitude, 'K');
                        totalDistance += distance;
                        tp.DistanceMeters = totalDistance;
                    }
                    tcxtrack.TrackPoints.Add(tp);
                }
                tcxlap.Tracks.Add(tcxtrack);
                activity.Laps.Add(tcxlap);
            }
            return new List<Activity> { activity };
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2)) +
                          Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2)) * Math.Cos(Deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344 * 1000;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private List<GpxTrack> GetGpxTracks(XNamespace gpx, XDocument gpxDoc)
        {
            IEnumerable<GpxTrack> tracks = from track in gpxDoc.Descendants(gpx + "trk")
                                           select new GpxTrack
                                                      {
                                                          Name = track.Element(gpx + "name") != null
                                                                     ? track.Element(gpx + "name").Value
                                                                     : null,
                                                          Segs = (
                                                                     from trackpoint in track.Descendants(gpx + "trkpt")
                                                                     select new GpxSegs
                                                                                {
                                                                                    Latitude =
                                                                                        Convert.ToDouble(
                                                                                            trackpoint.Attribute("lat").
                                                                                                Value),
                                                                                    Longitude =
                                                                                        Convert.ToDouble(
                                                                                            trackpoint.Attribute("lon").
                                                                                                Value),
                                                                                    Elevation =
                                                                                        Convert.ToDouble(
                                                                                            trackpoint.Element(gpx +
                                                                                                               "ele") !=
                                                                                            null
                                                                                                ? trackpoint.Element(
                                                                                                    gpx +
                                                                                                    "ele")
                                                                                                      .
                                                                                                      Value
                                                                                                : null),
                                                                                    Time =
                                                                                        trackpoint.Element(gpx + "time") !=
                                                                                        null
                                                                                            ? DateTime.Parse(
                                                                                                trackpoint.Element(gpx +
                                                                                                                   "time")
                                                                                                    .
                                                                                                    Value)
                                                                                            : DateTime.MinValue
                                                                                }
                                                                 ).ToList()
                                                      };

            return tracks.ToList();
        }

        /// <summary>
        /// Es werden für den Chart nur ca. 400 bis 500 Werte verwendet, alle anderen werden ignoriert
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public List<GpxTrack> GetReducedTracks(List<GpxTrack> tracks)
        {
            var reducedListOfPoints = new List<GpxTrack> { new GpxTrack { Segs = new List<GpxSegs>() } };

            double avgValueCountDbl = 1;
            if (tracks.Count > 0 && tracks[0].Segs.Count > MAX_VALUE_COUNT)
                avgValueCountDbl = (double)tracks[0].Segs.Count / MAX_VALUE_COUNT;
            var avgValueCount = (int)avgValueCountDbl;

            int avgValueCounter = 1;
            int startElement = 0;

            for (int i = 0; i < tracks[0].Segs.Count; i++)
            {
                //if (avgValueCounter == 1)
                //{
                //    startElement = i - 1;
                //}
                if (avgValueCounter == avgValueCount)
                {
                    //var seg1 = tracks.ElementAt(0).Segs[startDistanceElement];
                    //var seg2 = tracks.ElementAt(0).Segs[i];
                    //var distance = CalculateDistance(seg1.Latitude, seg1.Longitude, seg2.Latitude,
                    //                                                            seg2.Longitude, 'K');
                    GpxSegs oriSeg = tracks.ElementAt(0).Segs[i];

                    reducedListOfPoints[0].Segs.Add(new GpxSegs
                                                        {
                                                            Elevation = oriSeg.Elevation,
                                                            Latitude = oriSeg.Latitude,
                                                            Longitude = oriSeg.Longitude,
                                                            Time = oriSeg.Time
                                                        });

                    avgValueCounter = 0;
                }
                avgValueCounter++;
            }

            return reducedListOfPoints;
        }
    }
}