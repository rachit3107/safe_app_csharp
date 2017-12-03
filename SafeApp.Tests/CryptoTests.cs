using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class CryptoTests {
    private const int EncKeySize = 32;
    private const int SignKeySize = 32;
    private const int NonceBytes = 32;

    /*[Test]
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
       using (var pub = encKeyPairTuple.Item1)
       using (var sec = encKeyPairTuple.Item2) {
         var rawKey = await Crypto.EncPubKeyGetAsync(pub);
         Assert.AreEqual(rawKey.Count, EncKeySize);
         var handle = await Crypto.EncPubKeyNewAsync(rawKey);
         Assert.NotNull(handle);
         rawKey = await Crypto.EncSecretKeyGetAsync(sec);
         Assert.AreEqual(rawKey.Count, EncKeySize);
         handle = await Crypto.EncSecretKeyNewAsync(rawKey);
         Assert.NotNull(handle);
       }
     }

     [Test]
     public async Task GetSignKeys() {
       Utils.InitialiseSessionForRandomTestApp();
       var signKeyPairTuple = await Crypto.SignGenerateKeyPairAsync();
       Assert.NotNull(signKeyPairTuple.Item1);
       Assert.NotNull(signKeyPairTuple.Item2);
       using (var pub = signKeyPairTuple.Item1)
       using (var sec = signKeyPairTuple.Item1) {
         var rawKey = await Crypto.SignPubKeyGetAsync(signKeyPairTuple.Item1);
         Assert.AreEqual(rawKey.Count, EncKeySize);
         var handle = await Crypto.SignPubKeyNewAsync(rawKey);
         Assert.NotNull(handle);
         rawKey = await Crypto.SignSecKeyGetAsync(signKeyPairTuple.Item2);
         Assert.AreEqual(rawKey.Count, EncKeySize);
         handle = await Crypto.SignSecKeyNewAsync(rawKey);
         Assert.NotNull(handle);
       }
     }*/

    /*[Test]
    public async Task SealedBoxEncryption() {
      Utils.InitialiseSessionForRandomTestApp();
      var encKeyPairTuple = await Crypto.EncGenerateKeyPairAsync();
      //Assert.NotNull(encKeyPairTuple.Item1);
      //Assert.NotNull(encKeyPairTuple.Item2);
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      using (var pub = encKeyPairTuple.Item1)
      using (var sec = encKeyPairTuple.Item2) {
        var rawKey = await Crypto.EncPubKeyGetAsync(pub);
        //Assert.AreEqual(rawKey.Count, EncKeySize);
        var handle = await Crypto.EncPubKeyNewAsync(rawKey);
        //Assert.NotNull(handle);
        var rawKey2 = await Crypto.EncSecretKeyGetAsync(sec);
        //Assert.AreEqual(rawKey.Count, EncKeySize);
        var handle2 = await Crypto.EncSecretKeyNewAsync(rawKey2);
        //Assert.NotNull(handle2);
        var cipherBytes = await Crypto.EncryptSealedBoxAsync(plainBytes.ToList(), handle);
        var decryptedBytes = await Crypto.DecryptSealedBoxAsync(cipherBytes, handle, handle2);
        Assert.AreEqual(plainBytes, decryptedBytes);

      }
    }

    [Test]
    public async Task VerifySignature() {
      var signKeyPairTuple = await Crypto.SignGenerateKeyPairAsync();
      Assert.NotNull(signKeyPairTuple.Item1);
      Assert.NotNull(signKeyPairTuple.Item2);
      var plainBytes = new byte[1024];
      new Random().NextBytes(plainBytes);
      using (var pub = signKeyPairTuple.Item1)
      using (var sec = signKeyPairTuple.Item2) {
      var rawKey = await Crypto.SignPubKeyGetAsync(signKeyPairTuple.Item1);
        Assert.AreEqual(rawKey.Count, EncKeySize);
        var handle = await Crypto.SignPubKeyNewAsync(rawKey);
        Assert.NotNull(handle);
        Console.WriteLine(handle);
        rawKey = await Crypto.SignSecKeyGetAsync(signKeyPairTuple.Item2);
        Assert.AreEqual(rawKey.Count, EncKeySize);
        var handle2 = await Crypto.SignSecKeyNewAsync(rawKey);
        Assert.NotNull(handle);
        Debug.Print(handle.ToString());
        //Assert.AreEqual(handle, pub);
        //var cipherBytes = await Crypto.SignAsync(plainBytes.ToList(), handle2);
        //var decryptedBytes = await Crypto.VerifyAsync(cipherBytes, handle);
        //Assert.AreEqual(plainBytes, decryptedBytes);
      }
    }
    /* [Test]
     public async Task VerifyNonce()
     {
       Utils.InitialiseSessionForRandomTestApp();
       var data = await Crypto.GenerateNonceAsync();
       Assert.AreEqual(data.Count, NonceBytes);
      }*/

    
  }

}

