﻿using System;
using System.Threading.Tasks;

namespace SafeApp.Utilities {
  public class NativeHandle : IDisposable {
    public static readonly NativeHandle Zero = new NativeHandle(0, null);

    private readonly Func<ulong, Task> _disposer;
    private readonly ulong _handle;

    public NativeHandle(ulong handle, Func<ulong, Task> disposer) {
      _disposer = disposer;
      _handle = handle;
    }

    public void Dispose() {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~NativeHandle() {
      ReleaseUnmanagedResources();
    }

    public static implicit operator ulong(NativeHandle obj) {
      return obj?._handle ?? 0;
    }

    private void ReleaseUnmanagedResources() {
      _disposer?.Invoke(_handle);
    }
  }
}
