using Sporty.Business.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Sport.Business.Test
{


    /// <summary>
    ///This is a test class for HrmParserTest and is intended
    ///to contain all HrmParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class HrmParserTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void ParseExerciseWithValidDurationTest()
        {
            HrmParser target = new HrmParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20100119.hrm"; // TODO: Initialize to an appropriate value
            var Exercise = target.ParseExercise(filePath);
            Assert.IsTrue(Exercise.Duration == new TimeSpan(0, 36, 54));
        }

        [TestMethod()]
        public void ParseExerciseWithEmptyDistanceTest()
        {
            HrmParser target = new HrmParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20100119.hrm"; // TODO: Initialize to an appropriate value
            var Exercise = target.ParseExercise(filePath);
            Assert.IsFalse(Exercise.Distance.HasValue);
        }

        [TestMethod()]
        public void ParseExerciseWithValidHrmTest()
        {
            HrmParser target = new HrmParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty.Business.Test\IO\20100119.hrm"; // TODO: Initialize to an appropriate value
            var Exercise = target.ParseExercise(filePath);
            Assert.IsTrue(Exercise.Heartrate == 127);
        }
    }
}
