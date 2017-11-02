using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests
{
    class Utils
    {
      public static void InitialiseSessionForRandomTestApp() {
        var appPtr = IntPtr.Zero;
        Assert.DoesNotThrow(() => appPtr = MockAuthResolver.Current.TestCreateApp());
        Assert.AreNotEqual(appPtr, IntPtr.Zero);
        Session.AppPtr = appPtr;
      }
    }
}
