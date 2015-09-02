using System;
using System.Collections.Generic;
using Sporty.Business.IO.Tcx;

namespace Sporty.Business.Series
{
    public class ElevationDataSeries : ExerciseDataSeries
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
        public ElevationDataSeries(IEnumerable<Activity> activities)
        {
            Type = "ELEVATION";
            UnitY = "m";
            this.activities = activities;
            CalculatePoints();
        }

        private void CalculatePoints()
        {
            Points = new List<object[]>();
            var totalDistance = 0.0;
            var distanceInMetersTemp = 0.0;
            var previousDistanceInMetersTemp = 0.0;
            
            foreach (var activity in activities)
            {
                totalDistance += distanceInMetersTemp;
                
                foreach (Lap lap in activity.Laps)
                {
                    foreach (Track track in lap.Tracks)
                    {
                        for (int i = 0; i < track.TrackPoints.Count; i++)
                        {
                            TrackPoint trackPoint = track.TrackPoints[i];
                            double altitude;
                            //wenn keine GPS Daten da sind, bleiben die Felder leer
                            if (Math.Abs(trackPoint.AltitudeMeters - 0.0) < 0.001 && i > 0)
                            {
                                altitude = track.TrackPoints[i - 1].AltitudeMeters;
                            }
                            else
                            {
                                altitude = trackPoint.AltitudeMeters;
                            }
                            if (altitude > 0)
                            {
                                //sobald auch nur ein Wert vorhanden ist, dann soll chart angezeigt werden
                                noData = false;
                            }
                            if (trackPoint.DistanceMeters > 0)
                            {
                                distanceInMetersTemp = totalDistance + (trackPoint.DistanceMeters / 1000);

                                //daten in tcx/gps manchmal nicht korrekt
                                //Ein Punkt muss immer eine größere zumindest gleiche Distanz aufweisen, wie sein Vorgänger, wenn nicht, dann wird der Punkt nicht berücksichtigt
                                if (distanceInMetersTemp >= previousDistanceInMetersTemp)
                                {
                                    Points.Add(new object[] { distanceInMetersTemp, altitude });
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