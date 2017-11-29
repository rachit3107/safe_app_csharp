#if !NETSTANDARD1_2 || __DESKTOP__

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SafeApp.Utilities;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.MockAuthBindings {
#region Delegates
     
  
  public delegate void AuthDecodeContCb(IntPtr self, uint reqId, IntPtr ffiContainersReq);

  public delegate void AuthDecodeShareMDataCb(IntPtr self, uint reqId, IntPtr ffiShareMDataReq, IntPtr ffiUserMetadata);

  public delegate void AuthDecodeErrorCb(IntPtr self, IntPtr result, string response);

  public delegate void AuthDecodeAuthCb(IntPtr self, uint reqId, IntPtr authFfiresultPtr);

  public delegate void EncodeAuthResponseCb(IntPtr self, IntPtr result, string response);
  #endregion

  #region Types
  public struct AuthDecodeIpcResult
  {
    public (uint, AuthReq) AuthReq;
    public (IntPtr, IntPtr) UnRegAppInfo;
    public (uint, IntPtr) ContReq;
    public uint? ShareMData;
    public bool? Revoked;
  }
  #endregion

  public class MockAuthBindings : IMockAuthBindings {
#region TestCreateApp

    public IntPtr TestCreateApp() {
      var ret = TestCreateAppNative(out var appPtr);
      if (ret != 0) {
        throw new InvalidOperationException();
      }
      return appPtr;
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "test_create_app")]
#else
    [DllImport("safe_app", EntryPoint = "test_create_app")]
#endif
    public static extern int TestCreateAppNative(out IntPtr appPtr);

#endregion
    
    
#region CreateAcc

    public void CreateAcc(
      string accountLocator,
      string accountPassword,
      string invitation,
      Action disconnectedCb,
      Action<FfiResult, IntPtr> intPtrCb) {
      var cb = new List<object> {disconnectedCb, intPtrCb};
      CreateAccNative(accountLocator, accountLocator, invitation, cb.ToHandlePtr(), OnDisconnectedCb, OnIntPtrCb);
    }
#if __IOS__
    [DllImport("__Internal", EntryPoint = "create_acc")]
#else
    [DllImport("safe_app", EntryPoint = "create_acc")]
#endif
    public static extern void CreateAccNative(
      string accountLocator,
      string accountPassword,
      string invitation,
      IntPtr userData,
      DisconnectedCb disconnectedCb,
      IntPtrCb intPtrCb);
#if __IOS__
    [MonoPInvokeCallback(typeof(IntPtrCb))]
#endif
    private static void OnIntPtrCb(IntPtr userData, IntPtr result, IntPtr authPointer) {
      var cb = (Action<FfiResult, IntPtr>)userData.HandlePtrToType<List<object>>()[1];
      cb(Marshal.PtrToStructure<FfiResult>(result), authPointer);
    }
  
#if __IOS__
    [MonoPInvokeCallback(typeof(DisconnectedCb))]
#endif
  private static void OnDisconnectedCb(IntPtr userData)
  {
  var cb = (Action)userData.HandlePtrToType<List<object>>()[0];
  cb();
  }


#endregion
#region EncodeAuthResponse

    public void EncodeAuthResponse(IntPtr authIntPtr, IntPtr authReq, uint reqId, bool isgranted, Action <FfiResult, string> callback)
    {
      EncodeAuthResponseNative(authIntPtr, authReq, reqId, isgranted, callback.ToHandlePtr(), OnEncodeAuthResponseCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "encode_auth_resp")]
#else
    [DllImport("safe_app", EntryPoint = "encode_auth_resp")]
#endif
    public static extern void EncodeAuthResponseNative(IntPtr authIntPtr, IntPtr authReq, uint reqId, [MarshalAs(UnmanagedType.U1)] bool isgranted, IntPtr userData,  EncodeAuthResponseCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(EncodeAuthResponseCb))]
#endif
    private static void OnEncodeAuthResponseCb(IntPtr userData, IntPtr result, string response)
    {
      var cb = userData.HandlePtrToType<Action<FfiResult, string>>();
      cb(Marshal.PtrToStructure<FfiResult>(result), response);
    }

#endregion

#region AuthDecodeIpcMsg

    public void AuthDecodeIpcMsg(
      IntPtr authIntPtr,
      string encodedReq,
      Action<uint, AuthReq> authCb,
      Action<uint, IntPtr> contCb,
      Action<uint, IntPtr, IntPtr> unregCb,
      Action<uint, IntPtr, IntPtr> shareMDataCb,
      Action<FfiResult> errorCb) {
      var cbs = new List<object> {authCb, contCb, unregCb, shareMDataCb, errorCb};
      AuthDecodeIpcMsgNative(
        authIntPtr,
        encodedReq,
        cbs.ToHandlePtr(),
        OnAuthDecodeAuthCb,
        OnAuthDecodeContCb,
        OnDecodeUnregCb,
        OnAuthDecodeShareMDataCb,
        OnAuthDecodeErrorCb);
    }
#if __IOS__
    [DllImport("__Internal", EntryPoint = "auth_decode_ipc_msg")]
#else
    [DllImport("safe_app", EntryPoint = "auth_decode_ipc_msg")]
#endif
    public static extern void AuthDecodeIpcMsgNative(IntPtr authIntPtr, string encodedReq,
      IntPtr self,
      AuthDecodeAuthCb authCb,
      AuthDecodeContCb contCb,
      DecodeUnregCb unregCb,
      AuthDecodeShareMDataCb shareMDataCb,
      AuthDecodeErrorCb errorCb);
#if __IOS__
    [MonoPInvokeCallback(typeof(AuthDecodeAuthCb))]
#endif
    private static void OnAuthDecodeAuthCb(IntPtr self, uint reqId, IntPtr authReqFffiPtr)
    {
      var cb = (Action<uint, AuthReq>)self.HandlePtrToType<List<object>>()[0];
      var authReqFfi = Marshal.PtrToStructure<AuthReqFfi>(authReqFffiPtr);

      var authreq = new AuthReq
      {
        AppExchangeInfo = authReqFfi.AppExchangeInfo,
        AppContainer = authReqFfi.AppContainer,
        Containers = authReqFfi.ContainersArrayPtr.ToList<ContainerPermissions>(authReqFfi.ContainersLen)
      };


      cb(reqId, authreq);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(AuthDecodeContCb))]
#endif
    private static void OnAuthDecodeContCb(IntPtr self, uint reqId, IntPtr ffiContainerreq)
    {
      var cb = (Action<uint, IntPtr>)self.HandlePtrToType<List<object>>()[2];
      cb(reqId, ffiContainerreq);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeUnregCb))]
#endif
    private static void OnDecodeUnregCb(IntPtr self, uint reqId, IntPtr extreDataPtr, IntPtr extreDatalength)
    {
      var cb = (Action<uint, IntPtr, IntPtr>)self.HandlePtrToType<List<object>>()[1];
      cb(reqId, extreDataPtr, extreDatalength);
    }



#if __IOS__
    [MonoPInvokeCallback(typeof(AuthDecodeShareMDataCb))]
#endif
    private static void OnAuthDecodeShareMDataCb(IntPtr self, uint reqId, IntPtr ffiShareMDataReq, IntPtr ffiUserMetadata)
    {
      var cb = (Action<uint, IntPtr, IntPtr>)self.HandlePtrToType<List<object>>()[3];
      cb(reqId, ffiShareMDataReq, ffiUserMetadata);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(AuthDecodeErrorCb))]
#endif
    private static void OnAuthDecodeErrorCb(IntPtr self, IntPtr result, string response)
    {
      var cb = (Action<FfiResult>)self.HandlePtrToType<List<object>>()[5];
      cb(Marshal.PtrToStructure<FfiResult>(result));
    }



#endregion
  }
}

#endif
