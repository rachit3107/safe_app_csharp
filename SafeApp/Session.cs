using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  public static class Session {
    public static event EventHandler Disconnected;
    private static IntPtr _appPtr;
    private static volatile bool _isDisconnected;
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    private static readonly Action OnDisconnectedCb;

    private static readonly Action OnDisconnectedObserverCb = () => {
      Debug.WriteLine("Network Disconnected Fired");

      IsDisconnected = true;
      OnDisconnected(EventArgs.Empty);
    };

    public static bool IsDisconnected { get => _isDisconnected; private set => _isDisconnected = value; }

    public static IntPtr AppPtr {
      set {
        if (_appPtr == value) {
          return;
        }

        if (_appPtr != IntPtr.Zero) {
          AppBindings.FreeApp(_appPtr);
        }

        _appPtr = value;
      }
      get {
        if (_appPtr == IntPtr.Zero) {
          throw new ArgumentNullException(nameof(AppPtr));
        }
        return _appPtr;
      }
    }

    static Session() {
      AppPtr = IntPtr.Zero;
      OnDisconnectedCb = OnDisconnectedObserverCb;
    }

    public static Task<bool> AppRegisteredAsync(string appId, AuthGranted authGranted) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<bool>();
          var authGrantedFfi = new AuthGrantedFfi {
            AccessContainer = authGranted.AccessContainer,
            AppKeys = authGranted.AppKeys,
            BootStrapConfigPtr = authGranted.BootStrapConfig.ToIntPtr(),
            BootStrapConfigLen = (IntPtr)authGranted.BootStrapConfig.Count
          };
          var authGrantedFfiPtr = Helpers.StructToPtr(authGrantedFfi);

          Action<FfiResult, IntPtr> callback = (result, appPtr) => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            AppPtr = appPtr;
            IsDisconnected = false;
            tcs.SetResult(true);
          };

          AppBindings.AppRegistered(appId, authGrantedFfiPtr, OnDisconnectedCb, callback);
          Marshal.FreeHGlobal(authGrantedFfi.BootStrapConfigPtr);
          Marshal.FreeHGlobal(authGrantedFfiPtr);

          return tcs.Task;
        });
    }

    public static Task<DecodeIpcResult> DecodeIpcMessageAsync(string encodedReq) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<DecodeIpcResult>();

          Action<uint, IntPtr> authCb = (id, authGrantedFfiPtr) => {
            var authGrantedFfi = Marshal.PtrToStructure<AuthGrantedFfi>(authGrantedFfiPtr);
            var authGranted = new AuthGranted {
              AppKeys = authGrantedFfi.AppKeys,
              AccessContainer = authGrantedFfi.AccessContainer,
              BootStrapConfig = authGrantedFfi.BootStrapConfigPtr.ToList<byte>(authGrantedFfi.BootStrapConfigLen)
            };

            tcs.SetResult(new DecodeIpcResult {AuthGranted = authGranted});
          };
          Action<uint, IntPtr, IntPtr> unregCb = (id, config, size) => {
            tcs.SetResult(new DecodeIpcResult {UnRegAppInfo = (config, size)});
          };
          Action<uint> contCb = id => { tcs.SetResult(new DecodeIpcResult {ContReqId = id}); };
          Action<uint> shareMDataCb = id => { tcs.SetResult(new DecodeIpcResult {ShareMData = id}); };
          Action revokedCb = () => { tcs.SetResult(new DecodeIpcResult {Revoked = true}); };
          Action<FfiResult> errorCb = result => { tcs.SetException(result.ToException()); };

          AppBindings.DecodeIpcMessage(encodedReq, authCb, unregCb, contCb, shareMDataCb, revokedCb, errorCb);

          return tcs.Task;
        });
    }

    public static Task<string> EncodeAuthReqAsync(AuthReq authReq) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<string>();
          if (authReq.Containers == null) {
            tcs.SetException(new ArgumentNullException($"{nameof(authReq.Containers)} cannot be null"));
            return tcs.Task;
          }
          if (string.IsNullOrEmpty(authReq.AppExchangeInfo.Name) || string.IsNullOrEmpty(authReq.AppExchangeInfo.Id) ||
              string.IsNullOrEmpty(authReq.AppExchangeInfo.Vendor)) {
            tcs.SetException(
              new ArgumentException(
                $"{nameof(authReq.AppExchangeInfo.Name)}, {nameof(authReq.AppExchangeInfo.Id)}, {nameof(authReq.AppExchangeInfo.Vendor)} fields are mandatory for AppExchageInfo"));
            return tcs.Task;
          }
          var authReqFfi = new AuthReqFfi {
            AppContainer = authReq.AppContainer,
            AppExchangeInfo = authReq.AppExchangeInfo,
            ContainersLen = (IntPtr)authReq.Containers.Count,
            ContainersArrayPtr = authReq.Containers.ToIntPtr()
          };
          var authReqFfiPtr = Helpers.StructToPtr(authReqFfi);
          Action<FfiResult, uint, string> callback = (result, id, req) => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            tcs.SetResult(req);
          };

          AppBindings.EncodeAuthReq(authReqFfiPtr, callback);
          Marshal.FreeHGlobal(authReqFfi.ContainersArrayPtr);
          Marshal.FreeHGlobal(authReqFfiPtr);

          return tcs.Task;
        });
    }

    public static void FreeApp() {
      IsDisconnected = false;
      AppPtr = IntPtr.Zero;
    }

    public static Task<bool> InitLoggingAsync(string configFilesPath) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<bool>();

          Action<FfiResult> cb2 = result => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            tcs.SetResult(true);
          };

          Action<FfiResult> cb1 = result => {
            if (result.ErrorCode != 0) {
              tcs.SetException(result.ToException());
              return;
            }

            AppBindings.AppInitLogging(null, cb2);
          };

          AppBindings.AppSetAdditionalSearchPath(configFilesPath, cb1);
          return tcs.Task;
        });
    }

    private static void OnDisconnected(EventArgs e) {
      Disconnected?.Invoke(null, e);
    }
  }
}
