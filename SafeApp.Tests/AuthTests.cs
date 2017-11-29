using System;
using System.Collections.Generic;
using NUnit.Framework;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class AuthTest {
    private const string AuthUri =
        "safe-bmV0Lm1haWRzYWZlLnRlc3Qud2ViYXBwLmlk:AQAAAGSv7oQAAAAAAAAAACAAAAAAAAAAGQ1zYg9iFKof2TVkAPp0R2kjU9DDWmmR_uAXBYvaeIAgAAAAAAAAAKecZc5pOSeoU53v43RdoTscGQbuAO0hF6HA_4ou9GJnIAAAAAAAAADsycX-1RCaNJxnYf6ka1pLncSez4w4PmPIS5lau_IblkAAAAAAAAAAbZdkFJ6Ydhh_OwA7mfYcnta_95k2xRazJsDSeMFGj3vsycX-1RCaNJxnYf6ka1pLncSez4w4PmPIS5lau_IbliAAAAAAAAAAx559E774w-6AWnIXBSm0NWOBW2zr8TOPThmdIeEsoFEgAAAAAAAAAHRNdser-WDOLIBGsDfRbNI304vnYILXI1JZC96tiFvzAAAAAAAAAAAAAAAAAAAAAG7Di2O1ssjN0izb88iclOKj7WD5LtaVriMIrLBbVRHimDoAAAAAAAAYAAAAAAAAAH2p2f2I4yuQPLkSJE_u9-PtM1WD7E65ZA=="
      ;

    private AppExchangeInfo GetExchangeInfo() {
      return new AppExchangeInfo {Id = "net.maidsafe.example", Name = "Test App", Vendor = "Maidsafe Ltd."};
    }

//    [Test]
//    public async Task ConnectAsRegisteredApp() {
//      var auth = new AuthReq { AppContainer = true, AppExchangeInfo = GetExchangeInfo(), Containers = new List<ContainerPermissions>() };
//      var authPointer = await Utils.CrateAccAsync(Utils.RandomString(10), Utils.RandomString(10), "test-invite");
//      var encodedAuthReq = await Session.EncodeAuthReqAsync(auth);
//      Assert.NotNull(encodedAuthReq);
//      var decodedAuthIpcResult = await Utils.AuthDecodeIpcMsgAsync(authPointer, encodedAuthReq);
//      Assert.NotNull(decodedAuthIpcResult.AuthReq);
//      var reqId = decodedAuthIpcResult.AuthReq.Item1;
//      var authReq = decodedAuthIpcResult.AuthReq.Item2;
//      Assert.AreEqual(GetExchangeInfo(), authReq.AppExchangeInfo);
//      Assert.AreEqual(auth.Containers.Count, authReq.Containers.Count);
//      Assert.AreEqual(auth.AppContainer, authReq.AppContainer);
//      var uri = await Utils.EncodeAuthResAsync(authPointer, authReq, reqId, true);
//      Assert.NotNull(uri);
//      Console.WriteLine(uri);
//      var result = await Session.DecodeIpcMessageAsync(uri);
//      Assert.NotNull(result.AuthGranted);
//      var isConnected = result.AuthGranted != null && await Session.AppRegisteredAsync(auth.AppExchangeInfo.Id, result.AuthGranted.Value);
//      Assert.IsTrue(isConnected);
//    }

    [Test]
    public void EncodeAuthRequestWithContainersAsNull() {
      var authReq = new AuthReq {AppContainer = false, AppExchangeInfo = GetExchangeInfo(), Containers = null};
      Assert.ThrowsAsync<ArgumentNullException>(async () => await Session.EncodeAuthReqAsync(authReq));
    }

    [Test]
    public void EncodeAuthRequestWithEmptyContainers() {
      var authReq = new AuthReq {AppContainer = false, AppExchangeInfo = GetExchangeInfo(), Containers = new List<ContainerPermissions>()};
      Assert.DoesNotThrowAsync(async () => await Session.EncodeAuthReqAsync(authReq));
    }

    [Test]
    public void EncodeAuthRequestWithEmptyContainersAndAppContainer() {
      var authReq = new AuthReq {AppContainer = true, AppExchangeInfo = GetExchangeInfo(), Containers = new List<ContainerPermissions>()};
      Assert.DoesNotThrowAsync(async () => await Session.EncodeAuthReqAsync(authReq));
    }

    [Test]
    public void EncodeAuthRequestWithInvalidExchangeInfoThrowsException() {
      var authReq = new AuthReq {
        AppContainer = false,
        AppExchangeInfo = new AppExchangeInfo(),
        Containers = new List<ContainerPermissions>()
      };
      Assert.CatchAsync(async () => await Session.EncodeAuthReqAsync(authReq));
    }
  }
}
