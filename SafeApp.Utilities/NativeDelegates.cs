using System;

namespace SafeApp.Utilities {
  public delegate void ResultCb(IntPtr self, IntPtr result);

  public delegate void DisconnectedCb(IntPtr self);

  public delegate void AppRegCb(IntPtr self, IntPtr result, IntPtr value);

  public delegate void ListBasedResultCb(IntPtr self, FfiResult result);

  public delegate void StringCb(IntPtr self, FfiResult result, string exeFileStem);

  public delegate void ByteArrayCb(IntPtr self, FfiResult result, IntPtr arrayIntPtr, IntPtr len);

  public delegate void IntCb(IntPtr self, FfiResult result, int value);

  public delegate void IntPtrCb(IntPtr self, FfiResult result, IntPtr value);

  public delegate void UlongCb(IntPtr self, FfiResult result, ulong handle);

  public delegate void EncodeAuthReqCb(IntPtr self, IntPtr result, uint requestId, string encodedReq);

  public delegate void EncGenerateKeyPairCb(IntPtr self, FfiResult result, ulong encPubKeyHandle, ulong encSecKeyHandle);
  public delegate void MDataEntriesForEachCb(
    IntPtr self,
    IntPtr entryKey,
    IntPtr entryKeyLen,
    IntPtr entryVal,
    IntPtr entryValLen,
    ulong entryVersion);

  public delegate void MDataInfoCb(IntPtr self, IntPtr result, IntPtr mdataInfo);

  public delegate void MDataKeysForEachCb(IntPtr self, IntPtr bytePtr, IntPtr byteLen);

  public delegate void MDataEntriesLenCb(IntPtr self, ulong len);

  public delegate void MDataGetValueCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen, ulong entryVersion);

  public delegate void DecodeAuthCb(IntPtr self, uint reqId, IntPtr authGrantedFfiPtr);

  public delegate void DecodeUnregCb(IntPtr self, uint reqId, IntPtr bsConfig, IntPtr bsSize);

  public delegate void DecodeContCb(IntPtr self, uint reqId);

  public delegate void DecodeShareMDataCb(IntPtr self, uint reqId);

  public delegate void DecodeRevokedCb(IntPtr self);

  public delegate void DecodeErrorCb(IntPtr self, IntPtr result, uint reqId);
}
