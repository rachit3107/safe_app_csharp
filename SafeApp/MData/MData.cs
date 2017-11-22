using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MData {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    
    public static Task<(List<byte>, ulong)> GetValueAsync(MDataInfo info, List<byte> key) {
      var tcs = new TaskCompletionSource<(List<byte>, ulong)>();
      var infoPtr = info.ToHandlePtr();
      var keyPtr = key.ToIntPtr();
      Action<FfiResult, IntPtr, IntPtr, ulong> callback = (result, dataPtr, dataLen, entryVersion) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        var data = dataPtr.ToList<byte>(dataLen);
        tcs.SetResult((data, entryVersion));
      };

      AppBindings.MDataGetValue(Session.AppPtr, infoPtr, keyPtr, (IntPtr)key.Count, callback);
      Marshal.FreeHGlobal(keyPtr);
      Marshal.FreeHGlobal(infoPtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> ListEntriesAsync(MDataInfo info) {
      var tcs = new TaskCompletionSource<NativeHandle>();
      var infoPtr = info.ToHandlePtr();
      Action<FfiResult, ulong> callback = (result, mDataEntriesHandle) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(mDataEntriesHandle, MDataEntries.FreeAsync));
      };

      AppBindings.MDataListEntries(Session.AppPtr, infoPtr, callback);
      Marshal.FreeHGlobal(infoPtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> ListKeysAsync(MDataInfo mDataInfo) {
      var tcs = new TaskCompletionSource<NativeHandle>();
      var infoPtr = mDataInfo.ToHandlePtr();
      Action<FfiResult, ulong> callback = (result, mDataEntKeysH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(mDataEntKeysH, MDataKeys.FreeAsync));
      };

      AppBindings.MDataListKeys(Session.AppPtr, infoPtr, callback);
      Marshal.FreeHGlobal(infoPtr);
      return tcs.Task;
    }

    public static Task MutateEntriesAsync(MDataInfo mDataInfo, NativeHandle entryActionsH) {
      var tcs = new TaskCompletionSource<object>();
      Action<FfiResult> callback = (result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };
      var infoPtr = mDataInfo.ToHandlePtr();
      AppBindings.MDataMutateEntries(Session.AppPtr, infoPtr, entryActionsH, callback);
      Marshal.FreeHGlobal(infoPtr);
      return tcs.Task;
    }

    public static Task PutAsync(MDataInfo mDataInfo, NativeHandle permissionsH, NativeHandle entriesH) {
      var tcs = new TaskCompletionSource<object>();
      Action<FfiResult> callback = (result) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };
      var infoPtr = mDataInfo.ToHandlePtr();
      AppBindings.MDataPut(Session.AppPtr, infoPtr, permissionsH, entriesH, callback);

      return tcs.Task;
    }
  }
}
