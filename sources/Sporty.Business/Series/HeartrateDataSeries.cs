using System;
using System.Collections.Generic;
using Sporty.Business.IO.Tcx;

namespace Sporty.Business.Series
{
    public class HeartrateDataSeries : ExerciseDataSeries
    {
        private readonly IEnumerable<Activity> activities;

        public bool IsEmpty
        {
            get
            {
                return noData || Points == null || Points.Count == 0;
            }
        }

        private bool noData = true;

        public HeartrateDataSeries(IEnumerable<Activity> activities)
        {
            Type = "HEARTRATE";
            UnitY = "bpm";
            this.activities = activities;
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            Points = new List<object[]>();
            var totalDistance = 0.0;
            var distanceInMetersTemp = 0.0;
            var previousDistanceInMetersTemp = 0.0;

            DateTime startTime = DateTime.MinValue;
            foreach (var activity in activities)
            {
                totalDistance += distanceInMetersTemp;

                foreach (Lap lap in activity.Laps)
                {
                    bool useTimeForData = (Math.Abs(lap.DistanceMeters - 0) < 0.1);

                    foreach (Track track in lap.Tracks)
                    {
                        for (int i = 0; i < track.TrackPoints.Count; i++)
                        {
                            TrackPoint trackPoint = track.TrackPoints[i];
                            if (i == 0 && startTime == DateTime.MinValue)
                            {
                                startTime = trackPoint.Time;
                            }
                            int heartrate;
                            //wenn keine GPS Daten da sind, bleiben die Felder leer
                            if (trackPoint.HeartRateBpm == 0 && i > 0)
                            {
                                heartrate = track.TrackPoints[i - 1].HeartRateBpm;
                            }
                            else
                            {
                                heartrate = trackPoint.HeartRateBpm;
                            }
                            if (heartrate > 0)
                            {
                                //sobald auch nur ein Wert vorhanden ist, dann soll chart angezeigt werden
                                noData = false;
                            }
                            if (useTimeForData)
                            {
                                Points.Add(new object[] { trackPoint.Time.Subtract(startTime).TotalMinutes, heartrate });
                            }
                            else
                            {
                                if (trackPoint.DistanceMeters > 0)
                                {
                                    distanceInMetersTemp = totalDistance + (trackPoint.DistanceMeters / 1000);

                                    //daten in tcx/gps manchmal nicht korrekt
                                    //Ein Punkt muss immer eine größere zumindest gleiche Distanz aufweisen, wie sein Vorgänger, wenn nicht, dann wird der Punkt nicht berücksichtigt
                                    if (distanceInMetersTemp >= previousDistanceInMetersTemp)
                                    {
                                        Points.Add(new object[] { distanceInMetersTemp, heartrate });
                                        previousDistanceInMetersTemp = distanceInMetersTemp;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}