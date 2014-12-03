using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sporty.Business.IO;

namespace Sport.Business.Test
{
    [TestClass()]
    public class TurParserTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void ParseExerciseWithValidDurationTest()
        {
            TurParser target = new TurParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20100119.tur"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Duration == new TimeSpan(0, 36, 54));

        }

        [TestMethod()]
        public void ParseExerciseWithValidDuration2Test()
        {
            TurParser target = new TurParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20110506.tur"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Duration == new TimeSpan(1, 54, 57));

        }

        [TestMethod()]
        public void ParseExerciseWithValidDistanceTest()
        {
            TurParser target = new TurParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20110506.tur"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Distance == 58.5);

        }


        [TestMethod()]
        public void ParseExerciseWithValidSpeedTest()
        {
            TurParser target = new TurParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20110506.tur"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Speed == 30.5);

        }

        [TestMethod()]
        public void ParseExerciseWithEmptyDistanceTest()
        {
            TurParser target = new TurParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20100119.tur"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsFalse(exercise.Distance.HasValue);

        }

        [TestMethod()]
        public void ParseExerciseWithValidHeartrateTest()
        {
            TurParser target = new TurParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20100119.tur"; // TODO: Initialize to an appropriate value
            var Exercise = target.ParseExercise(filePath);
            Assert.IsTrue(Exercise.Heartrate == 127);
        }
    }
}
