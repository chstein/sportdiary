using System;
using System.Collections;
using System.IO;
using System.Linq;
using Sporty.DataModel;

namespace Sporty.Business.IO
{
    public class HrmParser : ExerciseParser
    {
        // Fields
        private string[] _fileContentsLines;
        private bool _hasAltitude;
        private bool _hasCadence;
        private bool _hasPower;
        private bool _hasSpeed;
        private uint _recordingInterval;
        private bool _useMetricUnits;

        // Methods
        private Hashtable GetMarkNotes(string[] intNotesBlockLines)
        {
            var hashtable = new Hashtable();
            if (intNotesBlockLines.Length > 0)
            {
                for (int i = 0; i < intNotesBlockLines.Length; i++)
                {
                    string[] strArray = intNotesBlockLines[i].Split(new[] {'\t'});
                    if (strArray.Length == 2)
                    {
                        int key = int.Parse(strArray[0]);
                        string str = strArray[1];
                        if ((str != null) && (str.Length > 0))
                        {
                            hashtable.Add(key, str);
                        }
                    }
                }
            }
            return hashtable;
        }

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
            var file = new FileInfo(filePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Exercise file not found!", filePath);
            }
            _fileContentsLines = ReadHelper.ReadFileToLineArray(filePath);
            string[] blockLines = ReadHelper.GetBlockLines("Params", _fileContentsLines);
            if (blockLines.Length == 0)
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmParamsBlockNotFound"));
            }
            string valueFromBlock = ReadHelper.GetValueFromBlock(blockLines, "Version");
            if ((valueFromBlock == null) || (!valueFromBlock.Equals("106") && !valueFromBlock.Equals("107")))
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidVersion"));
            }
            ReadHelper.GetValueFromBlock(blockLines, "Monitor");
            string recordingMode = ReadHelper.GetValueFromBlock(blockLines, "SMode");
            if (((recordingMode == null) || (recordingMode.Length < 8)) || (recordingMode.Length > 9))
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidRecordingMode"));
            }
            _hasSpeed = recordingMode[0] == '1';
            _hasCadence = recordingMode[1] == '1';
            _hasAltitude = recordingMode[2] == '1';
            _hasPower = recordingMode[3] == '1';
            _useMetricUnits = recordingMode[7] == '0';
            string dateStr = ReadHelper.GetValueFromBlock(blockLines, "Date");
            if ((dateStr == null) || (dateStr.Length != 8))
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidDate"));
            }
            int year = int.Parse(dateStr.Substring(0, 4));
            int month = int.Parse(dateStr.Substring(4, 2));
            int day = int.Parse(dateStr.Substring(6, 2));
            string startTimeStr = ReadHelper.GetValueFromBlock(blockLines, "StartTime");
            if (startTimeStr == null)
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidStartTime"));
            }
            string[] startTimeSplit = startTimeStr.Split(new[] {':', '.'});
            if (startTimeSplit.Length != 4)
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidStartTime"));
            }
            int hour = int.Parse(startTimeSplit[0]);
            int minute = int.Parse(startTimeSplit[1]);
            int second = int.Parse(startTimeSplit[2]);
            var time = new DateTime(year, month, day, hour, minute, second);
            string lengthStr = ReadHelper.GetValueFromBlock(blockLines, "Length");
            if (lengthStr == null)
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidDuration"));
            }
            string[] durationSplits = lengthStr.Split(new[] {':', '.'});
            int totalSeconds = 0;
            if (durationSplits.Length == 4)
            {
                int num7 = int.Parse(durationSplits[0]);
                int num8 = int.Parse(durationSplits[1]);
                int num9 = int.Parse(durationSplits[2]);
                int num10 = int.Parse(durationSplits[3]);
                totalSeconds = ((((num7*60)*60) + (num8*60)) + num9) + (int) ((num10)/10.0);
            }
            else if (durationSplits.Length == 3)
            {
                int num8 = int.Parse(durationSplits[0]);
                int num9 = int.Parse(durationSplits[1]);
                int num10 = int.Parse(durationSplits[2]);
                totalSeconds = (((num8*60)) + num9) + (int) ((num10)/10.0);
            }
            else
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidDuration"));
            }


            string intervalCountStr = ReadHelper.GetValueFromBlock(blockLines, "Interval");
            if (intervalCountStr == null)
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmInvalidInterval"));
            }
            _recordingInterval = uint.Parse(intervalCountStr);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
                // PathHelper.GetFileNameWithoutExtension(file.FullName);
            string notes = "";
            string[] noteStr = ReadHelper.GetBlockLines("Note", _fileContentsLines);
            if (noteStr.Length > 0)
            {
                fileNameWithoutExtension = noteStr[0];
                if (noteStr.Length > 1)
                {
                    for (int i = 1; i < noteStr.Length; i++)
                    {
                        notes = notes + noteStr[i] + " ";
                    }
                    notes = notes.Substring(0, notes.Length - 1);
                }
            }
            string[] distanceStr = ReadHelper.GetBlockLines("Trip", _fileContentsLines);
            double totalDistance = 0;
            if ((distanceStr.Length == 8) && _hasSpeed)
            {
                totalDistance = ((int.Parse(distanceStr[0]))/10.0)*1000.0;
                if (!_useMetricUnits)
                {
                    //this._totalDistance = Length.Mile2Km(this._totalDistance);
                }
            }
            if (exercise == null)
                exercise = new Exercise();

            exercise.Date = time;
            if (!String.IsNullOrEmpty(notes))
                exercise.Description = notes;
            if (totalDistance > 0)
                exercise.Distance = totalDistance;
            if (totalSeconds > 0)
                exercise.Duration = new TimeSpan(0, 0, totalSeconds);

            //Exercise exercise = new Exercise(file, ErgoGraphFileType.PolarHrmExercise);
            ParseSamples(exercise);
            //this.ParseMarks(exercise);
            //exercise.Name = fileNameWithoutExtension;
            //exercise.Description = notes;
            //exercise.Timestamp = time;
            //exercise.ProgramType = ProgramType.DistanceHeight;
            //bool hasHeartrateData = exercise.EntryList.MaxEntry.Heartrate > 0;
            //exercise.SetRecordingMode(true, this._hasPower, hasHeartrateData, this._hasCadence, this._hasSpeed, false, this._hasAltitude, false);
            return exercise;
        }

        //private void ParseMarks(Exercise exercise)
        //{
        //    string[] blockLines = ReadHelper.GetBlockLines("IntTimes", this._fileContentsLines);
        //    string[] intNotesBlockLines = ReadHelper.GetBlockLines("IntNotes", this._fileContentsLines);
        //    if ((blockLines.Length != 0) && ((blockLines.Length % 5) == 0))
        //    {
        //        int num = blockLines.Length / 5;
        //        Hashtable markNotes = this.GetMarkNotes(intNotesBlockLines);
        //        for (int i = 0; i < num; i++)
        //        {
        //            string[] strArray3 = blockLines[i * 5].Split(new char[] { '\t' });
        //            if (strArray3.Length == 5)
        //            {
        //                string[] strArray4 = strArray3[0].Split(new char[] { ':', '.' });
        //                if (strArray4.Length == 4)
        //                {
        //                    int num3 = int.Parse(strArray4[0]);
        //                    int num4 = int.Parse(strArray4[1]);
        //                    int num5 = int.Parse(strArray4[2]);
        //                    int num6 = int.Parse(strArray4[3]);
        //                    double time = ((((num3 * 60) * 60) + (num4 * 60)) + num5) + (((double) num6) / 10.0);
        //                    int key = i + 1;
        //                    string name = "Lap " + key.ToString();
        //                    if (((markNotes != null) && (markNotes.Count > 0)) && markNotes.ContainsKey(key))
        //                    {
        //                        name = markNotes[key].ToString();
        //                    }
        //                    if (time > 0.0)
        //                    {
        //                        exercise.AddTimeMark(time, name);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void ParseSamples(Exercise exercise)
        {
            string[] blockLines = ReadHelper.GetBlockLines("HRData", _fileContentsLines);
            if (blockLines.Length == 0)
            {
                //throw new Exception(ResxManager.Instance.GetString("HrmHRDataBlockNotFound"));
            }
            //ExerciseEntryList list = exercise.CreateEntryList(BaseType.Time, this._recordingInterval);
            var heartrateList = new int[blockLines.Length];
            var speedList = new double[blockLines.Length];
            for (int i = 0; i < blockLines.Length; i++)
            {
                int index = 0;
                string[] strArray2 = blockLines[i].Split(new[] {'\t'});
                //ExerciseEntry entry = new ExerciseEntry();
                //entry.Time = (uint)(i * this._recordingInterval);
                //entry.Heartrate = uint.Parse(strArray2[index]);
                heartrateList[i] = Int32.Parse(strArray2[index]);

                index++;
                if ((strArray2.Length > index) && _hasSpeed)
                {
                    double miles = (int.Parse(strArray2[index]))/10.0;
                    //if (!this._useMetricUnits)
                    //{
                    //    miles = Length.Mile2Km(miles);
                    //}
                    speedList[i] = miles;
                    index++;
                }
                //if ((strArray2.Length > index) && this._hasCadence)
                //{
                //    entry.Cadence = ConvertHelper.ToDouble(strArray2[index], true);
                //    index++;
                //}
                //if ((strArray2.Length > index) && this._hasAltitude)
                //{
                //    int num4 = int.Parse(strArray2[index]);
                //    if (!this._useMetricUnits)
                //    {
                //        num4 = (int)Length.Ft2M((double)num4);
                //    }
                //    entry.TrainingValue = num4;
                //    index++;
                //}
                //if ((strArray2.Length > index) && this._hasPower)
                //{
                //    entry.Power = ConvertHelper.ToDouble(strArray2[index], true);
                //    index++;
                //}
                //list.Add(entry);
            }
            exercise.Heartrate = (int) heartrateList.Average();
            exercise.Speed = speedList.Average();
            //if (this._hasSpeed)
            //{
            //    double num5 = 0.0;
            //    foreach (ExerciseEntry entry2 in list)
            //    {
            //        entry2.Distance = num5;
            //        num5 += ((entry2.Speed * this._recordingInterval) / 3600.0) * 1000.0;
            //    }
            //    if ((((this._totalDistance > 0.0) && (this._totalSeconds > 0.0)) && ((list != null) && (list.Count > 0))) && (list.Count >= (this._totalSeconds / ((double)this._recordingInterval))))
            //    {
            //        double num6 = num5 / this._totalDistance;
            //        foreach (ExerciseEntry entry3 in list)
            //        {
            //            entry3.Distance /= num6;
            //        }
            //    }
            //}
        }
    }
}