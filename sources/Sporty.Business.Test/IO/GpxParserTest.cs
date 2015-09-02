using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sporty.Business.IO;

namespace Sport.Business.Test.IO
{
    /// <summary>
    /// Summary description for GpxParserTest
    /// </summary>
    [TestClass]
    public class GpxParserTest
    {
        public GpxParserTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod()]
        public void ParseExerciseWithValidDateTest()
        {
            var target = new GpxParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\endomondotest.gpx"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Date.Date == new DateTime(2011, 6, 13));
        }

        [TestMethod()]
        public void ParseExerciseWithValidDurationTest()
        {
            var target = new GpxParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\endomondotest.gpx"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Duration == new TimeSpan(0, 34, 36));
        }

        [TestMethod()]
        public void ParseExerciseWithValidDistanceTest()
        {
            var target = new GpxParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\endomondotest.gpx"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            Assert.IsTrue(exercise.Distance == 6.11);
        }

        
    }
}
