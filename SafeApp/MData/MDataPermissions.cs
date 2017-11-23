using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class MDataPermissions {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task FreeAsync(ulong permissionsH) {
      var tcs = new TaskCompletionSource<object>();
      Action<FfiResult> callback = result => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPermissionsFree(Session.AppPtr, permissionsH, callback);

      return tcs.Task;
    }

    public static Task InsertAsync(NativeHandle permissionsH, NativeHandle forUserH, PermissionSet permissionSet) {
      var tcs = new TaskCompletionSource<object>();
      var permissionSetPtr = Helpers.StructToPtr(permissionSet);

      Action<FfiResult> callback = result => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(null);
      };

      AppBindings.MDataPermissionsInsert(Session.AppPtr, permissionsH, forUserH, permissionSetPtr, callback);
      Marshal.FreeHGlobal(permissionSetPtr);

      return tcs.Task;
    }

    public static Task<NativeHandle> NewAsync() {
      var tcs = new TaskCompletionSource<NativeHandle>();

      Action<FfiResult, ulong> callback = (result, permissionsH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(new NativeHandle(permissionsH, FreeAsync));
      };

      AppBindings.MDataPermissionsNew(Session.AppPtr, callback);

      return tcs.Task;
    }
  }
}
