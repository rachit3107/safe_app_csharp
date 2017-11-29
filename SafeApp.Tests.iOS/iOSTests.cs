using System;
using System.IO;
using NUnit.Framework;

namespace SafeApp.Tests.iOS {
  [TestFixture]
#pragma warning disable IDE1006 // Naming Styles
  // ReSharper disable once InconsistentNaming
  public class iOSTests {
    [SetUp]
    public void Setup() { }

    [TearDown]
    public void Tear() { }

    [Test]
    public void RustLog() {
      var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);

      using (var reader = new StreamReader(Path.Combine(".", "log.toml"))) {
        using (var writer = new StreamWriter(Path.Combine(configPath, "log.toml"))) {
          writer.Write(reader.ReadToEnd());
          writer.Close();
        }
        reader.Close();
      }

      Assert.DoesNotThrowAsync(async () => await Session.InitLoggingAsync(configPath));
      Assert.ThrowsAsync<Exception>(async () => await Session.DecodeIpcMessageAsync("Some Random Invalid String"));
      Assert.IsFalse(string.IsNullOrEmpty(File.ReadAllText(Path.Combine(configPath, "Client.log"))));
    }
#pragma warning restore IDE1006 // Naming Styles
  }
}
