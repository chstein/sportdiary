using Sporty.Business.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sporty.DataModel;
using Rhino.Mocks;
using System.Data.Linq;
using System.Collections.Generic;

namespace Sport.Business.Test
{


    /// <summary>
    ///This is a test class for ApplicationRepositoryTest and is intended
    ///to contain all ApplicationRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ApplicationRepositoryTest
    {
        private SportyEntities context;

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
        [TestInitialize()]
        public void Initialize()
        {
            context = MockRepository.GenerateStub<SportyEntities>();
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ApplicationRepository Constructor
        ///</summary>
        [TestMethod()]
        public void ApplicationRepositoryConstructorTest()
        {
            //SportyEntities context = null; // TODO: Initialize to an appropriate value
            //ApplicationRepository target = new ApplicationRepository(context);
            //Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetApplicationIdOrCreate
        ///</summary>
        [TestMethod()]
        public void GetApplicationIdOrCreateTest()
        {
            //IEnumerable<Application> table = new Table<Application>(LinqToSqlObjects);
            //var id = Guid.NewGuid();
            //string applicationName = "/";
            //var al = new TableWrapper<Application>(table) ;//{ new Application { ApplicationId = id, ApplicationName = applicationName } };
            //context.Stub(stub => stub.Applications).Return(al);
            
            //ApplicationRepository target = new ApplicationRepository(context); // TODO: Initialize to an appropriate value
            //Guid actual = target.GetApplicationIdOrCreate(applicationName);
            //Assert.AreEqual(id, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
