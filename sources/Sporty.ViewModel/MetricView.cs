using System;

namespace Sporty.ViewModel
{
    public class MetricView
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }

        public double? Weight { get; set; }

        public int? RestingPulse { get; set; }

        public double? SleepDuration { get; set; }

        public short? SleepQuality { get; set; }

        public short? StressLevel { get; set; }

        public short? Motivation { get; set; }

        public short? Mood { get; set; }

        public short? Sick { get; set; }

        public string Description { get; set; }

        public bool IsNew { get; set; }
        
        public short? YesterdaysTraining { get; set; }
    }

    public enum YesterdaysTraining
    {
        WorseThanNormal = 1,
        Normal,
        BetterThanNormal,
        RestDay
    }

    public enum Sick
    {
        ExtremelySick = 1,
        VerySick,
        Sick,
        SlightlySick,
        Healthy,
        VeryHealthy,
        ExtremelyHealthy
    }

    public enum Motivation
    {
        ExtremelyUnmotivated = 1,
        VeryUnmotivated,
        Unmotivated,
        Uninspired,
        BelowAverage,
        AboveAverage,
        Inspired,
        Motivated,
        VeryMotivated,
        ExtremlyMotivated
    }

    public enum Mood
    {
        WorseThanNormal = 1,
        Normal,
        BetterThanNormal
    }

    public enum StressLevel
    {
        None = 1,
        VeryLow,
        Low,
        Average,
        High,
        VeryHigh,
        Extreme
    }

    public enum SleepQuality
    {
        Horrible = 1,
        Poor,
        Bad,
        Average,
        Good,
        Better,
        Best
    }
}