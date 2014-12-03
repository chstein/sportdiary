//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Sporty.DataModel;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Rhino.Mocks;
//using Sporty.Business.Repositories;

//namespace SportBusinessTest
//{
//    /// <summary>
//    /// Summary description for TestExerciseService
//    /// </summary>
//    [TestClass]
//    public class TestExerciseService
//    {
//        public TestExerciseService()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext { get; set; }

//        #region Additional test attributes
//        //
//        // You can use the following additional attributes as you write your tests:
//        //
//        // Use ClassInitialize to run code before running the first test in the class
//        // [ClassInitialize()]
//        // public static void MyClassInitialize(TestContext testContext) { }
//        //
//        // Use ClassCleanup to run code after all tests in a class have run
//        // [ClassCleanup()]
//        // public static void MyClassCleanup() { }
//        //
//        // Use TestInitialize to run code before running each test 
//        // [TestInitialize()]
//        // public void MyTestInitialize() { }
//        //
//        // Use TestCleanup to run code after each test has run
//        // [TestCleanup()]
//        // public void MyTestCleanup() { }
//        //
//        #endregion

//        [TestMethod]
//        public void Test_GetAllExercises_ShouldReturnList()
//        {
//            //Arrange
//            //generate stub
//            //MockRepository mock = new MockRepository();
//            //IExerciseContext context = mock.DynamicMock<IExerciseContext>();
//            ExerciseRepository handler = new ExerciseRepository();
            
//            //Act
//            //mock.ReplayAll();
//            var Exercises = handler.GetAllExercises("admin");

//            //Assert
//            //context.AssertWasCalled(svc => svc.GetAllExercises("admin"));
//        }
//    }
//}
