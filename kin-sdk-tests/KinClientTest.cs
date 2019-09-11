using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Kin.Sdk;
using Kin.Base;

namespace kin_sdk_tests
{
    [TestClass]
    public class KinClientTest
    {
        static TestingKeystoreProvider keystoreProvider = new TestingKeystoreProvider();
        static FakeHttpClient fakeClient = new FakeHttpClient();

        [TestInitialize]
        public void InitTest() => keystoreProvider.CleanStorage();

        [ClassCleanup]
        public static void CleanClass() => keystoreProvider.CleanStorage();


        [TestMethod]
        public void TestValidConstractor()
        {
            KinClient client = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider);
            KinClient client2 = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, "abcd");

            Assert.AreEqual(client.AppId, "anon");
            Assert.AreEqual(client2.AppId, "abcd");

            Assert.AreEqual(Kin.Base.Network.Current.NetworkPassphrase, client.Environment.NetworkPassphrase);
            
            new KinClient(Kin.Sdk.Environment.Production, keystoreProvider);
            Assert.AreEqual(Kin.Base.Network.Current.NetworkPassphrase, Kin.Sdk.Environment.Production.NetworkPassphrase);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidEnvironment()
        {
            new KinClient(null, keystoreProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidKeystore()
        {
            new KinClient(Kin.Sdk.Environment.Test, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "appId @@@@ is invalid, an appId can only contain letters and digits, and be 3 or 4 characters long")]
        public void TestInvalidAppId()
        {
            new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, "@@@@");
        }

        [TestMethod]
        public void TestExplicitlyEmptyAppId()
        {
            KinClient client = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, "");
            Assert.AreEqual(client.AppId, "");

            KinClient client2 = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, null);
            Assert.AreEqual(client2.AppId, null);
        }

        [TestMethod]
        public async Task TestAddAccount()
        {
            KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider);
            KeyPair keyPair = await kinClient.AddAccount();

            Assert.AreEqual(1, await keystoreProvider.GetAccountCount());
            Assert.AreEqual(keyPair.AccountId, (await keystoreProvider.GetAccountAtIndex(0)).AccountId);
        }

        [TestMethod]
        public async Task TestAddAccountWithExtras()
        {
            KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider);
            Dictionary<string, object> extras = new Dictionary<string, object>();
            extras.Add("PlayerName", "Ron");

            KeyPair keyPair = await kinClient.AddAccount(extras);

            Assert.AreEqual(keyPair.AccountId, keystoreProvider.GetAccountByPlayerName("Ron").AccountId);
        }

        [TestMethod]
        public void TestGetAccount()
        {
            KeyPair keyPair = KeyPair.Random();
            KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider);

            KinAccount kinAccount = kinClient.GetAccount(keyPair);
            Assert.AreEqual(kinAccount.KeyPair, keyPair);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullKeyPair()
        {
            KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider);
            kinClient.GetAccount(null);
        }
    }
}
