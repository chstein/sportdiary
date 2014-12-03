using System;
using Sporty.Business.IO.Tcx;
using Sporty.DataModel;

namespace Sporty.Business.IO
{
    public abstract class ExerciseParser
    {
        public abstract Exercise ParseExercise(string filePath);
        public abstract Exercise ParseExercise(string filePath, Exercise exercise);

        public static ExerciseParser GetParser(string fileExtension)
        {
            ExerciseParser parser;
            switch (fileExtension.ToLower())
            {
                case ".tur":
                    parser = new TurParser();
                    break;
                case ".hrm":
                    parser = new HrmParser();
                    break;
                case ".gpx":
                    parser = new GpxParser();
                    break;
                case ".tcx":
                    parser = new TcxParser();
                    break;
                default:
                    throw new NotSupportedException("File format not supported!");
                    break;
            }
            return parser;
        }
    }
}