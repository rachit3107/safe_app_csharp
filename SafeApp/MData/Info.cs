using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData {
  public static class Info {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static Task<List<byte>> DecryptAsync(MDataInfo mDataInfo, List<byte> cipherText) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var mdataInfoPtr = Helpers.StructToPtr(mDataInfo);
      var cipherPtr = cipherText.ToIntPtr();
      var cipherLen = (IntPtr)cipherText.Count;

      Action<FfiResult, IntPtr, IntPtr> callback = (result, plainText, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        var byteList = plainText.ToList<byte>(len);
        tcs.SetResult(byteList);
      };

      AppBindings.MDataInfoDecrypt(mdataInfoPtr, cipherPtr, cipherLen, callback);
      Marshal.FreeHGlobal(cipherPtr);
      Marshal.FreeHGlobal(mdataInfoPtr);
      return tcs.Task;
    }

    public static Task<MDataInfo> DeserialiseAsync(List<byte> serialisedData) {
      var tcs = new TaskCompletionSource<MDataInfo>();
      Action<FfiResult, IntPtr> callback = (result, mdataInfoPtr) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(Marshal.PtrToStructure<MDataInfo>(mdataInfoPtr));
      };

      var serialisedDataPtr = serialisedData.ToIntPtr();
      AppBindings.MDataInfoDeserialise(serialisedDataPtr, (IntPtr)serialisedData.Count, callback);

      Marshal.FreeHGlobal(serialisedDataPtr);

      return tcs.Task;
    }

    public static Task<List<byte>> EncryptEntryKeyAsync(MDataInfo info, List<byte> inputBytes) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var inputBytesPtr = inputBytes.ToIntPtr();
      var infoPtr = Helpers.StructToPtr(info);
      Action<FfiResult, IntPtr, IntPtr> callback = (result, dataPtr, dataLen) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }
        var data = dataPtr.ToList<byte>(dataLen);
        tcs.SetResult(data);
      };

      AppBindings.MDataInfoEncryptEntryKey(infoPtr, inputBytesPtr, (IntPtr)inputBytes.Count, callback);
      Marshal.FreeHGlobal(inputBytesPtr);
      Marshal.FreeHGlobal(infoPtr);
      return tcs.Task;
    }

    public static Task<List<byte>> EncryptEntryValueAsync(MDataInfo info, List<byte> inputBytes) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var inputBytesPtr = inputBytes.ToIntPtr();
      var infoPtr = Helpers.StructToPtr(info);
      Action<FfiResult, IntPtr, IntPtr> callback = (result, dataPtr, dataLen) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }
        var data = dataPtr.ToList<byte>(dataLen);
        tcs.SetResult(data);
      };

      AppBindings.MDataInfoEncryptEntryValue(infoPtr, inputBytesPtr, (IntPtr)inputBytes.Count, callback);
      Marshal.FreeHGlobal(inputBytesPtr);
      Marshal.FreeHGlobal(infoPtr);

      return tcs.Task;
    }

    public static Task<MDataInfo> NewPublicAsync(List<byte> xorName, ulong typeTag) {
      var tcs = new TaskCompletionSource<MDataInfo>();

      Action<FfiResult, IntPtr> callback = (result, pubMDataInfoPtr) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(Marshal.PtrToStructure<MDataInfo>(pubMDataInfoPtr));
      };

      var xorNamePtr = xorName.ToIntPtr();
      AppBindings.MDataInfoNewPublic(xorNamePtr, typeTag, callback);
      Marshal.FreeHGlobal(xorNamePtr);

      return tcs.Task;
    }

    public static Task<MDataInfo> RandomPrivateAsync(ulong typeTag) {
      var tcs = new TaskCompletionSource<MDataInfo>();

      Action<FfiResult, IntPtr> callback = (result, privateMDataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(Marshal.PtrToStructure<MDataInfo>(privateMDataInfoH));
      };

      AppBindings.MDataInfoRandomPrivate(typeTag, callback);

      return tcs.Task;
    }

    public static Task<MDataInfo> RandomPublicAsync(ulong typeTag) {
      var tcs = new TaskCompletionSource<MDataInfo>();

      Action<FfiResult, IntPtr> callback = (result, pubMDataInfoH) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(Marshal.PtrToStructure<MDataInfo>(pubMDataInfoH));
      };

      AppBindings.MDataInfoRandomPublic(typeTag, callback);

      return tcs.Task;
    }

    public static Task<List<byte>> SerialiseAsync(MDataInfo mdataInfo) {
      var tcs = new TaskCompletionSource<List<byte>>();
      var mdataInfoPtr = mdataInfo.ToHandlePtr();
      Action<FfiResult, IntPtr, IntPtr> callback = (result, bytesPtr, len) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(bytesPtr.ToList<byte>(len));
      };

      AppBindings.MDataInfoSerialise(mdataInfoPtr, callback);
      Marshal.FreeHGlobal(mdataInfoPtr);

      return tcs.Task;
    }
  }
}
