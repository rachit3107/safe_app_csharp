using System;
using System.Collections.Generic;
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

    /*public static Task<List<PermissionSet>> ListPermissionsAsync(MDataInfo info, NativeHandle signPubKeyH)
    {
      var tcs = new TaskCompletionSource<List<PermissionSet>>();
      var infoPtr = Helpers.StructToPtr(info);

      Action<FfiResult, IntPtr> callback = (result, permissionsetPtr) => {
        if (result.ErrorCode != 0)
        {
          tcs.SetException(result.ToException());
          return;
        }
        var permissionSet = Marshal.PtrToStructure<PermissionSet>(permissionsetPtr);

        tcs.SetResult((permissionSet.Select(k => k.DataPtr.ToList<byte>(k.Len)).ToList()));
      };

      AppBindings.MDataPermissionsInsert(Session.AppPtr, permissionsH, forUserH, permissionSetPtr, callback);
      Marshal.FreeHGlobal(permissionSetPtr);

      return tcs.Task;
    }*/

   

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

    public static Task<ulong> PermissionLenAsync(NativeHandle mdatPermissionHandle)
    {
      var tcs = new TaskCompletionSource<ulong>();
      Action<FfiResult, ulong> callback = (result, len) => {
        if (result.ErrorCode != 0)
        {
          tcs.SetException(result.ToException());
          return;
        }
        tcs.SetResult(len);
      };

      AppBindings.MDataEntriesLen(Session.AppPtr, mdatPermissionHandle, callback);

      return tcs.Task;
    }
    public static Task<PermissionSet> PermissionGetAsync(NativeHandle mdatPermissionHandle, NativeHandle signPubKeyNativeHandle)
    {
      var tcs = new TaskCompletionSource<PermissionSet>();
      Action<FfiResult, IntPtr> callback = (result, permissionsetPtr) => {
        if (result.ErrorCode != 0)
        {
          tcs.SetException(result.ToException());
          return;
        }
        var permissionSet = Marshal.PtrToStructure<PermissionSet>(permissionsetPtr);
        tcs.SetResult(permissionSet);
      };

      AppBindings.MdataPermissionGet(Session.AppPtr, mdatPermissionHandle, signPubKeyNativeHandle, callback);

      return tcs.Task;
    }
  }
}
