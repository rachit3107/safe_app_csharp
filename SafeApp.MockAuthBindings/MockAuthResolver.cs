using System;
using System.Threading;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  public class MockAuthResolver {
    private static readonly Lazy<IMockAuthBindings> Implementation =
      new Lazy<IMockAuthBindings>(CreateBindings, LazyThreadSafetyMode.PublicationOnly);

    public static IMockAuthBindings Current {
      get {
        var ret = Implementation.Value;
        if (ret == null) {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    private static IMockAuthBindings CreateBindings() {
#if NETSTANDARD1_2 && !__DESKTOP__
      return null;
#else
      return new MockAuthBindings();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly() {
      return new NotImplementedException(
        "Please ensure you have SAFE_APP_MOCK defined in the application project as well. " +
        "You should also have a reference to the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
