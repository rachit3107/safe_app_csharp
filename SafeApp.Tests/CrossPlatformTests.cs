using System;
using NUnit.Framework;
using SafeApp.MData;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CrossPlatformTests {
    [SetUp]
    public void Setup() { }

    [TearDown]
    public void Tear() { }

    [Test]
    public void RandomAppCreate() {
      var appPtr = IntPtr.Zero;
      Assert.DoesNotThrow(() => appPtr = MockAuthResolver.Current.TestCreateApp());
      Assert.AreNotEqual(appPtr, IntPtr.Zero);
      Session.AppPtr = appPtr;
      Assert.DoesNotThrow(async () => await MDataEntryActions.NewAsync());
    }
  }
}
