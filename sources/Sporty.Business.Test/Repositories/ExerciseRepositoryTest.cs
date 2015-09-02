//using Sporty.Business.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using Sporty.DataModel;
//using Sporty.ViewModel;
//using System.Web.UI.WebControls;
//using System.Collections.Generic;

//namespace Sport.Business.Test
//{


//    /// <summary>
//    ///This is a test class for ExerciseRepositoryTest and is intended
//    ///to contain all ExerciseRepositoryTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class ExerciseRepositoryTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        // 
//        //You can use the following additional attributes as you write your tests:
//        //
//        //Use ClassInitialize to run code before running the first test in the class
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //Use ClassCleanup to run code after all tests in a class have run
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //Use TestInitialize to run code before running each test
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //Use TestCleanup to run code after each test has run
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///A test for ExerciseRepository Constructor
//        ///</summary>
//        [TestMethod()]
//        public void ExerciseRepositoryConstructorTest()
//        {
//            IEntitiesDataContext context = null; // TODO: Initialize to an appropriate value
//            ExerciseRepository target = new ExerciseRepository(context);
//            Assert.Inconclusive("TODO: Implement code to verify target");
//        }

//        /// <summary>
//        ///A test for Delete
//        ///</summary>
//        [TestMethod()]
//        public void DeleteTest()
//        {
//            IEntitiesDataContext context = null; // TODO: Initialize to an appropriate value
//            ExerciseRepository target = new ExerciseRepository(context); // TODO: Initialize to an appropriate value
//            Nullable<Guid> userId = new Nullable<Guid>(); // TODO: Initialize to an appropriate value
//            int id = 0; // TODO: Initialize to an appropriate value
//            target.Delete(userId, id);
//            Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        }

//        /// <summary>
//        ///A test for GetElement
//        ///</summary>
//        [TestMethod()]
//        public void GetElementTest()
//        {
//            IEntitiesDataContext context = null; // TODO: Initialize to an appropriate value
//            ExerciseRepository target = new ExerciseRepository(context); // TODO: Initialize to an appropriate value
//            Nullable<Guid> userId = new Nullable<Guid>(); // TODO: Initialize to an appropriate value
//            int id = 0; // TODO: Initialize to an appropriate value
//            ExerciseDetailsView expected = null; // TODO: Initialize to an appropriate value
//            ExerciseDetailsView actual;
//            actual = target.GetElement(userId, id);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for GetExercises
//        ///</summary>
//        [TestMethod()]
//        public void GetExercisesTest()
//        {
//            IEntitiesDataContext context = null; // TODO: Initialize to an appropriate value
//            ExerciseRepository target = new ExerciseRepository(context); // TODO: Initialize to an appropriate value
//            int monthValue = 0; // TODO: Initialize to an appropriate value
//            int yearValue = 0; // TODO: Initialize to an appropriate value
//            CalendarViewModel expected = null; // TODO: Initialize to an appropriate value
//            CalendarViewModel actual;
//            actual = target.GetExercises(monthValue, yearValue);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for GetPagedExercise
//        ///</summary>
//        [TestMethod()]
//        public void GetPagedExerciseTest()
//        {
//            IEntitiesDataContext context = null; // TODO: Initialize to an appropriate value
//            ExerciseRepository target = new ExerciseRepository(context); // TODO: Initialize to an appropriate value
//            Nullable<Guid> userId = new Nullable<Guid>(); // TODO: Initialize to an appropriate value
//            int itemsPerPage = 0; // TODO: Initialize to an appropriate value
//            Nullable<int> page = new Nullable<int>(); // TODO: Initialize to an appropriate value
//            string sortColumnName = string.Empty; // TODO: Initialize to an appropriate value
//            SortDirection sortDirection = new SortDirection(); // TODO: Initialize to an appropriate value
//            IEnumerable<ExerciseView> expected = null; // TODO: Initialize to an appropriate value
//            IEnumerable<ExerciseView> actual;
//            actual = target.GetPagedExercise(userId, itemsPerPage, page, sortColumnName, sortDirection);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }

//        /// <summary>
//        ///A test for Save
//        ///</summary>
//        [TestMethod()]
//        public void SaveTest()
//        {
//            IEntitiesDataContext context = null; // TODO: Initialize to an appropriate value
//            ExerciseRepository target = new ExerciseRepository(context); // TODO: Initialize to an appropriate value
//            Nullable<Guid> userId = new Nullable<Guid>(); // TODO: Initialize to an appropriate value
//            ExerciseDetailsView element = null; // TODO: Initialize to an appropriate value
//            target.Save(userId, element);
//            Assert.Inconclusive("A method that does not return a value cannot be verified.");
//        }

//        public void TestCalenderView()
//        {
//            //var exerciseRepro = new ExerciseRepository()
//        }

//    }
//}
