using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataKeys {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<List<byte>>> ForEachAsync(NativeHandle entKeysH) {
      var tcs = new TaskCompletionSource<List<List<byte>>>();
      var keys = new List<List<byte>>();
      Action<MDataKeyFfi> forEachCb = mdataKey => {
        var key = mdataKey.DataPtr.ToList<byte>(mdataKey.Len);
        keys.Add(key);
      };

      Action<FfiResult> forEachResCb = result => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(keys);
      };

      AppBindings.MDataKeysForEach(Session.AppPtr, entKeysH, forEachCb, forEachResCb);

      return tcs.Task;
    }

    public static Task<IntPtr> LenAsync(NativeHandle mDataInfoH) {
      var tcs = new TaskCompletionSource<IntPtr>();
      Action<FfiResult, IntPtr> callback = (result, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(len);
      };

      AppBindings.MDataKeysLen(Session.AppPtr, mDataInfoH, callback);

      return tcs.Task;
    }
  }
}
