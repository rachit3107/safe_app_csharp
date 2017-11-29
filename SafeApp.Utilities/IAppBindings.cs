using System;
using System.Collections.Generic;

namespace SafeApp.Utilities {
  public interface IAppBindings {
    void AccessContainerGetContainerMDataInfo(IntPtr appPtr, string name, Action<FfiResult, MDataInfo> callback);
    void AppExeFileStem(Action<FfiResult, string> callback);
    void AppInitLogging(string fileName, Action<FfiResult> callback);
    void AppOutputLogPath(string fileName, Action<FfiResult, string> callback);
    void AppPubSignKey(IntPtr appPtr, Action<FfiResult, ulong> callback);

    void AppRegistered(string appId, IntPtr ffiAuthGrantedPtr, Action onDisconnectedCb, Action<FfiResult, IntPtr> appRegCb);

    void AppSetAdditionalSearchPath(string path, Action<FfiResult> callback);
    void CipherOptFree(IntPtr appPtr, ulong cipherOptHandle, Action<FfiResult> callback);
    void CipherOptNewAsymmetric(IntPtr appPtr, ulong encryptPubKeyHandle, Action<FfiResult, ulong> callback);
    void CipherOptNewPlaintext(IntPtr appPtr, Action<FfiResult, ulong> callback);
    void CipherOptNewSymmetric(IntPtr appPtr, Action<FfiResult, ulong> callback);

    void DecodeIpcMessage(
      string encodedReq,
      Action<uint, IntPtr> authCb,
      Action<uint, IntPtr, IntPtr> unregCb,
      Action<uint> contCb,
      Action<uint> shareMDataCb,
      Action revokedCb,
      Action<FfiResult> errorCb);

    void Decrypt(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong encryptPubKeyHandle,
      ulong encryptSecKeyHandle,
      Action<FfiResult, IntPtr, IntPtr> callback);

    void DecryptSealedBox(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong pkHandle,
      ulong skHandle,
      Action<FfiResult, IntPtr, IntPtr> callback);

    void EncGenerateKeyPair(IntPtr appPtr, Action<FfiResult, ulong, ulong> callback);
    void EncodeAuthReq(IntPtr authReq, Action<FfiResult, uint, string> callback);
    void EncPubKeyFree(IntPtr appPtr, ulong encryptPubKeyHandle, Action<FfiResult> callback);
    void EncPubKeyGet(IntPtr appPtr, ulong encryptPubKeyHandle, Action<FfiResult, IntPtr> callback);
    void EncPubKeyNew(IntPtr appPtr, IntPtr asymPublicKey, Action<FfiResult, ulong> callback);

    void Encrypt(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong encryptPubKeyHandle,
      ulong encryptSecKeyHandle,
      Action<FfiResult, IntPtr, IntPtr> callback);

    void EncryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, Action<FfiResult, IntPtr, IntPtr> callback);
    void EncSecretKeyFree(IntPtr appPtr, ulong encryptSecKeyHandle, Action<FfiResult> callback);
    void EncSecretKeyGet(IntPtr appPtr, ulong encryptSecKeyHandle, Action<FfiResult, IntPtr> callback);
    void EncSecretKeyNew(IntPtr appPtr, IntPtr asymSecretKey, Action<FfiResult, ulong> callback);
    void FreeApp(IntPtr appPtr);

    void GenerateNonce(Action<FfiResult, IntPtr> callback);

    void MDataEntriesForEach(
      IntPtr appPtr,
      ulong entriesHandle,
      Action<IntPtr, IntPtr, IntPtr, IntPtr, ulong> forEachCallback,
      Action<FfiResult> resultCallback);

    void MDataEntriesFree(IntPtr appPtr, ulong entriesHandle, Action<FfiResult> callback);

    void MDataEntriesInsert(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      Action<FfiResult> callback);

    void MDataEntriesLen(IntPtr appPtr, ulong entriesHandle, Action<FfiResult, ulong> callback);
    void MDataEntriesNew(IntPtr appPtr, Action<FfiResult, ulong> callback);
    void MDataEntryActionsFree(IntPtr appPtr, ulong actionsHandle, Action<FfiResult> callback);

    void MDataEntryActionsInsert(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      Action<FfiResult> callback);

    void MDataEntryActionsNew(IntPtr appPtr, Action<FfiResult, ulong> callback);
    void MDataGetValue(IntPtr appPtr, IntPtr info, IntPtr keyPtr, IntPtr keyLen, Action<FfiResult, IntPtr, IntPtr, ulong> callback);
    void MDataInfoDecrypt(IntPtr mDataInfoPtr, IntPtr cipherText, IntPtr cipherLen, Action<FfiResult, IntPtr, IntPtr> callback);
    void MDataInfoDeserialise(IntPtr data, IntPtr len, Action<FfiResult, IntPtr> callback);

    void MDataInfoEncryptEntryKey(IntPtr info, IntPtr inputPtr, IntPtr inputLen, Action<FfiResult, IntPtr, IntPtr> callback);

    void MDataInfoEncryptEntryValue(IntPtr info, IntPtr inputPtr, IntPtr inputLen, Action<FfiResult, IntPtr, IntPtr> callback);

    void MDataInfoNewPublic(IntPtr xorNameArr, ulong typeTag, Action<FfiResult, IntPtr> callback);
    void MDataInfoRandomPrivate(ulong typeTag, Action<FfiResult, IntPtr> callback);
    void MDataInfoRandomPublic(ulong typeTag, Action<FfiResult, IntPtr> callback);
    void MDataInfoSerialise(IntPtr infoHandle, Action<FfiResult, IntPtr, IntPtr> callback);
    void MDataKeysForEach(IntPtr appPtr, ulong keysHandle, Action<MDataKeyFfi> forEachCb, Action<FfiResult> resCb);

    void MDataKeysFree(IntPtr appPtr, ulong keysHandle, Action<FfiResult> callback);

    void MDataKeysLen(IntPtr appPtr, ulong keysHandle, Action<FfiResult, IntPtr> callback);
    void MDataListEntries(IntPtr appPtr, IntPtr info, Action<FfiResult, ulong> callback);
    void MDataListKeys(IntPtr appPtr, IntPtr info, Action<FfiResult, List<MDataKeyFfi>> callback);
    void MDataMutateEntries(IntPtr appPtr, IntPtr info, ulong actionsHandle, Action<FfiResult> callback);
    void MDataPermissionSetAllow(IntPtr appPtr, ulong setHandle, MDataAction action, ResultCb callback);
    void MDataPermissionSetFree(IntPtr appPtr, ulong setHandle, ResultCb callback);
    void MDataPermissionSetNew(IntPtr appPtr, UlongCb callback);
    void MDataPermissionsFree(IntPtr appPtr, ulong permissionsHandle, Action<FfiResult> callback);

    void MDataPermissionsInsert(
      IntPtr appPtr,
      ulong permissionsHandle,
      ulong userHandle,
      IntPtr permissionSetHandle,
      Action<FfiResult> callback);

    void MDataPermissionsNew(IntPtr appPtr, Action<FfiResult, ulong> callback);
    void MDataPut(IntPtr appPtr, IntPtr infoHandle, ulong permissionsHandle, ulong entriesHandle, Action<FfiResult> callback);
    void Sha3Hash(IntPtr data, IntPtr len, Action<FfiResult, IntPtr, IntPtr> callback);

    void Sign(IntPtr appPtr, IntPtr data, IntPtr len, ulong signSeckeyHandle, Action<FfiResult, IntPtr, IntPtr> callback);

    void SignGenerateKeyPair(IntPtr appPtr, Action<FfiResult, ulong, ulong> callback);

    void SignPubKeyFree(IntPtr appPtr, ulong signPubKeyHandle, Action<FfiResult> callback);

    void SignPubKeyGet(IntPtr appPtr, ulong signPubKeyHandle, Action<FfiResult, IntPtr> callback);

    void SignPubKeyNew(IntPtr appPtr, IntPtr signPubKey, Action<FfiResult, ulong> callback);

    void SignSecKeyFree(IntPtr appPtr, ulong signSecKeyHandle, Action<FfiResult> callback);

    void SignSecKeyGet(IntPtr appPtr, ulong signSecKeyHandle, Action<FfiResult, IntPtr> callback);

    void SignSecKeyNew(IntPtr appPtr, IntPtr signSecKey, Action<FfiResult, ulong> callback);

    void Verify(IntPtr appPtr, IntPtr signdata, IntPtr len, ulong signPubkeyHandle, Action<FfiResult, IntPtr, IntPtr> callback);

    // ReSharper disable InconsistentNaming
    void IDataCloseSelfEncryptor(IntPtr appPtr, ulong seH, ulong cipherOptH, Action<FfiResult, IntPtr> callback);

    void IDataFetchSelfEncryptor(IntPtr appPtr, IntPtr xorNameArr, Action<FfiResult, ulong> callback);
    void IDataNewSelfEncryptor(IntPtr appPtr, Action<FfiResult, ulong> callback);

    void IDataReadFromSelfEncryptor(IntPtr appPtr, ulong seHandle, ulong fromPos, ulong len, Action<FfiResult, IntPtr, IntPtr> callback);

    void IDataSelfEncryptorReaderFree(IntPtr appPtr, ulong sEReaderHandle, Action<FfiResult> callback);
    void IDataSelfEncryptorWriterFree(IntPtr appPtr, ulong sEWriterHandle, Action<FfiResult> callback);
    void IDataSize(IntPtr appPtr, ulong seHandle, Action<FfiResult, ulong> callback);

    void IDataWriteToSelfEncryptor(IntPtr appPtr, ulong seHandle, IntPtr data, IntPtr size, Action<FfiResult> callback);
    // ReSharper restore InconsistentNaming
  }
}
