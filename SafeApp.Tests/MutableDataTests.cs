using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.MData;
using SafeApp.Misc;
using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class MutableDataTests {
    [Test]
    public async Task RandomPrivateMutableDataUpdateAction() {
      Utils.InitialiseSessionForRandomTestApp();
      const ulong tagType = 15001;
      const string actKey = "sample_key";
      const string actValue = "sample_value";
      var mdInfo = await Info.RandomPrivateAsync(tagType);
      var permissionSet = new PermissionSet {Insert = true, ManagePermissions = true, Update = false, Read = true};
      using (var permissionsH = await MDataPermissions.NewAsync()) {
        using (var appSignKeyH = await Crypto.AppPubSignKeyAsync()) {
          await MDataPermissions.InsertAsync(permissionsH, appSignKeyH, permissionSet);
          await MData.MData.PutAsync(mdInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entryActionsH = await MDataEntryActions.NewAsync()) {
        var key = Encoding.Default.GetBytes(actKey).ToList();
        var value = Encoding.Default.GetBytes(actValue).ToList();
        key = await Info.EncryptEntryKeyAsync(mdInfo, key);
        value = await Info.EncryptEntryValueAsync(mdInfo, value);
        await MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await MData.MData.MutateEntriesAsync(mdInfo, entryActionsH);
      }

      var keys = await MData.MData.ListKeysAsync(mdInfo);
      Assert.AreEqual(1, keys.Count);

      using (var entriesHandle = await MData.MData.ListEntriesAsync(mdInfo)) {
        var entries = await MDataEntries.ForEachAsync(entriesHandle);
        Assert.AreEqual(1, entries.Count);
        var entry = entries.First();
        var key = await Info.DecryptAsync(mdInfo, entry.Item1);
        var value = await Info.DecryptAsync(mdInfo, entry.Item2);
        var encoding = new ASCIIEncoding();
        Assert.AreEqual(actKey, encoding.GetString(key.ToArray()));
        Assert.AreEqual(actValue, encoding.GetString(value.ToArray()));
      }
    }

    [Test]
    public async Task RandomPublicMutableDataInsertAction() {
      Utils.InitialiseSessionForRandomTestApp();
      const ulong tagType = 15001;
      var mdInfo = await Info.RandomPublicAsync(tagType);
      var permissionSet = new PermissionSet {Insert = true, ManagePermissions = true, Update = false, Read = true};
      using (var permissionsH = await MDataPermissions.NewAsync()) {
        using (var appSignKeyH = await Crypto.AppPubSignKeyAsync()) {
          await MDataPermissions.InsertAsync(permissionsH, appSignKeyH, permissionSet);
          await MData.MData.PutAsync(mdInfo, permissionsH, NativeHandle.Zero);
        }
      }

      using (var entryActionsH = await MDataEntryActions.NewAsync()) {
        var key = Encoding.Default.GetBytes("sample_key").ToList();
        var value = Encoding.Default.GetBytes("sample_value").ToList();
        await MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await MData.MData.MutateEntriesAsync(mdInfo, entryActionsH);
      }

      using (var entryActionsH = await MDataEntryActions.NewAsync()) {
        var key = Encoding.Default.GetBytes("sample_key_2").ToList();
        var value = Encoding.Default.GetBytes("sample_value_2").ToList();
        await MDataEntryActions.InsertAsync(entryActionsH, key, value);
        await MData.MData.MutateEntriesAsync(mdInfo, entryActionsH);
      }

      var keys = await MData.MData.ListKeysAsync(mdInfo);
      Assert.AreEqual(2, keys.Count);
    }
  }
}
