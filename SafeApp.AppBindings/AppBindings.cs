#if !NETSTANDARD1_2 || __DESKTOP__

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SafeApp.Utilities;

#if __IOS__
using ObjCRuntime;

#endif

namespace SafeApp.AppBindings {
  public class AppBindings : IAppBindings {
    #region Generic FFiResult with value Callbacks

#if __IOS__
    [MonoPInvokeCallback(typeof(UlongCb))]
#endif
    private static void OnUlongCb(IntPtr self, IntPtr result, ulong value) {
      self.HandlePtrToType<Action<FfiResult, ulong>>()(Marshal.PtrToStructure<FfiResult>(result), value);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(StringCb))]
#endif
    private static void OnStringCb(IntPtr self, IntPtr result, string value) {
      self.HandlePtrToType<Action<FfiResult, string>>()(Marshal.PtrToStructure<FfiResult>(result), value);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(ResultCb))]
#endif
    private static void OnResultCb(IntPtr self, IntPtr result) {
      self.HandlePtrToType<Action<FfiResult>>()(Marshal.PtrToStructure<FfiResult>(result));
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(IntPtrCb))]
#endif
    private static void OnIntPtrCb(IntPtr self, IntPtr result, IntPtr intPtr) {
      self.HandlePtrToType<Action<FfiResult, IntPtr>>()(Marshal.PtrToStructure<FfiResult>(result), intPtr);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(ByteArrayCb))]
#endif
    private static void OnByteArrayCb(IntPtr self, IntPtr result, IntPtr data, IntPtr dataLen) {
      var cb = self.HandlePtrToType<Action<FfiResult, IntPtr, IntPtr>>();
      cb(Marshal.PtrToStructure<FfiResult>(result), data, dataLen);
    }

    #endregion

    #region AccessContainerGetContainerMDataInfo

    public void AccessContainerGetContainerMDataInfo(IntPtr appPtr, string name, Action<FfiResult, MDataInfo> callback) {
      AccessContainerGetContainerMDataInfoNative(appPtr, name, callback.ToHandlePtr(), OnMDataInfoCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "access_container_get_container_mdata_info")]
#else
    [DllImport("safe_app", EntryPoint = "access_container_get_container_mdata_info")]
#endif
    public static extern void AccessContainerGetContainerMDataInfoNative(IntPtr appPtr, string name, IntPtr self, MDataInfoCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataInfoCb))]
#endif
    private static void OnMDataInfoCb(IntPtr self, IntPtr result, IntPtr mdataInfo) {
      self.HandlePtrToType<Action<FfiResult, MDataInfo>>()(
        Marshal.PtrToStructure<FfiResult>(result),
        Marshal.PtrToStructure<MDataInfo>(mdataInfo));
    }

    #endregion

    #region AppExeFileStem

    public void AppExeFileStem(Action<FfiResult, string> callback) {
      AppExeFileStemNative(callback.ToHandlePtr(), OnStringCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_exe_file_stem")]
#else
    [DllImport("safe_app", EntryPoint = "app_exe_file_stem")]
#endif
    public static extern void AppExeFileStemNative(IntPtr self, StringCb callback);

    #endregion

    #region AppInitLogging

    public void AppInitLogging(string fileName, Action<FfiResult> callback) {
      AppInitLoggingNative(fileName, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_init_logging")]
#else
    [DllImport("safe_app", EntryPoint = "app_init_logging")]
#endif
    public static extern void AppInitLoggingNative(string fileName, IntPtr userDataPtr, ResultCb callback);

    #endregion

    #region AppOutputLogPath

    public void AppOutputLogPath(string fileName, Action<FfiResult, string> callback) {
      AppOutputLogPathNative(fileName, callback.ToHandlePtr(), OnStringCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_output_log_path")]
#else
    [DllImport("safe_app", EntryPoint = "app_output_log_path")]
#endif
    public static extern void AppOutputLogPathNative(string fileName, IntPtr userDataPtr, StringCb callback);

    #endregion

    #region AppPubSignKey

    public void AppPubSignKey(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      AppPubSignKeyNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_pub_sign_key")]
#else
    [DllImport("safe_app", EntryPoint = "app_pub_sign_key")]
#endif
    public static extern void AppPubSignKeyNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region AppRegistered

    public void AppRegistered(string appId, IntPtr ffiAuthGrantedPtr, Action onDisconnectedCb, Action<FfiResult, IntPtr> appRegCb) {
      var cbs = new List<object> {onDisconnectedCb, appRegCb};
      AppRegisteredNative(appId, ffiAuthGrantedPtr, cbs.ToHandlePtr(), OnDisconnectedCb, OnAppRegCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_registered")]
#else
    [DllImport("safe_app", EntryPoint = "app_registered")]
#endif
    public static extern void AppRegisteredNative(
      string appId,
      IntPtr ffiAuthGrantedPtr,
      IntPtr userData,
      DisconnectedCb netObsCb,
      AppRegCb appRegCb);

#if __IOS__
    [MonoPInvokeCallback(typeof(AppRegCb))]
#endif
    private static void OnAppRegCb(IntPtr self, IntPtr result, IntPtr appPtr) {
      var cb = (Action<FfiResult, IntPtr>)self.HandlePtrToType<List<object>>()[1];
      cb(Marshal.PtrToStructure<FfiResult>(result), appPtr);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DisconnectedCb))]
#endif
    private static void OnDisconnectedCb(IntPtr self) {
      var cb = (Action)self.HandlePtrToType<List<object>>()[0];
      cb();
    }

    #endregion

    #region AppSetAdditionalSearchPath

    public void AppSetAdditionalSearchPath(string path, Action<FfiResult> callback) {
      AppSetAdditionalSearchPathNative(path, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_set_additional_search_path")]
#else
    [DllImport("safe_app", EntryPoint = "app_set_additional_search_path")]
#endif
    public static extern void AppSetAdditionalSearchPathNative(string path, IntPtr self, ResultCb callback);

    #endregion

    #region CipherOptFree

    public void CipherOptFree(IntPtr appPtr, ulong cipherOptHandle, Action<FfiResult> callback) {
      CipherOptFreeNative(appPtr, cipherOptHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "cipher_opt_free")]
#else
    [DllImport("safe_app", EntryPoint = "cipher_opt_free")]
#endif
    public static extern void CipherOptFreeNative(IntPtr appPtr, ulong cipherOptHandle, IntPtr self, ResultCb callback);

    #endregion

    #region CipherOptNewPlaintext

    public void CipherOptNewPlaintext(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      CipherOptNewPlaintextNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "cipher_opt_new_plaintext")]
#else
    [DllImport("safe_app", EntryPoint = "cipher_opt_new_plaintext")]
#endif
    public static extern void CipherOptNewPlaintextNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region CipherOptNewSymmetric

    public void CipherOptNewSymmetric(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      CipherOptNewSymmetricNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "cipher_opt_new_symmetric")]
#else
    [DllImport("safe_app", EntryPoint = "cipher_opt_new_symmetric")]
#endif
    public static extern void CipherOptNewSymmetricNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region CipherOptNewAsymmetric

    public void CipherOptNewAsymmetric(IntPtr appPtr, ulong encryptPubKeyHandle, Action<FfiResult, ulong> callback) {
      CipherOptNewAsymmetricNative(appPtr, encryptPubKeyHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "cipher_opt_new_asymmetric")]
#else
    [DllImport("safe_app", EntryPoint = "cipher_opt_new_asymmetric")]
#endif
    public static extern void CipherOptNewAsymmetricNative(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtr self, UlongCb callback);

    #endregion

    #region DecodeIpcMessage

    public void DecodeIpcMessage(
      string encodedReq,
      Action<uint, IntPtr> authCb,
      Action<uint, IntPtr, IntPtr> unregCb,
      Action<uint> contCb,
      Action<uint> shareMDataCb,
      Action revokedCb,
      Action<FfiResult> errorCb) {
      var cbs = new List<object> {authCb, unregCb, contCb, shareMDataCb, revokedCb, errorCb};
      DecodeIpcMessageNative(
        encodedReq,
        cbs.ToHandlePtr(),
        OnDecodeAuthCb,
        OnDecodeUnregCb,
        OnDecodeContCb,
        OnDecodeShareMDataCb,
        OnDecodeRevokedCb,
        OnDecodeErrorCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "decode_ipc_msg")]
#else
    [DllImport("safe_app", EntryPoint = "decode_ipc_msg")]
#endif
    public static extern void DecodeIpcMessageNative(
      string encodedReq,
      IntPtr self,
      DecodeAuthCb authCb,
      DecodeUnregCb unregCb,
      DecodeContCb contCb,
      DecodeShareMDataCb shareMDataCb,
      DecodeRevokedCb revokedCb,
      DecodeErrorCb errorCb);

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeAuthCb))]
#endif
    private static void OnDecodeAuthCb(IntPtr self, uint reqId, IntPtr authGrantedFfiPtr) {
      var cb = (Action<uint, IntPtr>)self.HandlePtrToType<List<object>>()[0];
      cb(reqId, authGrantedFfiPtr);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeUnregCb))]
#endif
    private static void OnDecodeUnregCb(IntPtr self, uint reqId, IntPtr bsConfig, IntPtr bsSize) {
      var cb = (Action<uint, IntPtr, IntPtr>)self.HandlePtrToType<List<object>>()[1];
      cb(reqId, bsConfig, bsSize);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeContCb))]
#endif
    private static void OnDecodeContCb(IntPtr self, uint reqId) {
      var cb = (Action<uint>)self.HandlePtrToType<List<object>>()[2];
      cb(reqId);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeShareMDataCb))]
#endif
    private static void OnDecodeShareMDataCb(IntPtr self, uint reqId) {
      var cb = (Action<uint>)self.HandlePtrToType<List<object>>()[3];
      cb(reqId);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeRevokedCb))]
#endif
    private static void OnDecodeRevokedCb(IntPtr self) {
      var cb = (Action)self.HandlePtrToType<List<object>>()[4];
      cb();
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(DecodeErrorCb))]
#endif
    private static void OnDecodeErrorCb(IntPtr self, IntPtr result, uint id) {
      var cb = (Action<FfiResult>)self.HandlePtrToType<List<object>>()[5];
      cb(Marshal.PtrToStructure<FfiResult>(result));
    }

    #endregion

    #region DecryptSealedBox

    public void DecryptSealedBox(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong pkHandle,
      ulong skHandle,
      Action<FfiResult, IntPtr, IntPtr> callback) {
      DecryptSealedBoxNative(appPtr, data, len, pkHandle, skHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "decrypt_sealed_box")]
#else
    [DllImport("safe_app", EntryPoint = "decrypt_sealed_box")]
#endif
    public static extern void DecryptSealedBoxNative(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong pkHandle,
      ulong skHandle,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region EncGenerateKeyPair

    public void EncGenerateKeyPair(IntPtr appPtr, Action<FfiResult, ulong, ulong> callback) {
      EncGenerateKeyPairNative(appPtr, callback.ToHandlePtr(), OnEncGenerateKeyPairCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_generate_key_pair")]
#else
    [DllImport("safe_app", EntryPoint = "enc_generate_key_pair")]
#endif
    public static extern void EncGenerateKeyPairNative(IntPtr appPtr, IntPtr self, EncGenerateKeyPairCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(EncGenerateKeyPairCb))]
#endif
    private static void OnEncGenerateKeyPairCb(IntPtr self, IntPtr result, ulong encPubKeyHandle, ulong encSecKeyHandle) {
      var cb = self.HandlePtrToType<Action<FfiResult, ulong, ulong>>();
      cb(Marshal.PtrToStructure<FfiResult>(result), encPubKeyHandle, encSecKeyHandle);
    }

    #endregion

    #region SignGenerateKeyPair

    public void SignGenerateKeyPair(IntPtr appPtr, Action<FfiResult, ulong, ulong> callback) {
      SignGenerateKeyPairNative(appPtr, callback.ToHandlePtr(), OnSignGenerateKeyPairCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_generate_key_pair")]
#else
    [DllImport("safe_app", EntryPoint = "sign_generate_key_pair")]
#endif
    public static extern void SignGenerateKeyPairNative(IntPtr appPtr, IntPtr self, SignGenerateKeyPairCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(SignGenerateKeyPairCb))]
#endif
    private static void OnSignGenerateKeyPairCb(IntPtr self, IntPtr result, ulong signPubKeyHandle, ulong signSecKeyHandle) {
      var cb = self.HandlePtrToType<Action<FfiResult, ulong, ulong>>();
      cb(Marshal.PtrToStructure<FfiResult>(result), signPubKeyHandle, signSecKeyHandle);
    }

    #endregion

    #region SignPubKeyNew

    public void SignPubKeyNew(IntPtr appPtr, IntPtr signPublicKey, Action<FfiResult, ulong> callback) {
      SignPubKeyNewNative(appPtr, signPublicKey, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_pub_key_new")]
#else
    [DllImport("safe_app", EntryPoint = "sign_pub_key_new")]
#endif
    public static extern void SignPubKeyNewNative(IntPtr appPtr, IntPtr signPublicKey, IntPtr self, UlongCb callback);

    #endregion

    #region SignPubKeyGet

    public void SignPubKeyGet(IntPtr appPtr, ulong signPubKeyHandle, Action<FfiResult, IntPtr> callback) {
      SignPubKeyGetNative(appPtr, signPubKeyHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_pub_key_get")]
#else
    [DllImport("safe_app", EntryPoint = "sign_pub_key_get")]
#endif
    public static extern void SignPubKeyGetNative(IntPtr appPtr, ulong signPubKeyHandle, IntPtr self, IntPtrCb callback);

    #endregion

    #region SignPubKeyFree

    public void SignPubKeyFree(IntPtr appPtr, ulong signPubKeyHandle, Action<FfiResult> callback) {
      SignPubKeyFreeNative(appPtr, signPubKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_pub_key_free")]
#else
    [DllImport("safe_app", EntryPoint = "sign_pub_key_free")]
#endif
    public static extern void SignPubKeyFreeNative(IntPtr appPtr, ulong signPubKeyHandle, IntPtr self, ResultCb callback);

    #endregion

    #region SignSecKeyNew

    public void SignSecKeyNew(IntPtr appPtr, IntPtr signSecKey, Action<FfiResult, ulong> callback) {
      SignSecretKeyNewNative(appPtr, signSecKey, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_sec_key_new")]
#else
    [DllImport("safe_app", EntryPoint = "sign_sec_key_new")]
#endif
    public static extern void SignSecretKeyNewNative(IntPtr appPtr, IntPtr signSecKey, IntPtr self, UlongCb callback);

    #endregion

    #region SignSecKeyGet

    public void SignSecKeyGet(IntPtr appPtr, ulong signSecKeyHandle, Action<FfiResult, IntPtr> callback) {
      SignSecKeyGetNative(appPtr, signSecKeyHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_sec_key_get")]
#else
    [DllImport("safe_app", EntryPoint = "sign_sec_key_get")]
#endif
    public static extern void SignSecKeyGetNative(IntPtr appPtr, ulong signSecKeyHandle, IntPtr self, IntPtrCb callback);

    #endregion

    #region SignSecKeyFree

    public void SignSecKeyFree(IntPtr appPtr, ulong signSecKeyHandle, Action<FfiResult> callback) {
      SignSecKeyFreeNative(appPtr, signSecKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_sec_key_free")]
#else
    [DllImport("safe_app", EntryPoint = "sign_sec_key_free")]
#endif
    public static extern void SignSecKeyFreeNative(IntPtr appPtr, ulong signSecKeyHandle, IntPtr self, ResultCb callback);

    #endregion

    #region Encrypt

    public void Encrypt(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong encryptPubKeyHandle,
      ulong encryptSecKeyHandle,
      Action<FfiResult, IntPtr, IntPtr> callback) {
      EncryptNative(appPtr, data, len, encryptPubKeyHandle, encryptSecKeyHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "encrypt")]
#else
    [DllImport("safe_app", EntryPoint = "encrypt")]
#endif
    public static extern void EncryptNative(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong encryptPubKeyHandle,
      ulong encryptSecKeyHandle,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region Decrypt

    public void Decrypt(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong encryptPubKeyHandle,
      ulong encryptSecKeyHandle,
      Action<FfiResult, IntPtr, IntPtr> callback) {
      DecryptNative(appPtr, data, len, encryptPubKeyHandle, encryptSecKeyHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "decrypt")]
#else
    [DllImport("safe_app", EntryPoint = "decrypt")]
#endif
    public static extern void DecryptNative(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong encryptPubKeyHandle,
      ulong encryptSecKeyHandle,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region Sign

    public void Sign(IntPtr appPtr, IntPtr data, IntPtr len, ulong signSeckeyHandle, Action<FfiResult, IntPtr, IntPtr> callback) {
      SignNative(appPtr, data, len, signSeckeyHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }
#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign")]
#else
    [DllImport("safe_app", EntryPoint = "sign")]
#endif
    public static extern void SignNative(IntPtr appPtr, IntPtr data, IntPtr len, ulong signSeckeyHandle, IntPtr self, ByteArrayCb callback);

    #endregion

    #region Verify

    public void Verify(IntPtr appPtr, IntPtr signdata, IntPtr len, ulong signPubkeyHandle, Action<FfiResult, IntPtr, IntPtr> callback) {
      VerifyNative(appPtr, signdata, len, signPubkeyHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }
#if __IOS__
    [DllImport("__Internal", EntryPoint = "verify")]
#else
    [DllImport("safe_app", EntryPoint = "verify")]
#endif
    public static extern void VerifyNative(
      IntPtr appPtr,
      IntPtr signdata,
      IntPtr len,
      ulong signPubkeyHandle,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region GenerateNonce

    public void GenerateNonce(Action<FfiResult, IntPtr> callback) {
      Generate_NonceNative(callback.ToHandlePtr(), OnIntPtrCb);
    }
#if __IOS__
    [DllImport("__Internal", EntryPoint = "generate_nonce")]
#else
    [DllImport("safe_app", EntryPoint = "generate_nonce")]
#endif
    public static extern void Generate_NonceNative(IntPtr self, IntPtrCb callback);

    #endregion

    #region EncodeAuthReq

    public void EncodeAuthReq(IntPtr authReq, Action<FfiResult, uint, string> callback) {
      EncodeAuthReqNative(authReq, callback.ToHandlePtr(), OnEncodeAuthReqCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "encode_auth_req")]
#else
    [DllImport("safe_app", EntryPoint = "encode_auth_req")]
#endif
    public static extern void EncodeAuthReqNative(IntPtr authReq, IntPtr userDataPtr, EncodeAuthReqCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(EncodeAuthReqCb))]
#endif
    private static void OnEncodeAuthReqCb(IntPtr self, IntPtr result, uint requestId, string encodedReq) {
      var cb = self.HandlePtrToType<Action<FfiResult, uint, string>>();
      cb(Marshal.PtrToStructure<FfiResult>(result), requestId, encodedReq);
    }

    #endregion

    #region EncPubKeyFree

    public void EncPubKeyFree(IntPtr appPtr, ulong encryptPubKeyHandle, Action<FfiResult> callback) {
      EncPubKeyFreeNative(appPtr, encryptPubKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_pub_key_free")]
#else
    [DllImport("safe_app", EntryPoint = "enc_pub_key_free")]
#endif
    public static extern void EncPubKeyFreeNative(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtr self, ResultCb callback);

    #endregion

    #region EncPubKeyGet

    public void EncPubKeyGet(IntPtr appPtr, ulong encryptPubKeyHandle, Action<FfiResult, IntPtr> callback) {
      EncPubKeyGetNative(appPtr, encryptPubKeyHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_pub_key_get")]
#else
    [DllImport("safe_app", EntryPoint = "enc_pub_key_get")]
#endif
    public static extern void EncPubKeyGetNative(IntPtr appPtr, ulong encryptPubKeyHandle, IntPtr self, IntPtrCb callback);

    #endregion

    #region EncPubKeyNew

    public void EncPubKeyNew(IntPtr appPtr, IntPtr asymPublicKey, Action<FfiResult, ulong> callback) {
      EncPubKeyNewNative(appPtr, asymPublicKey, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_pub_key_new")]
#else
    [DllImport("safe_app", EntryPoint = "enc_pub_key_new")]
#endif
    public static extern void EncPubKeyNewNative(IntPtr appPtr, IntPtr asymPublicKey, IntPtr self, UlongCb callback);

    #endregion

    #region EncryptSealedBox

    public void EncryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, Action<FfiResult, IntPtr, IntPtr> callback) {
      EncryptSealedBoxNative(appPtr, data, len, pkHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "encrypt_sealed_box")]
#else
    [DllImport("safe_app", EntryPoint = "encrypt_sealed_box")]
#endif
    public static extern void EncryptSealedBoxNative(
      IntPtr appPtr,
      IntPtr data,
      IntPtr len,
      ulong pkHandle,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region EncSecretKeyFree

    public void EncSecretKeyFree(IntPtr appPtr, ulong encryptSecKeyHandle, Action<FfiResult> callback) {
      EncSecretKeyFreeNative(appPtr, encryptSecKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_secret_key_free")]
#else
    [DllImport("safe_app", EntryPoint = "enc_secret_key_free")]
#endif
    public static extern void EncSecretKeyFreeNative(IntPtr appPtr, ulong encryptSecKeyHandle, IntPtr self, ResultCb callback);

    #endregion

    #region EncSecretKeyGet

    public void EncSecretKeyGet(IntPtr appPtr, ulong encryptSecKeyHandle, Action<FfiResult, IntPtr> callback) {
      EncSecretKeyGetNative(appPtr, encryptSecKeyHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_secret_key_get")]
#else
    [DllImport("safe_app", EntryPoint = "enc_secret_key_get")]
#endif
    public static extern void EncSecretKeyGetNative(IntPtr appPtr, ulong encryptSecKeyHandle, IntPtr self, IntPtrCb callback);

    #endregion

    #region EncSecretKeyNew

    public void EncSecretKeyNew(IntPtr appPtr, IntPtr asymSecretKey, Action<FfiResult, ulong> callback) {
      EncSecretKeyNewNative(appPtr, asymSecretKey, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "enc_secret_key_new")]
#else
    [DllImport("safe_app", EntryPoint = "enc_secret_key_new")]
#endif
    public static extern void EncSecretKeyNewNative(IntPtr appPtr, IntPtr asymSecretKey, IntPtr self, UlongCb callback);

    #endregion

    #region FreeAppNative

    public void FreeApp(IntPtr appPtr) {
      FreeAppNative(appPtr);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "app_free")]
#else
    [DllImport("safe_app", EntryPoint = "app_free")]
#endif
    public static extern void FreeAppNative(IntPtr appPtr);

    #endregion

    #region IDataCloseSelfEncryptor

    public void IDataCloseSelfEncryptor(IntPtr appPtr, ulong seHandle, ulong cipherOptHandle, Action<FfiResult, IntPtr> callback) {
      IDataCloseSelfEncryptorNative(appPtr, seHandle, cipherOptHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_close_self_encryptor")]
#else
    [DllImport("safe_app", EntryPoint = "idata_close_self_encryptor")]
#endif
    public static extern void IDataCloseSelfEncryptorNative(
      IntPtr appPtr,
      ulong seHandle,
      ulong cipherOptHandle,
      IntPtr self,
      IntPtrCb callback);

    #endregion

    #region IDataFetchSelfEncryptor

    public void IDataFetchSelfEncryptor(IntPtr appPtr, IntPtr xorNameArr, Action<FfiResult, ulong> callback) {
      IDataFetchSelfEncryptorNative(appPtr, xorNameArr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_fetch_self_encryptor")]
#else
    [DllImport("safe_app", EntryPoint = "idata_fetch_self_encryptor")]
#endif
    public static extern void IDataFetchSelfEncryptorNative(IntPtr appPtr, IntPtr xorNameArr, IntPtr self, UlongCb callback);

    #endregion

    #region IDataNewSelfEncryptor

    public void IDataNewSelfEncryptor(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      IDataNewSelfEncryptorNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_new_self_encryptor")]
#else
    [DllImport("safe_app", EntryPoint = "idata_new_self_encryptor")]
#endif
    public static extern void IDataNewSelfEncryptorNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region IDataReadFromSelfEncryptor

    public void IDataReadFromSelfEncryptor(
      IntPtr appPtr,
      ulong seHandle,
      ulong fromPos,
      ulong len,
      Action<FfiResult, IntPtr, IntPtr> callback) {
      IDataReadFromSelfEncryptorNative(appPtr, seHandle, fromPos, len, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_read_from_self_encryptor")]
#else
    [DllImport("safe_app", EntryPoint = "idata_read_from_self_encryptor")]
#endif
    public static extern void IDataReadFromSelfEncryptorNative(
      IntPtr appPtr,
      ulong seHandle,
      ulong fromPos,
      ulong len,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region IDataSelfEncryptorReaderFree

    public void IDataSelfEncryptorReaderFree(IntPtr appPtr, ulong sEReaderHandle, Action<FfiResult> callback) {
      IDataSelfEncryptorReaderFreeNative(appPtr, sEReaderHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_self_encryptor_reader_free")]
#else
    [DllImport("safe_app", EntryPoint = "idata_self_encryptor_reader_free")]
#endif
    public static extern void IDataSelfEncryptorReaderFreeNative(IntPtr appPtr, ulong sEReaderHandle, IntPtr self, ResultCb callback);

    #endregion

    #region IDataSelfEncryptorWriterFree

    public void IDataSelfEncryptorWriterFree(IntPtr appPtr, ulong sEWriterHandle, Action<FfiResult> callback) {
      IDataSelfEncryptorWriterFreeNative(appPtr, sEWriterHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_self_encryptor_writer_free")]
#else
    [DllImport("safe_app", EntryPoint = "idata_self_encryptor_writer_free")]
#endif
    public static extern void IDataSelfEncryptorWriterFreeNative(IntPtr appPtr, ulong sEWriterHandle, IntPtr self, ResultCb callback);

    #endregion

    #region IDataSize

    public void IDataSize(IntPtr appPtr, ulong seHandle, Action<FfiResult, ulong> callback) {
      IDataSizeNative(appPtr, seHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_size")]
#else
    [DllImport("safe_app", EntryPoint = "idata_size")]
#endif
    public static extern void IDataSizeNative(IntPtr appPtr, ulong seHandle, IntPtr self, UlongCb callback);

    #endregion

    #region IDataWriteToSelfEncryptor

    public void IDataWriteToSelfEncryptor(IntPtr appPtr, ulong seHandle, IntPtr data, IntPtr size, Action<FfiResult> callback) {
      IDataWriteToSelfEncryptorNative(appPtr, seHandle, data, size, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "idata_write_to_self_encryptor")]
#else
    [DllImport("safe_app", EntryPoint = "idata_write_to_self_encryptor")]
#endif
    public static extern void IDataWriteToSelfEncryptorNative(
      IntPtr appPtr,
      ulong seHandle,
      IntPtr data,
      IntPtr size,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region MDataEntriesForEach

    public void MDataEntriesForEach(
      IntPtr appPtr,
      ulong entriesHandle,
      Action<IntPtr, IntPtr, IntPtr, IntPtr, ulong> forEachCallback,
      Action<FfiResult> resultCallback) {
      var cbs = new List<object> {forEachCallback, resultCallback};
      MDataEntriesForEachNative(appPtr, entriesHandle, cbs.ToHandlePtr(), OnMDataEntriesForEachCb, OnEntriesForEachResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_for_each")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entries_for_each")]
#endif
    public static extern void MDataEntriesForEachNative(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr self,
      MDataEntriesForEachCb forEachCallback,
      EntriesForEachResultCb resultCallback);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataEntriesForEachCb))]
#endif
    private static void OnMDataEntriesForEachCb(
      IntPtr self,
      IntPtr entryKey,
      IntPtr entryKeyLen,
      IntPtr entryVal,
      IntPtr entryValLen,
      ulong entryVersion) {
      var cb = (Action<IntPtr, IntPtr, IntPtr, IntPtr, ulong>)self.HandlePtrToType<List<object>>(false)[0];
      cb(entryKey, entryKeyLen, entryVal, entryValLen, entryVersion);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(EntriesForEachResultCb))]
#endif
    private static void OnEntriesForEachResultCb(IntPtr self, IntPtr result) {
      var cb = (Action<FfiResult>)self.HandlePtrToType<List<object>>(false)[1];
      cb(Marshal.PtrToStructure<FfiResult>(result));
    }

    #endregion

    #region MDataEntriesFree

    public void MDataEntriesFree(IntPtr appPtr, ulong entriesHandle, Action<FfiResult> callback) {
      MDataEntriesFreeNative(appPtr, entriesHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_free")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entries_free")]
#endif
    public static extern void MDataEntriesFreeNative(IntPtr appPtr, ulong entriesHandle, IntPtr self, ResultCb callback);

    #endregion

    #region MDataEntriesInsert

    public void MDataEntriesInsert(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      Action<FfiResult> callback) {
      MDataEntriesInsertNative(appPtr, entriesHandle, keyPtr, keyLen, valuePtr, valueLen, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_insert")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entries_insert")]
#endif
    public static extern void MDataEntriesInsertNative(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region MDataEntriesLen

    public void MDataEntriesLen(IntPtr appPtr, ulong entriesHandle, Action<FfiResult, ulong> callback) {
      MDataEntriesLenNative(appPtr, entriesHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_len")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entries_len")]
#endif
    public static extern void MDataEntriesLenNative(IntPtr appPtr, ulong entriesHandle, IntPtr self, UlongCb callback);

    #endregion

    #region MDataEntriesNew

    public void MDataEntriesNew(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      MDataEntriesNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entries_new")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entries_new")]
#endif
    public static extern void MDataEntriesNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region MDataEntryActionsFree

    public void MDataEntryActionsFree(IntPtr appPtr, ulong actionsHandle, Action<FfiResult> callback) {
      MDataEntryActionsFreeNative(appPtr, actionsHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entry_actions_free")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entry_actions_free")]
#endif
    public static extern void MDataEntryActionsFreeNative(IntPtr appPtr, ulong actionsHandle, IntPtr self, ResultCb callback);

    #endregion

    #region MDataEntryActionsInsert

    public void MDataEntryActionsInsert(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      Action<FfiResult> callback) {
      MDataEntryActionsInsertNative(appPtr, actionsHandle, keyPtr, keyLen, valuePtr, valueLen, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entry_actions_insert")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entry_actions_insert")]
#endif
    public static extern void MDataEntryActionsInsertNative(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region MDataEntryActionsNew

    public void MDataEntryActionsNew(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      MDataEntryActionsNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_entry_actions_new")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_entry_actions_new")]
#endif
    public static extern void MDataEntryActionsNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region MDataGetValue

    public void MDataGetValue(IntPtr appPtr, IntPtr info, IntPtr keyPtr, IntPtr keyLen, Action<FfiResult, IntPtr, IntPtr, ulong> callback) {
      MDataGetValueNative(appPtr, info, keyPtr, keyLen, callback.ToHandlePtr(), OnMDataGetValueCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_get_value")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_get_value")]
#endif
    public static extern void MDataGetValueNative(
      IntPtr appPtr,
      IntPtr info,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr self,
      MDataGetValueCb callback);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataGetValueCb))]
#endif
    private static void OnMDataGetValueCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen, ulong entryVersion) {
      var cb = self.HandlePtrToType<Action<FfiResult, IntPtr, IntPtr, ulong>>();
      cb(result, data, dataLen, entryVersion);
    }

    #endregion

    #region MDataInfoDecrypt

    public void MDataInfoDecrypt(IntPtr mDataInfoPtr, IntPtr cipherText, IntPtr cipherLen, Action<FfiResult, IntPtr, IntPtr> callback) {
      MDataInfoDecryptNative(mDataInfoPtr, cipherText, cipherLen, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_decrypt")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_decrypt")]
#endif
    public static extern void MDataInfoDecryptNative(
      IntPtr mDataInfoPtr,
      IntPtr cipherText,
      IntPtr cipherLen,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region MDataInfoDeserialise

    public void MDataInfoDeserialise(IntPtr ptr, IntPtr len, Action<FfiResult, IntPtr> callback) {
      MDataInfoDeserialiseNative(ptr, len, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_deserialise")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_deserialise")]
#endif
    public static extern void MDataInfoDeserialiseNative(IntPtr ptr, IntPtr len, IntPtr self, IntPtrCb callback);

    #endregion

    #region MDataInfoEncryptEntryKey

    public void MDataInfoEncryptEntryKey(IntPtr infoH, IntPtr inputPtr, IntPtr inputLen, Action<FfiResult, IntPtr, IntPtr> callback) {
      MDataInfoEncryptEntryKeyNative(infoH, inputPtr, inputLen, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_encrypt_entry_key")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_encrypt_entry_key")]
#endif
    public static extern void MDataInfoEncryptEntryKeyNative(
      IntPtr infoH,
      IntPtr inputPtr,
      IntPtr inputLen,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region MDataInfoEncryptEntryValue

    public void MDataInfoEncryptEntryValue(IntPtr infoH, IntPtr inputPtr, IntPtr inputLen, Action<FfiResult, IntPtr, IntPtr> callback) {
      MDataInfoEncryptEntryValueNative(infoH, inputPtr, inputLen, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_encrypt_entry_value")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_encrypt_entry_value")]
#endif
    public static extern void MDataInfoEncryptEntryValueNative(
      IntPtr infoH,
      IntPtr inputPtr,
      IntPtr inputLen,
      IntPtr self,
      ByteArrayCb callback);

    #endregion

    #region MDataInfoFree

    public void MDataInfoFree(IntPtr appPtr, ulong infoHandle, ResultCb callback) {
      MDataInfoFreeNative(appPtr, infoHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_free")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_free")]
#endif
    public static extern void MDataInfoFreeNative(IntPtr appPtr, ulong infoHandle, IntPtr self, ResultCb callback);

    #endregion

    #region MDataInfoNewPublic

    public void MDataInfoNewPublic(IntPtr xorNameArr, ulong typeTag, Action<FfiResult, IntPtr> callback) {
      MDataInfoNewPublicNative(xorNameArr, typeTag, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_new_public")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_new_public")]
#endif
    public static extern void MDataInfoNewPublicNative(IntPtr xorNameArr, ulong typeTag, IntPtr self, IntPtrCb callback);

    #endregion

    #region MDataInfoRandomPrivate

    public void MDataInfoRandomPrivate(ulong typeTag, Action<FfiResult, IntPtr> callback) {
      MDataInfoRandomPrivateNative(typeTag, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_random_private")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_random_private")]
#endif
    public static extern void MDataInfoRandomPrivateNative(ulong typeTag, IntPtr self, IntPtrCb callback);

    #endregion

    #region MDataInfoRandomPublic

    public void MDataInfoRandomPublic(ulong typeTag, Action<FfiResult, IntPtr> callback) {
      MDataInfoRandomPublicNative(typeTag, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_random_public")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_random_public")]
#endif
    public static extern void MDataInfoRandomPublicNative(ulong typeTag, IntPtr self, IntPtrCb callback);

    #endregion

    #region MDataInfoSerialise

    public void MDataInfoSerialise(IntPtr infoHandle, Action<FfiResult, IntPtr, IntPtr> callback) {
      MDataInfoSerialiseNative(infoHandle, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_info_serialise")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_info_serialise")]
#endif
    public static extern void MDataInfoSerialiseNative(IntPtr infoHandle, IntPtr self, ByteArrayCb callback);

    #endregion

    #region MDataKeysForEach

    public void MDataKeysForEach(IntPtr appPtr, ulong keysHandle, Action<MDataKeyFfi> forEachCb, Action<FfiResult> resCb) {
      var cbs = new List<object> {forEachCb, resCb};
      var a = cbs.ToHandlePtr();
      Debug.WriteLine(a);
      MDataKeysForEachNative(appPtr, keysHandle, a, OnMDataKeysForEachCb, OnMDataKeysForEachResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_keys_for_each")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_keys_for_each")]
#endif
    public static extern void MDataKeysForEachNative(
      IntPtr appPtr,
      ulong keysHandle,
      IntPtr self,
      MDataKeysForEachCb forEachCb,
      MDataKeysForEachResultCb resCb);

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataKeysForEachCb))]
#endif
    private static void OnMDataKeysForEachCb(IntPtr self, IntPtr mdataKey) {
      var cb = (Action<MDataKeyFfi>)self.HandlePtrToType<List<object>>(false)[0];
      cb(Marshal.PtrToStructure<MDataKeyFfi>(mdataKey));
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataKeysForEachResultCb))]
#endif
    private static void OnMDataKeysForEachResultCb(IntPtr self, IntPtr result) {
      var cb = (Action<FfiResult>)self.HandlePtrToType<List<object>>(false)[1];
      cb(Marshal.PtrToStructure<FfiResult>(result));
    }

    #endregion

    #region MDataKeysFree

    public void MDataKeysFree(IntPtr appPtr, ulong keysHandle, Action<FfiResult> callback) {
      MDataKeysFreeNative(appPtr, keysHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_keys_free")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_keys_free")]
#endif
    public static extern void MDataKeysFreeNative(IntPtr appPtr, ulong keysHandle, IntPtr self, ResultCb callback);

    #endregion

    #region MDataKeysLen

    public void MDataKeysLen(IntPtr appPtr, ulong keysHandle, Action<FfiResult, IntPtr> callback) {
      MDataKeysLenNative(appPtr, keysHandle, callback.ToHandlePtr(), OnIntPtrCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_keys_len")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_keys_len")]
#endif
    public static extern void MDataKeysLenNative(IntPtr appPtr, ulong keysHandle, IntPtr self, IntPtrCb callback);

    #endregion

    #region MDataListEntries

    public void MDataListEntries(IntPtr appPtr, IntPtr infoHandle, Action<FfiResult, ulong> callback) {
      MDataListEntriesNative(appPtr, infoHandle, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_list_entries")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_list_entries")]
#endif
    public static extern void MDataListEntriesNative(IntPtr appPtr, IntPtr infoHandle, IntPtr self, UlongCb callback);

    #endregion

    #region MDataListKeys

    public void MDataListKeys(IntPtr appPtr, IntPtr infoHandle, Action<FfiResult, List<MDataKeyFfi>> callback) {
      MDataListKeysNative(appPtr, infoHandle, callback.ToHandlePtr(), OnMDataKeyListCb);
    }

#if __IOS__
    [MonoPInvokeCallback(typeof(MDataKeyListCb))]
#endif
    private static void OnMDataKeyListCb(IntPtr self, IntPtr result, IntPtr listPtr, IntPtr size) {
      var cb = self.HandlePtrToType<Action<FfiResult, List<MDataKeyFfi>>>();
      cb(Marshal.PtrToStructure<FfiResult>(result), listPtr.ToList<MDataKeyFfi>(size));
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_list_keys")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_list_keys")]
#endif
    public static extern void MDataListKeysNative(IntPtr appPtr, IntPtr infoHandle, IntPtr self, MDataKeyListCb callback);

    #endregion

    #region MDataMutateEntries

    public void MDataMutateEntries(IntPtr appPtr, IntPtr infoHandle, ulong actionsHandle, Action<FfiResult> callback) {
      MDataMutateEntriesNative(appPtr, infoHandle, actionsHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_mutate_entries")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_mutate_entries")]
#endif
    public static extern void MDataMutateEntriesNative(
      IntPtr appPtr,
      IntPtr infoHandle,
      ulong actionsHandle,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region MDataPermissionSetAllow

    public void MDataPermissionSetAllow(IntPtr appPtr, ulong setHandle, MDataAction action, ResultCb callback) {
      MDataPermissionSetAllowNative(appPtr, setHandle, action, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permission_set_allow")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_permission_set_allow")]
#endif
    public static extern void MDataPermissionSetAllowNative(
      IntPtr appPtr,
      ulong setHandle,
      MDataAction action,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region MDataPermissionSetFree

    public void MDataPermissionSetFree(IntPtr appPtr, ulong setHandle, ResultCb callback) {
      MDataPermissionSetFreeNative(appPtr, setHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permission_set_free")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_permission_set_free")]
#endif
    public static extern void MDataPermissionSetFreeNative(IntPtr appPtr, ulong setHandle, IntPtr self, ResultCb callback);

    #endregion

    #region MDataPermissionSetNew

    public void MDataPermissionSetNew(IntPtr appPtr, UlongCb callback) {
      MDataPermissionSetNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permission_set_new")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_permission_set_new")]
#endif
    public static extern void MDataPermissionSetNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region MDataPermissionsFree

    public void MDataPermissionsFree(IntPtr appPtr, ulong permissionsHandle, Action<FfiResult> callback) {
      MDataPermissionsFreeNative(appPtr, permissionsHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permissions_free")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_permissions_free")]
#endif
    public static extern void MDataPermissionsFreeNative(IntPtr appPtr, ulong permissionsHandle, IntPtr self, ResultCb callback);

    #endregion

    #region MDataPermissionsInsert

    public void MDataPermissionsInsert(
      IntPtr appPtr,
      ulong permissionsHandle,
      ulong userHandle,
      IntPtr permissionSetPtr,
      Action<FfiResult> callback) {
      MDataPermissionsInsertNative(appPtr, permissionsHandle, userHandle, permissionSetPtr, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permissions_insert")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_permissions_insert")]
#endif
    public static extern void MDataPermissionsInsertNative(
      IntPtr appPtr,
      ulong permissionsHandle,
      ulong userHandle,
      IntPtr permissionSetPtr,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region MDataPermissionsNew

    public void MDataPermissionsNew(IntPtr appPtr, Action<FfiResult, ulong> callback) {
      MDataPermissionsNewNative(appPtr, callback.ToHandlePtr(), OnUlongCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_permissions_new")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_permissions_new")]
#endif
    public static extern void MDataPermissionsNewNative(IntPtr appPtr, IntPtr self, UlongCb callback);

    #endregion

    #region MDataPut

    public void MDataPut(IntPtr appPtr, IntPtr infoHandle, ulong permissionsHandle, ulong entriesHandle, Action<FfiResult> callback) {
      MDataPutNative(appPtr, infoHandle, permissionsHandle, entriesHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "mdata_put")]
#else
    [DllImport("safe_app", EntryPoint = "mdata_put")]
#endif
    public static extern void MDataPutNative(
      IntPtr appPtr,
      IntPtr infoHandle,
      ulong permissionsHandle,
      ulong entriesHandle,
      IntPtr self,
      ResultCb callback);

    #endregion

    #region Sha3Hash

    public void Sha3Hash(IntPtr data, IntPtr len, Action<FfiResult, IntPtr, IntPtr> callback) {
      Sha3HashNative(data, len, callback.ToHandlePtr(), OnByteArrayCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sha3_hash")]
#else
    [DllImport("safe_app", EntryPoint = "sha3_hash")]
#endif
    public static extern void Sha3HashNative(IntPtr data, IntPtr len, IntPtr self, ByteArrayCb callback);

    #endregion

    #region SignKeyFree

    public void SignKeyFree(IntPtr appPtr, ulong signKeyHandle, ResultCb callback) {
      SignKeyFreeNative(appPtr, signKeyHandle, callback.ToHandlePtr(), OnResultCb);
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "sign_key_free")]
#else
    [DllImport("safe_app", EntryPoint = "sign_key_free")]
#endif
    public static extern void SignKeyFreeNative(IntPtr appPtr, ulong signKeyHandle, IntPtr self, ResultCb callback);

    #endregion
  }
}
#endif
