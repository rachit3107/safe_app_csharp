using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MockAuthBindings;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  internal class Utils {
    private static volatile bool _isDisconnected;
    private static readonly IMockAuthBindings MockAuthBindings = MockAuthResolver.Current;

    private static readonly Action OnDisconnectedCb = () => { IsDisconnected = true; };

    public static bool IsDisconnected { get => _isDisconnected; private set => _isDisconnected = value; }

    public static Task<AuthDecodeIpcResult> AuthDecodeIpcMsgAsync(IntPtr authPointer, string encodedReq) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<AuthDecodeIpcResult>();
          Action<uint, AuthReq> authCb = (id, authReq) => { tcs.SetResult(new AuthDecodeIpcResult {AuthReq = (id, authReq)}); };

          Action<uint, IntPtr, IntPtr> unregCb = (id, extreDataPtr, extreDataLength) => { throw new NotImplementedException(); };
          Action<uint, IntPtr> contCb = (id, ffiShareMDataReq) => { throw new NotImplementedException();  };
          Action<uint, IntPtr, IntPtr> shareMDataCb = (id, ffiShareMDataReq, ffiUserMetadata) => { throw new NotImplementedException();  };
          Action<FfiResult> errorCb = result => { throw new NotImplementedException(); };

          MockAuthBindings.AuthDecodeIpcMsg(authPointer, encodedReq, authCb, contCb, unregCb, shareMDataCb, errorCb);

          return tcs.Task;
        });
    }

    public static Task<IntPtr> CrateAccAsync(string accountLocator, string accountPassword, string invitation) {
      var tcs = new TaskCompletionSource<IntPtr>();
      Action<FfiResult, IntPtr> callback = (result, authPointer) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }
        tcs.SetResult(authPointer);
      };

      MockAuthBindings.CreateAcc(accountLocator, accountPassword, invitation, OnDisconnectedCb, callback);

      return tcs.Task;
    }

    public static Task<string> EncodeAuthResAsync(IntPtr authIntPtr, AuthReq authReq, uint reqId, bool isgranted) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<string>();
          var authReqFfi = new AuthReqFfi {
            AppContainer = authReq.AppContainer,
            AppExchangeInfo = authReq.AppExchangeInfo,
            ContainersLen = (IntPtr)authReq.Containers.Count,
            ContainersArrayPtr = authReq.Containers.ToIntPtr()
          };
          var authReqFfiPtr = Helpers.StructToPtr(authReqFfi);

          Action<FfiResult, string> callback = (result, response) => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            tcs.SetResult(response);
          };

          MockAuthBindings.EncodeAuthResponse(authIntPtr, authReqFfiPtr, reqId, isgranted, callback);
          Marshal.FreeHGlobal(authReqFfi.ContainersArrayPtr);
          Marshal.FreeHGlobal(authReqFfiPtr);

          return tcs.Task;
        });
    }

    public static void InitialiseSessionForRandomTestApp() {
      var appPtr = IntPtr.Zero;
      Assert.DoesNotThrow(() => appPtr = MockAuthResolver.Current.TestCreateApp());
      Assert.AreNotEqual(appPtr, IntPtr.Zero);
      Session.AppPtr = appPtr;
    }

    public static string RandomString(int length) {
      var random = new Random();
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
  }
}
