using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Sporty.DataModel;

namespace Sporty.Business.IO
{
    public class TurParser : ExerciseParser
    {
        // Fields
        private const string VERSION_HEADER_STRING = "HACtronic - Tour";
        public double[] DistanceInMetersList;
        public int[] HeartrateList;
        public double[] SpeedInKmhList;
        public long[] TimeInSecList;
        private byte[] fileContentsBytes;
        private string[] fileContentsLines;

        // Methods

        public override Exercise ParseExercise(string filePath)
        {
            return ParseExercise(filePath, new Exercise());
        }

        public override Exercise ParseExercise(string filePath, Exercise exercise)
        {
            var file = new FileInfo(filePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Exercise file not found!", filePath);
            }
            fileContentsLines = ReadHelper.ReadFileToLineArray(filePath);
            fileContentsBytes = ReadHelper.ReadFileToByteArray(filePath);
            string str = ReadLine(0);
            if ((str == null) || !str.Equals("HACtronic - Tour"))
            {
                //throw new Exception(ResxManager.Instance.GetString("TurInvalidHeader"));
            }
            ReadInteger(1);
            ReadInteger(2);
            string str2 = ReadLine(4);
            string date = ReadLine(7);
            string str4 = ReadLine(8);
            DateTime time = ReadDateTime(date, str4);
            int num = ReadInteger(9);
            string notes = "";
            if (num > 0)
            {
                notes = notes + ReadLine(10);
            }
            if (num > 1)
            {
                notes = notes + ReadLine(11);
            }
            if (num > 2)
            {
                notes = notes + ReadLine(12);
            }
            int sampleCount = ReadInteger(0x35 + num);
            int headerByteCount = 0;
            for (int i = 0; i < (0x36 + num); i++)
            {
                headerByteCount += fileContentsLines[i].Length + 1;
            }
            int blockSize = 20;

            if (exercise == null)
                exercise = new Exercise(); //file, ErgoGraphFileType.CicloTurExercise);

            int sampleRate = ReadSampleRate(headerByteCount, sampleCount, blockSize);
            ParseSamples(exercise, headerByteCount, sampleCount, blockSize, sampleRate);
            int sourceIndex = headerByteCount + (sampleCount*blockSize);
            int length = fileContentsBytes.Length - sourceIndex;
            var destinationArray = new byte[length];
            Array.Copy(fileContentsBytes, sourceIndex, destinationArray, 0, length);
            string[] marksLines =
                Encoding.Default.GetString(destinationArray, 1, destinationArray.Length - 1).Split(new[] {'\n'});
            //this.ParseMarks(exercise, marksLines);
            //exercise.Name = str2;
            exercise.Description = notes;
            exercise.Date = time;
            //exercise.ProgramType = ProgramType.DistanceHeight;
            //bool hasHeartrateData = exercise.EntryList.MaxEntry.Heartrate > 0;
            //bool hasCadenceData = exercise.EntryList.MaxEntry.Cadence > 0.0;
            //bool hasSpeedData = exercise.EntryList.MaxEntry.Speed > 0.0;
            //bool hasTrainingValueData = exercise.EntryList.MaxEntry.TrainingValue > 0.0;
            //bool hasTemperatureData = exercise.EntryList.MaxEntry.Temperature > 0.0;
            //exercise.SetRecordingMode(true, false, hasHeartrateData, hasCadenceData, hasSpeedData, false, hasTrainingValueData, hasTemperatureData);
            return exercise;
        }

        //private void ParseMarks(Exercise exercise, string[] marksLines)
        //{
        //    int num = int.Parse(marksLines[0]);
        //    int index = 1;
        //    for (int i = 0; i < num; i++)
        //    {
        //        double time = ConvertHelper.ToDouble(marksLines[index]);
        //        string[] strArray = marksLines[index + 1].Split(new char[] { ';' });
        //        string name = strArray[0];
        //        string text1 = strArray[1];
        //        string text2 = strArray[2];
        //        if ((name == null) || (name.Length == 0))
        //        {
        //            name = "Lap " + ((i + 1)).ToString();
        //        }
        //        if (time > 0.0)
        //        {
        //            exercise.AddTimeMark(time, name);
        //        }
        //        index += 2;
        //    }
        //}

        private void ParseSamples(Exercise exercise, int headerByteCount, int sampleCount, int blockSize, int sampleRate)
        {
            //ExerciseEntryList list = exercise.CreateEntryList(BaseType.Time, (uint)sampleRate);

            double distance = 0.0;
            DistanceInMetersList = new double[sampleCount];
            HeartrateList = new int[sampleCount];
            SpeedInKmhList = new double[sampleCount];
            TimeInSecList = new long[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                var destinationArray = new byte[blockSize];
                int num3 = i*blockSize;
                Array.Copy(fileContentsBytes, headerByteCount + num3, destinationArray, 0, blockSize);
                var sample = new TurSample(destinationArray);
                //ExerciseEntry entry = new ExerciseEntry();
                TimeInSecList[i] = sample.Time;
                DistanceInMetersList[i] = sample.Distance*10.0;
                HeartrateList[i] = sample.Heartrate;
                //entry.TrainingValue = sample.Height;
                //entry.Cadence = sample.Cadence;
                double num4 = sample.Distance - distance;
                distance = sample.Distance;
                SpeedInKmhList[i] = ((num4/(sampleRate))*3.6)*10.0;
                //entry.Temperature = sample.Temperature;
                //list.Add(entry);
            }

            var duration = (int) TimeInSecList.Last();
            if (duration > 0) exercise.Duration = new TimeSpan(0, 0, duration);

            var heartrateAvg = (int) HeartrateList.Average();
            if (heartrateAvg > 0) exercise.Heartrate = heartrateAvg;

            double distanceInMeters = DistanceInMetersList.Last();
            if (distanceInMeters > 0) exercise.Distance = Math.Round(distanceInMeters/1000, 3);

            double speedAvg = SpeedInKmhList.Average();
            if (speedAvg > 0) exercise.Speed = Math.Round(speedAvg, 3);
        }

        private DateTime ReadDate(string date)
        {
            if (((date == null) || (date.Length == 0)) || ((date == "01.01.0000") || (date == "  .  .    ")))
            {
                return DateTime.MinValue;
            }
            string[] strArray = date.Split(new[] {'.'});
            int year = int.Parse(strArray[2]);
            int month = int.Parse(strArray[1]);
            return new DateTime(year, month, int.Parse(strArray[0]));
        }

        private DateTime ReadDateTime(string date, string time)
        {
            if (((date == null) || (date.Length == 0)) || ((date == "01.01.0000") || (date == "  .  .    ")))
            {
                return DateTime.MinValue;
            }
            string[] strArray = date.Split(new[] {'.'});
            string[] strArray2 = time.Split(new[] {':'});
            int year = int.Parse(strArray[2]);
            int month = int.Parse(strArray[1]);
            int day = int.Parse(strArray[0]);
            if ((time == null) || (time.Length == 0))
            {
                return new DateTime(year, month, day);
            }
            int hour = int.Parse(strArray2[0]);
            return new DateTime(year, month, day, hour, int.Parse(strArray2[1]), 0);
        }

        private double ReadDouble(int lineNumber)
        {
            double num;
            try
            {
                var provider = new NumberFormatInfo();
                provider.NumberDecimalDigits = 4;
                provider.NumberDecimalSeparator = ".";
                string s = ReadLine(lineNumber);
                if ((s == null) || (s.Length == 0))
                {
                    return 0.0;
                }
                num = double.Parse(s, provider);
            }
            catch (FormatException)
            {
                //throw new Exception(ResxManager.Instance.GetString("TurInvalidDouble"));
            }
            return num = 0;
        }

        private int ReadInteger(int lineNumber)
        {
            int num = 0;
            try
            {
                string s = ReadLine(lineNumber);
                if ((s == null) || (s.Length == 0))
                {
                    return 0;
                }
                num = int.Parse(s);
            }
            catch (FormatException)
            {
                //throw new Exception(ResxManager.Instance.GetString("TurInvalidInteger"));
            }
            return num;
        }

        private string ReadLine(int lineNumber)
        {
            return fileContentsLines[lineNumber];
        }

        private int ReadSampleRate(int headerByteCount, int sampleCount, int blockSize)
        {
            var destinationArray = new byte[blockSize];
            Array.Copy(fileContentsBytes, headerByteCount, destinationArray, 0, blockSize);
            var buffer2 = new byte[blockSize];
            Array.Copy(fileContentsBytes, headerByteCount + blockSize, buffer2, 0, blockSize);
            var sample = new TurSample(destinationArray);
            var sample2 = new TurSample(buffer2);
            return (int) (sample2.Time - sample.Time);
        }

        // Nested Types

        #region Nested type: LinePosition

        private enum LinePosition
        {
            AVERAGE_HEARTRATE = 0x13,
            AVERAGE_SPEED = 0x11,
            BEGIN_SAMPLES = 0x36,
            BIRTHDATE = 0x20,
            DISTANCE = 10,
            DURATION = 11,
            FINISH_PLACE = 6,
            HEARTRATE_LIMIT_LOWER = 0x24,
            HEARTRATE_LIMIT_UPPER = 0x23,
            HEIGHT_DIFFERENCE = 14,
            HEIGHT_MAX = 13,
            MAJOR_VERSION = 1,
            MATERIAL = 0x25,
            MINOR_VERSION = 2,
            NAME_1 = 30,
            NAME_2 = 0x1f,
            NOTE_LINE_1 = 10,
            NOTE_LINE_COUNT = 9,
            ODOMETER = 0x27,
            RECORDING_DEVICE = 20,
            RECORDING_MODE = 0x15,
            SAMPLE_COUNT = 0x35,
            SPORTS_CLUB = 0x21,
            START_DATE = 7,
            START_PLACE = 5,
            START_TIME = 8,
            TOTAL_EXERCISE_TIME = 40,
            TOTAL_HEIGHT_DOWN = 0x2a,
            TOTAL_HEIGHT_UP = 0x29,
            TOUR_NAME = 4,
            VERSION_HEADER = 0,
            WEIGHT_MATERIAL = 0x26,
            WEIGHT_PERSON = 0x22
        }

        #endregion
    }

    internal class TurSample
    {
        // Fields
        private readonly int _cadence;
        private readonly int _distance;
        private readonly int _heartrate;
        private readonly int _height;
        private readonly int _temperature;
        private readonly long _time;

        // Methods
        public TurSample(byte[] contents)
        {
            if (contents.Length != 20)
            {
                throw new Exception("The length of a Tur Sample must be 20 Bytes");
            }
            _time = BitConverter.ToInt32(contents, 4);
            _distance = BitConverter.ToInt32(contents, 8);
            _height = BitConverter.ToInt16(contents, 12);
            _heartrate = contents[14];
            _cadence = contents[15];
            _temperature = contents[0x10];
        }

        //public static byte[] GetBytes(ExerciseEntry entry)
        //{
        //    byte[] destinationArray = new byte[20];
        //    Array.Copy(BitConverter.GetBytes((uint)entry.Time), 0, destinationArray, 4, 4);
        //    int num = (int)Math.Round((double)(entry.Distance / 10.0), 0);
        //    Array.Copy(BitConverter.GetBytes((uint)num), 0, destinationArray, 8, 4);
        //    Array.Copy(BitConverter.GetBytes((ushort)entry.TrainingValue), 0, destinationArray, 12, 2);
        //    destinationArray[14] = (byte)entry.Heartrate;
        //    destinationArray[15] = (byte)((int)entry.Cadence);
        //    destinationArray[0x10] = (byte)entry.Temperature;
        //    return destinationArray;
        //}

        // Properties
        public int Cadence
        {
            get { return _cadence; }
        }

        public int Distance
        {
            get { return _distance; }
        }

        public int Heartrate
        {
            get { return _heartrate; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Temperature
        {
            get { return _temperature; }
        }

        public long Time
        {
            get { return _time; }
        }
    }
}