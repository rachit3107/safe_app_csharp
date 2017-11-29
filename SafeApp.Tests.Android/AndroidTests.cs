using System;
using System.IO;
using Android.App;
using NUnit.Framework;

namespace SafeApp.Tests.Android {
  [TestFixture]
  public class AndroidTests {
    [SetUp]
    public void Setup() { }

    [TearDown]
    public void Tear() { }

    [Test]
    public void RustLog() {
      var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

      using (var reader = new StreamReader(Application.Context.Assets.Open("log.toml"))) {
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
  }
}
