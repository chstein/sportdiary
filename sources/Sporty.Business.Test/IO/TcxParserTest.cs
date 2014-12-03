using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sporty.Business.IO.Tcx;

namespace Sport.Business.Test.IO
{
    [TestClass]
    public class TcxParserTest
    {
        [TestMethod]
        public void ParseExerciseWithValidDateTest()
        {
            var target = new TcxParser(); // TODO: Initialize to an appropriate value
            string filePath = @"C:\Projects\Sporty\Sporty\Upload\b5498922-b74b-45c9-b370-e932a1383ac1\2012-02-22-193123.tcx"; // TODO: Initialize to an appropriate value
            var exercise = target.ParseExercise(filePath);
            var resultDate = new DateTime(2012, 2, 22, 19, 31, 23).ToUniversalTime();
            Assert.IsTrue(exercise.Date == resultDate);
        }
    }
}
