using System.Collections.Generic;

namespace Sporty.ViewModel
{
    public class MetricNameBag
    {
        public static List<string> SleepQualityData = new List<string> { "", "Grauenhaft", "Schlecht", "Geht so", "Durchschnitt", "Gut", "Sehr gut", "Genial" };
        public static List<string> StressLevelData = new List<string> { "", "Kein Stress", "Sehr wenig", "Wenig", "Durchschnitt", "Viel", "Sehr viel", "Extrem" };
        public static List<string> MotivationData = new List<string>{"", "Extrem Unmotiviert", "Ziemlich unmotiviert", "Unmotiviert", "Wenig begeistert",
                                                                     "Unterm Schnitt", "Überm Schnitt", "Begeistert", "Motiviert", "Gut Motiviert", "Top motiviert", "Absolut Begeistert"};
        public static List<string> MoodData = new List<string> { "", "Niedergeschlagen", "Normal", "Besser als Normal" };
        public static List<string> SickData = new List<string> { "", "Völlig krank", "Ziemlich krank", "Krank", "Gesund", "Sehr gesund", "Superman" };
        public static List<string> YesterdaysTrainingData = new List<string> { "", "Schwere Glieder", "Normal", "Leicht", "Ruhetag" };
    }
}