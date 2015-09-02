using System.Collections.Generic;
using Sporty.Business.IO.Tcx;

namespace Sporty.Business.Series
{
    public class PaceDataSeries : ExerciseDataSeries
    {
        private readonly IEnumerable<Activity> activities;
        
        public PaceDataSeries(IEnumerable<Activity> activities)
        {
            Type = "PACE";
            UnitY = "min/km";
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
                        int positionDiff = 1;
                        for (int i = 0; i < track.TrackPoints.Count; i++)
                        {
                            TrackPoint trackPoint = track.TrackPoints[i];
                            double pace = 0.0;
                            //wenn keine GPS Daten da sind, bleiben die Felder leer
                            //dann muss ein Zähler her der sich den letzten Ort merkt, um später die Speed zu berechnen
                            if (trackPoint.Positionx != null && trackPoint.Positionx.Count == 0)
                            {
                                positionDiff++;
                                continue;
                            }

                            if (i > 0 && (i >= positionDiff))
                            {
                                double timeDiffInMinutes =
                                    trackPoint.Time.Subtract(track.TrackPoints[i - positionDiff].Time).TotalMinutes;
                                double distanceToLastPoint = trackPoint.DistanceMeters -
                                                             track.TrackPoints[i - positionDiff].DistanceMeters;
                                if (timeDiffInMinutes > 0 && distanceToLastPoint > 0)
                                    pace = timeDiffInMinutes / (distanceToLastPoint / 1000);

                                distanceInMetersTemp = totalDistance + (trackPoint.DistanceMeters / 1000);

                                //daten in tcx/gps manchmal nicht korrekt
                                //Ein Punkt muss immer eine größere zumindest gleiche Distanz aufweisen, wie sein Vorgänger, wenn nicht, dann wird der Punkt nicht berücksichtigt
                                if (distanceInMetersTemp >= previousDistanceInMetersTemp)
                                {
                                    Points.Add(new object[] { distanceInMetersTemp, pace });
                                    previousDistanceInMetersTemp = distanceInMetersTemp;
                                }
                            }

                            //reset positionDiff
                            if (positionDiff > 1 && trackPoint.Positionx != null && trackPoint.Positionx.Count > 0)
                                positionDiff = 1;
                        }
                    }
                }
            }
        }
    }
}