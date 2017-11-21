using System;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.MData;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  public static class AccessContainer {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    
    public static Task<MDataInfo> GetMDataInfoAsync(string containerId) {
      var tcs = new TaskCompletionSource<MDataInfo>();

      Action<FfiResult, MDataInfo> callback = (result, mdataInfo) => {
        if (result.ErrorCode != 0) {
          tcs.SetException(result.ToException());
          return;
        }

        tcs.SetResult(mdataInfo);
      };

      AppBindings.AccessContainerGetContainerMDataInfo(Session.AppPtr, containerId, callback);

      return tcs.Task;
    }
  }
}
