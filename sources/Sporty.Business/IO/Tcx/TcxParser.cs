using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Sporty.DataModel;
using Sporty.ViewModel;
using System.Xml;
using System.Threading;
using System.IO;

namespace Sporty.Business.IO.Tcx
{
    public class TcxParser : ExerciseParser
    {
        //Beschreibt wieviele Punkte zusammengefasst werden
        private const int Smoothfactor = 3;

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
            XElement root = XElement.Load(filePath);
            XNamespace ns1 = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";

            //TODO Alternativ ist auch Courses statt Activities möglich, resp. Course zu Activity
            bool isActivities = (root.Descendants(ns1 + "Activities").FirstOrDefault() != null);

            string id = isActivities ?
                root.Descendants(ns1 + "Activities").First().Descendants(ns1 + "Activity").First().Element(ns1 + "Id").
                Value :
                root.Descendants(ns1 + "Courses").First().Descendants(ns1 + "Course").First().Element(ns1 + "Id").
                Value;

            string sportKind = isActivities ?
                root.Descendants(ns1 + "Activities").First().Descendants(ns1 + "Activity").First().Attribute("Sport").
                Value :
                string.Empty;
            
            try
            {
                IEnumerable<Activity> activities = LoadTcxTracks(root, ns1);
                if (activities == null || activities.Count() == 0) return null;
                exercise.Duration = new TimeSpan(0, 0, (int)activities.Sum(ac => ac.Laps.Sum(a => a.TotalTimeSeconds)));
                exercise.Date = DateTime.Parse(id).ToUniversalTime();
                var totalDistance = activities.Sum(ac => ac.Laps.Sum(a => a.DistanceMeters));
                if (totalDistance > 0.5)
                {
                    exercise.Distance = Math.Round(totalDistance / 1000, 2);
                }

                var avgHeartrate = activities.Average(ac => ac.Laps.Average(a => a.AverageHeartRateBpm));
                if (avgHeartrate > 0)
                {
                    exercise.Heartrate = (int)avgHeartrate;
                }
                var maxHeartrate = activities.Max(ac => ac.Laps.Max(a => a.MaximumHeartRateBpm));
                if (maxHeartrate > 0)
                {
                    exercise.HeartrateMax = maxHeartrate;
                }
                var avgCadence = activities.Average(ac => ac.Laps.Average(a => a.Cadence));
                if (avgCadence > 0)
                {
                    exercise.Cadence = (int)avgCadence;
                }
                var maxCadence = activities.Max(ac => ac.Laps.Max(a => a.Cadence));
                if (maxCadence > 0)
                {
                    exercise.CadenceMax = maxCadence;
                }

                Disciplines disciplines;
                Enum.TryParse(sportKind, true, out disciplines);
                
                //False nix geparst wurde, wird die Standardsportart verwendet
                if (exercise.SportType == null)
                {
                    exercise.SportType = new SportType();
                }
                exercise.SportType.Type = (int)disciplines;
            }
            catch (Exception exc)
            {
                throw new Exception("Import fails", exc);
            }
            return exercise;
        }

        public IEnumerable<Activity> LoadTcxTracks(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            XElement root = XElement.Load(filePath);
            XNamespace ns1 = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";
            var activities = LoadTcxTracks(root, ns1);
            activities = SmoothTracks(activities);
            return activities;
        }

        private IEnumerable<Activity> SmoothTracks(IEnumerable<Activity> activities)
        {
            var newActivities = new List<Activity>();

            foreach (var activity in activities)
            {
                var newLaps = new List<Lap>();

                foreach (Lap lap in activity.Laps)
                {
                    var newLap = new Lap
                    {
                        AverageHeartRateBpm = lap.AverageHeartRateBpm,
                        Cadence = lap.Cadence,
                        TriggerMethod = lap.TriggerMethod,
                        Calories = lap.Calories,
                        DistanceMeters = lap.DistanceMeters,
                        Intensity = lap.Intensity,
                        TotalTimeSeconds = lap.TotalTimeSeconds,
                        Notes = lap.Notes,
                        MaximumSpeed = lap.MaximumSpeed,
                        MaximumHeartRateBpm = lap.MaximumHeartRateBpm
                    };

                    var newTracks = new List<Track>();

                    foreach (Track track in lap.Tracks)
                    {
                        var newTrackPoints = new List<TrackPoint>();
                        var avgTrackPoints = new List<TrackPoint>();

                        for (int i = 0; i < track.TrackPoints.Count; i++)
                        {
                            TrackPoint trackPoint = track.TrackPoints[i];

                            if (i > 0 && i % Smoothfactor == 0)
                            {
                                var newPoint = new TrackPoint();
                                newPoint.AltitudeMeters = avgTrackPoints.Average(t => t.AltitudeMeters);
                                newPoint.Cadence = (int)avgTrackPoints.Average(t => t.Cadence);
                                newPoint.DistanceMeters = trackPoint.DistanceMeters;
                                newPoint.HeartRateBpm = (int)avgTrackPoints.Average(t => t.HeartRateBpm);
                                newPoint.Positionx = trackPoint.Positionx;
                                newPoint.SensorState = trackPoint.SensorState;
                                newPoint.Time = trackPoint.Time;
                                //HACK prüfen ob das hinkommt oder besser ein Schnitt gebildet werden soll, da ich keine Testdaten hab
                                newPoint.Timex = trackPoint.Timex;
                                newTrackPoints.Add(newPoint);
                                avgTrackPoints.Clear();
                            }
                            else
                            {
                                avgTrackPoints.Add(trackPoint);
                            }
                        }
                        newTracks.Add(new Track { TrackPoints = newTrackPoints });
                        //track.TrackPoints = newTrackPoints;
                    }

                    newLap.Tracks = newTracks;
                    newLaps.Add(newLap);
                }

                newActivities.Add(new Activity { Id = activity.Id, Sport = activity.Sport, Laps = newLaps });

            }
            return newActivities;
        }

        private IEnumerable<Activity> LoadTcxTracks(XElement root, XNamespace ns1)
        {
            bool isActivities = (root.Descendants(ns1 + "Activities").FirstOrDefault() != null);

            var tcxKind = isActivities ? "Activity" : "Course";
                
            var usCulture = new CultureInfo("en-us");
            IEnumerable<Activity> activities = from activityElement in root.Descendants(ns1 + tcxKind)
                                               select new Activity
                                                          {
                                                              Id =
                                                                  activityElement.Element(ns1 + "Id") != null
                                                                      ? activityElement.Element(ns1 + "Id").Value
                                                                      : String.Empty,
                                                              Laps =
                                                                  (from lapElement in
                                                                       activityElement.Descendants(ns1 + "Lap")
                                                                   select new Lap
                                                                              {
                                                                                  TotalTimeSeconds =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "TotalTimeSeconds") !=
                                                                                      null
                                                                                          ? Convert.ToDouble(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "TotalTimeSeconds")
                                                                                                  .
                                                                                                  Value, usCulture)
                                                                                          : 0.00,
                                                                                  DistanceMeters =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "DistanceMeters") !=
                                                                                      null
                                                                                          ? Convert.ToDouble(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "DistanceMeters")
                                                                                                  .Value, usCulture)
                                                                                          : 0.00,
                                                                                  MaximumSpeed =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "MaximumSpeed") !=
                                                                                      null
                                                                                          ? Convert.ToDouble(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "MaximumSpeed")
                                                                                                  .Value, usCulture)
                                                                                          : 0.00,
                                                                                  Calories =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "Calories") !=
                                                                                      null
                                                                                          ? Convert.ToInt16(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "Calories")
                                                                                                  .Value, usCulture)
                                                                                          : 0,
                                                                                  AverageHeartRateBpm =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "AverageHeartRateBpm") !=
                                                                                      null
                                                                                          ? Convert.ToInt16(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "AverageHeartRateBpm")
                                                                                                  .Value, usCulture)
                                                                                          : 0,
                                                                                  MaximumHeartRateBpm =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "MaximumHeartRateBpm") !=
                                                                                      null
                                                                                          ? Convert.ToInt16(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "MaximumHeartRateBpm")
                                                                                                  .Value, usCulture)
                                                                                          : 0,
                                                                                  Intensity =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "Intensity") !=
                                                                                      null
                                                                                          ? lapElement.Element(ns1 +
                                                                                                               "Intensity")
                                                                                                .
                                                                                                Value
                                                                                          : "",
                                                                                  Cadence =
                                                                                      lapElement.Element(ns1 + "Cadence") !=
                                                                                      null
                                                                                          ? Convert.ToInt16(
                                                                                              lapElement.Element(ns1 +
                                                                                                                 "Cadence")
                                                                                                  .Value, usCulture)
                                                                                          : 0,
                                                                                  TriggerMethod =
                                                                                      lapElement.Element(ns1 +
                                                                                                         "TriggerMethod") !=
                                                                                      null
                                                                                          ? lapElement.Element(ns1 +
                                                                                                               "TriggerMethod")
                                                                                                .Value
                                                                                          : "",
                                                                                  Notes =
                                                                                      lapElement.Element(ns1 + "Notes") !=
                                                                                      null
                                                                                          ? lapElement.Element(ns1 +
                                                                                                               "Notes")
                                                                                                .Value
                                                                                          : "",
                                                                                  Tracks =
                                                                                      (from trackElement in
                                                                                           lapElement.Descendants(ns1 +
                                                                                                                  "Track")
                                                                                       select new Track
                                                                                                  {
                                                                                                      TrackPoints =
                                                                                                          (from
                                                                                                               trackPointElement
                                                                                                               in
                                                                                                               trackElement
                                                                                                               .
                                                                                                               Descendants
                                                                                                               (
                                                                                                                   ns1 +
                                                                                                                   "Trackpoint")
                                                                                                           select
                                                                                                               new TrackPoint
                                                                                                                   {
                                                                                                                       Timex
                                                                                                                           =
                                                                                                                           trackPointElement
                                                                                                                               .
                                                                                                                               Element
                                                                                                                               (ns1 +
                                                                                                                                "Time") !=
                                                                                                                           null
                                                                                                                               ? Convert
                                                                                                                                     .
                                                                                                                                     ToString
                                                                                                                                     (trackPointElement
                                                                                                                                          .
                                                                                                                                          Element
                                                                                                                                          (ns1 +
                                                                                                                                           "Time")
                                                                                                                                          .
                                                                                                                                          Value)
                                                                                                                               : "",
                                                                                                                       Time
                                                                                                                           =
                                                                                                                           DateTime
                                                                                                                           .
                                                                                                                           Parse
                                                                                                                           (trackPointElement
                                                                                                                                .
                                                                                                                                Element
                                                                                                                                (ns1 +
                                                                                                                                 "Time") !=
                                                                                                                            null
                                                                                                                                ? Convert
                                                                                                                                      .
                                                                                                                                      ToString
                                                                                                                                      (trackPointElement
                                                                                                                                           .
                                                                                                                                           Element
                                                                                                                                           (ns1 +
                                                                                                                                            "Time")
                                                                                                                                           .
                                                                                                                                           Value)
                                                                                                                                : "")
                                                                                                                       ,
                                                                                                                       AltitudeMeters
                                                                                                                           =
                                                                                                                           trackPointElement
                                                                                                                               .
                                                                                                                               Element
                                                                                                                               (ns1 +
                                                                                                                                "AltitudeMeters") !=
                                                                                                                           null
                                                                                                                               ? Convert
                                                                                                                                     .
                                                                                                                                     ToDouble
                                                                                                                                     (trackPointElement
                                                                                                                                          .
                                                                                                                                          Element
                                                                                                                                          (ns1 +
                                                                                                                                           "AltitudeMeters")
                                                                                                                                          .
                                                                                                                                          Value,
                                                                                                                                      usCulture)
                                                                                                                               : 0.0,
                                                                                                                       DistanceMeters
                                                                                                                           =
                                                                                                                           trackPointElement
                                                                                                                               .
                                                                                                                               Element
                                                                                                                               (ns1 +
                                                                                                                                "DistanceMeters") !=
                                                                                                                           null
                                                                                                                               ? Math.Round(Convert.
                                                                                                                                     ToDouble
                                                                                                                                     (trackPointElement
                                                                                                                                          .
                                                                                                                                          Element
                                                                                                                                          (ns1 +
                                                                                                                                           "DistanceMeters")
                                                                                                                                          .
                                                                                                                                          Value,
                                                                                                                                      usCulture), 0)
                                                                                                                               : 0.0,
                                                                                                                       HeartRateBpm
                                                                                                                           =
                                                                                                                           trackPointElement
                                                                                                                               .
                                                                                                                               Element
                                                                                                                               (ns1 +
                                                                                                                                "HeartRateBpm") !=
                                                                                                                           null
                                                                                                                               ? Convert
                                                                                                                                     .
                                                                                                                                     ToInt16
                                                                                                                                     (trackPointElement
                                                                                                                                          .
                                                                                                                                          Element
                                                                                                                                          (ns1 +
                                                                                                                                           "HeartRateBpm")
                                                                                                                                          .
                                                                                                                                          Value,
                                                                                                                                      usCulture)
                                                                                                                               : 0,
                                                                                                                       Cadence
                                                                                                                           =
                                                                                                                           trackPointElement
                                                                                                                               .
                                                                                                                               Element
                                                                                                                               (ns1 +
                                                                                                                                "Cadence") !=
                                                                                                                           null
                                                                                                                               ? Convert
                                                                                                                                     .
                                                                                                                                     ToInt16
                                                                                                                                     (trackPointElement
                                                                                                                                          .
                                                                                                                                          Element
                                                                                                                                          (ns1 +
                                                                                                                                           "Cadence")
                                                                                                                                          .
                                                                                                                                          Value,
                                                                                                                                      usCulture)
                                                                                                                               : 0,
                                                                                                                       SensorState
                                                                                                                           =
                                                                                                                           trackPointElement
                                                                                                                               .
                                                                                                                               Element
                                                                                                                               (ns1 +
                                                                                                                                "SensorState") !=
                                                                                                                           null
                                                                                                                               ? trackPointElement
                                                                                                                                     .
                                                                                                                                     Element
                                                                                                                                     (ns1 +
                                                                                                                                      "SensorState")
                                                                                                                                     .
                                                                                                                                     Value
                                                                                                                               : "",
                                                                                                                       Positionx
                                                                                                                           =
                                                                                                                           ((from
                                                                                                                                 positionElement
                                                                                                                                 in
                                                                                                                                 trackPointElement
                                                                                                                                 .
                                                                                                                                 Descendants
                                                                                                                                 (ns1 +
                                                                                                                                  "Position")
                                                                                                                             select
                                                                                                                                 new Position
                                                                                                                                     {
                                                                                                                                         LatitudeDegrees
                                                                                                                                             =
                                                                                                                                             Convert
                                                                                                                                             .
                                                                                                                                             ToDouble
                                                                                                                                             (positionElement
                                                                                                                                                  .
                                                                                                                                                  Element
                                                                                                                                                  (ns1 +
                                                                                                                                                   "LatitudeDegrees")
                                                                                                                                                  .
                                                                                                                                                  Value,
                                                                                                                                              usCulture),
                                                                                                                                         LongitudeDegrees
                                                                                                                                             =
                                                                                                                                             Convert
                                                                                                                                             .
                                                                                                                                             ToDouble
                                                                                                                                             (positionElement
                                                                                                                                                  .
                                                                                                                                                  Element
                                                                                                                                                  (ns1 +
                                                                                                                                                   "LongitudeDegrees")
                                                                                                                                                  .
                                                                                                                                                  Value,
                                                                                                                                              usCulture)
                                                                                                                                     })
                                                                                                                           .
                                                                                                                           ToList
                                                                                                                           ())
                                                                                                                   }).
                                                                                                          ToList
                                                                                                          ()
                                                                                                  }).ToList()
                                                                              }).ToList()
                                                          };

            return activities;
        }
    }
}