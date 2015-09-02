﻿using Sporty.Business.IO.Tcx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporty.Business.Series
{
    public class CadenceDataSeries : ExerciseDataSeries
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

        public CadenceDataSeries(IEnumerable<Activity> activities)
        {
            Type = "CADENCE";
            UnitY = "l/min";
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
                            int cadence;
                            //wenn keine Daten da sind, bleiben die Felder leer
                            if (trackPoint.Cadence == 0 && i > 0)
                            {
                                cadence = track.TrackPoints[i - 1].Cadence;
                            }
                            else
                            {
                                cadence = trackPoint.Cadence;
                            }
                            if (cadence > 0)
                            {
                                //sobald auch nur ein Wert vorhanden ist, dann soll chart angezeigt werden
                                noData = false;
                            }

                            if (useTimeForData)
                            {
                                Points.Add(new object[] { trackPoint.Time.Subtract(startTime).TotalMinutes, cadence });
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
                                        Points.Add(new object[] { distanceInMetersTemp, cadence });
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
