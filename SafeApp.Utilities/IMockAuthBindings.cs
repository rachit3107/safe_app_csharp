using System;

namespace SafeApp.Utilities {
  public interface IMockAuthBindings {
    void AuthDecodeIpcMsg(
      IntPtr authIntPtr,
      string encodedReq,
      Action<uint, AuthReq> authCb,
      Action<uint, IntPtr> contCb,
      Action<uint, IntPtr, IntPtr> unregCb,
      Action<uint, IntPtr, IntPtr> shareMDataCb,
      Action<FfiResult> errorCb);

    void CreateAcc(
      string accountLocator,
      string accountPassword,
      string invitation,
      Action disconnectedCb,
      Action<FfiResult, IntPtr> callback);

    void EncodeAuthResponse(IntPtr authIntPtr, IntPtr authReq, uint reqId, bool isgranted, Action<FfiResult, string> callback);
    IntPtr TestCreateApp();
  }
}
