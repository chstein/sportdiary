using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sporty.Helper;

namespace Sporty.Tests.Controllers
{
    /// <summary>
    /// Summary description for ExerciseController
    /// </summary>
    [TestClass]
    public class ExerciseControllerTest
    {
        public ExerciseControllerTest()
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

        [TestMethod]
        public void ExerciseTableViewWithCompleteUrl_Page_ShouldReturnTrue()
        {
            string sampleUrl = "http://localhost:52196/Exercise/ExerciseTableView?page=2&sort=1&dir=desc";
            UrlTableHelper helper = new UrlTableHelper(new Uri(sampleUrl));

            Assert.IsTrue(helper.Page == 2);
        }

        [TestMethod]
        public void ExerciseTableViewWithNoneUrl_Page_ShouldReturnFalse()
        {
            string sampleUrl = "http://localhost:52196/Exercise/ExerciseTableView";
            UrlTableHelper helper = new UrlTableHelper(new Uri(sampleUrl));

            Assert.IsNull(helper.Page);
        }

        [TestMethod]
        public void ExerciseTableViewWithCompleteUrl_SortColumn_ShouldReturnTrue()
        {
            string sampleUrl = "http://localhost:52196/Exercise/ExerciseTableView?page=2&sort=1&dir=desc";
            UrlTableHelper helper = new UrlTableHelper(new Uri(sampleUrl));

            Assert.IsTrue(helper.SortColumn == 1);
        }

        [TestMethod]
        public void ExerciseTableViewWithCompleteUrl_SortDirection_ShouldReturnTrue()
        {
            string sampleUrl = "http://localhost:52196/Exercise/ExerciseTableView?page=2&sort=1&dir=desc";
            UrlTableHelper helper = new UrlTableHelper(new Uri(sampleUrl));

            Assert.IsTrue(helper.SortDirection == SortDirection.Descending);
        }
    }
}
