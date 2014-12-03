//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Sporty.Controls;
//using SportBusinessTest;
//using Sporty.Business;
//using Rhino.Mocks;

//namespace Sporty.Tests.Controls
//{
//    /// <summary>
//    /// Summary description for LoginUserTest
//    /// </summary>
//    [TestClass]
//    public class LoginUserTest
//    {
//        public LoginUserTest()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

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
//        public void LoginUser_LoginAccepted_ReturnTrue()
//        {
//            //IBusinessService service = new TestBusinessService();
//            //SportyMembershipProvider provider = new SportyMembershipProvider();
//            //bool isAutendicated = provider.ValidateUser("Admin", "admin");
//            //Assert.IsTrue(isAutendicated, "User logs correctly.");

//            MockRepository mocks = new MockRepository();
//            IBusinessService fakeService = mocks.StrictMock<IBusinessService>();
//            Expect.Call(fakeService.ValidateUser("Admin", "admin")).Return(true);
            
//            bool isValidate = fakeService.ValidateUser("Admin", "admin");
//            mocks.ReplayAll();

//            fakeService.AssertWasCalled(svc => svc.ValidateUser("Admin", "admin"));
//        }

//        [TestMethod]
//        public void LoginUser_LoginFails_ReturnFalse()
//        {

//            IBusinessService service = new TestBusinessService();
//            SportyMembershipProvider provider = new SportyMembershipProvider();
//            bool isAuthenticated = provider.ValidateUser("Admin", "not correct");
//            Assert.IsFalse(isAuthenticated, "User login fails.");
//        }
//    }
//}
