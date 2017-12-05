using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataEntries {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<(List<byte>, List<byte>, ulong)>> ForEachAsync(NativeHandle entH) {
      var tcs = new TaskCompletionSource<List<(List<byte>, List<byte>, ulong)>>();
      var entries = new List<(List<byte>, List<byte>, ulong)>();

      Action<IntPtr, IntPtr, IntPtr, IntPtr, ulong> forEachCb = (entryKeyPtr, entryKeyLen, entryValPtr, entryValLen, entryVersion) => {
        var entryKey = entryKeyPtr.ToList<byte>(entryKeyLen);
        var entryVal = entryValPtr.ToList<byte>(entryValLen);
        entries.Add((entryKey, entryVal, entryVersion));
      };

      Action<FfiResult> forEachResCb = result => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(entries);
      };

      AppBindings.MDataEntriesForEach(Session.AppPtr, entH, forEachCb, forEachResCb);

      return tcs.Task;
    }

    public static Task FreeAsync(ulong entriesH) {
      var tcs = new TaskCompletionSource<object>();
      Action<FfiResult> callback = result => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataEntriesFree(Session.AppPtr, entriesH, callback);

      return tcs.Task;
    }
    public static Task<(IntPtr, ulong)> MDataEntryGetAsync(NativeHandle entriesH, List<byte> key)
    {
      var tcs = new TaskCompletionSource<(IntPtr, ulong)>();

      var keyPtr = key.ToIntPtr();
      Action<FfiResult, IntPtr, IntPtr, ulong> callback = (result, contentPtr, contentlen, entryVersion) => {
        if (result.ErrorCode != 0)
        {
          tcs.SetException(result.ToException());
          return;
        }

        
        tcs.SetResult((contentPtr, entryVersion));

      };
      
      
      AppBindings.MDataEntryGet(Session.AppPtr, entriesH, keyPtr, (IntPtr)key.Count, callback);
      Marshal.FreeHGlobal(keyPtr);

      return tcs.Task;
    }
    public static Task<ulong> LenAsync(NativeHandle entriesHandle) {
      var tcs = new TaskCompletionSource<ulong>();
      Action<FfiResult, ulong> callback = (result, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }
        tcs.SetResult(len);
      };

      AppBindings.MDataEntriesLen(Session.AppPtr, entriesHandle, callback);

      return tcs.Task;
    }

    

  }
}
