#if !NETSTANDARD1_2

using System;
using System.Runtime.InteropServices;
using SafeApp.Utilities;

#if __IOS__

#endif

namespace SafeApp.MockAuthBindings {
  public class MockAuthBindings : IMockAuthBindings {
    #region TestCreateApp

    public IntPtr TestCreateApp() {
      var ret = TestCreateAppNative(out var appPtr);
      if (ret != 0) {
        throw new InvalidOperationException();
      }
      return appPtr;
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "test_create_app")]
#elif __ANDROID__
    [DllImport("safe_app", EntryPoint = "test_create_app")]
#endif
    public static extern int TestCreateAppNative(out IntPtr appPtr);

    #endregion
  }
}

#endif
