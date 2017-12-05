using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  public static class CipherOpt {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong cipherOptHandle) {
      var tcs = new TaskCompletionSource<object>();
      Action<FfiResult> callback = (result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.CipherOptFree(Session.AppPtr, cipherOptHandle, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewAsymmetricAsync(NativeHandle encPubKeyH) {
      var tcs = new TaskCompletionSource<NativeHandle>();
      Action<FfiResult, ulong> callback = (result, cipherOptHandle) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(cipherOptHandle, FreeAsync));
      };

      AppBindings.CipherOptNewAsymmetric(Session.AppPtr, encPubKeyH, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewPlaintextAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();
      Action<FfiResult, ulong> callback = (result, cipherOptHandle) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(cipherOptHandle, FreeAsync));
      };

      AppBindings.CipherOptNewPlaintext(Session.AppPtr, callback);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewSymmetricAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();
      Action<FfiResult, ulong> callback = (result, cipherOptHandle) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(cipherOptHandle, FreeAsync));
      };

      AppBindings.CipherOptNewSymmetric(Session.AppPtr, callback);

      return tcs.Task;
    }
  }
}
