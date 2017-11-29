using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CryptoTests {
    private const int EncKeySize = 32;
    private const int SignKeySize = 32;

    [Test]
    public async Task DataEncryption() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      using (var pubkey = encKeyPairTuple.Item1)
      using (var seckey = encKeyPairTuple.Item2) {
        var cipherBytes = await Crypto.EncryptAsync(plainBytes.ToList(), pubkey, seckey);
        var decryptedBytes = await Crypto.DecryptAsync(cipherBytes, pubkey, seckey);
        Assert.AreEqual(plainBytes, decryptedBytes);
      }
    }

    [Test]
    public async Task GenerateEncKeyPair() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      await Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
    }

    [Test]
    public async Task GenerateNonce() {
      var nonce = await Crypto.GenerateNonceAsync();
      Assert.NotNull(nonce);
    }

    [Test]
    public async Task GetAppPubSignKey() {
      Utils.InitialiseSessionForRandomTestApp();
      using (var handle = await Crypto.AppPubSignKeyAsync()) {
        Assert.NotNull(handle);
      }
    }

    [Test]
    public async Task GetPublicEncryptKey() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      var rawKey = await Crypto.EncPubKeyGetAsync(encKeyPairTuple.Item1);
      Assert.AreEqual(rawKey.Count, EncKeySize);
      var handle = await Crypto.EncPubKeyNewAsync(rawKey);
      Assert.NotNull(handle);
      rawKey = await Crypto.EncSecretKeyGetAsync(encKeyPairTuple.Item2);
      Assert.AreEqual(rawKey.Count, EncKeySize);
      handle = await Crypto.EncSecretKeyNewAsync(rawKey);
      Assert.NotNull(handle);
      await Crypto.EncPubKeyFreeAsync(encKeyPairTuple.Item1);
      await Crypto.EncSecretKeyFreeAsync(encKeyPairTuple.Item2);
    }

    [Test]
    public async Task GetSignKeys() {
      Utils.InitialiseSessionForRandomTestApp();
      var signKeyPairTuple = await Crypto.SignGenerateKeyPairAsync();
      Assert.NotNull(signKeyPairTuple.Item1);
      Assert.NotNull(signKeyPairTuple.Item2);
      var rawKey = await Crypto.SignPubKeyGetAsync(signKeyPairTuple.Item1);
      Assert.AreEqual(rawKey.Count, EncKeySize);
      var handle = await Crypto.SignPubKeyNewAsync(rawKey);
      Assert.NotNull(handle);
      rawKey = await Crypto.SignSecKeyGetAsync(signKeyPairTuple.Item2);
      Assert.AreEqual(rawKey.Count, EncKeySize);
      handle = await Crypto.SignSecKeyNewAsync(rawKey);
      Assert.NotNull(handle);
      await Crypto.SignPubKeyFreeAsync(signKeyPairTuple.Item1);
      await Crypto.SignSecKeyFreeAsync(signKeyPairTuple.Item2);
    }

    [Test]
    public async Task SealedBoxEncryption() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      Assert.NotNull(encKeyPairTuple.Item1);
      Assert.NotNull(encKeyPairTuple.Item2);
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      using (var pub = encKeyPairTuple.Item1)
      using (var sign = encKeyPairTuple.Item2) {
        var cipherBytes = await Crypto.EncryptSealedBoxAsync(plainBytes.ToList(), pub);
        var decryptedBytes = await Crypto.DecryptSealedBoxAsync(cipherBytes, pub, sign);
        Assert.AreEqual(plainBytes, decryptedBytes);
      }
    }

    [Test]
    public async Task Verify() {
      var signKeyPairTuple = await Crypto.SignGenerateKeyPairAsync();
      Assert.NotNull(signKeyPairTuple.Item1);
      Assert.NotNull(signKeyPairTuple.Item2);
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      var signeddata = await Crypto.SignAsync(plainBytes.ToList(), signKeyPairTuple.Item2);
      Assert.NotNull(signeddata);
      var verifiedBytes = await Crypto.VerifyAsync(signeddata, signKeyPairTuple.Item1);
      Assert.AreEqual(plainBytes, verifiedBytes);
    }
  }
}
