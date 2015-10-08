using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DummyTests.Silverlight5
{
    using System.Windows;

    [TestClass]
    public class TestFixture1
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize1(TestContext context)
        {
        }

        [ClassInitialize]
        public static void ClassInitialize1(TestContext context)
        {
        }

        [TestInitialize]
        public void TestInitialize1()
        {
        }

        [TestMethod]
        public void PassingTest1()
        {
            var runtimeVersion = Deployment.Current.RuntimeVersion;
            Assert.IsTrue(runtimeVersion.StartsWith("5."), string.Format("Runtime version was {0}, expected 5.*", runtimeVersion));
        }

        [TestMethod]
        public void FailingTest1()
        {
            Assert.Fail();
        }

        [Ignore]
        [TestMethod]
        public void IgnoredTest1()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PassingTest2()
        {
            throw new InvalidOperationException();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void FailingTest2()
        {
            throw new InvalidOperationException();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FailingTest3()
        {
        }

        [TestMethod]
        public void FailingTest4()
        {
            throw new InvalidOperationException();
        }

        [TestCleanup]
        public void TestCleanup1()
        {
        }

        [ClassCleanup]
        public static void ClassCleanup1()
        {
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup1()
        {
        }
    }
}