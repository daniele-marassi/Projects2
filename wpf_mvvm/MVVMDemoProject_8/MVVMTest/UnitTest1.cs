using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVMDemo.ViewModel;

namespace MVVMTest
{
    [TestClass]

    public class UnitTest1
    {
        [TestMethod]

        public void TestMethod1()
        {
            StudentViewModel sViewModel = new StudentViewModel();
            int count = sViewModel.GetStudentCount();
            Assert.IsTrue(count == 3);
        }
    }
}