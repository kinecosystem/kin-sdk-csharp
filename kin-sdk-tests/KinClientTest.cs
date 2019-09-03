using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using kin_sdk;
using kin_base;

namespace kin_sdk_tests
{
    [TestClass]
    public class KinClientTest
    {
        static TestingKeystoreProvider keystoreProvider = new TestingKeystoreProvider();

        [ClassInitialize]
        public static void InitClass(TestContext testContext) => keystoreProvider.CleanStorage();

        [ClassCleanup]
        public static void CleanClass() => keystoreProvider.CleanStorage();


        [TestMethod]
        public void TestValidConstractor()
        {
            KinClient client = new KinClient(kin_sdk.Environment.Test, keystoreProvider);
            KinClient client2 = new KinClient(kin_sdk.Environment.Test, keystoreProvider, "abcd");

            Assert.AreEqual(client.appId, "anon");
            Assert.AreEqual(client2.appId, "abcd");

            Assert.AreEqual(kin_base.Network.Current.NetworkPassphrase, client.environment.networkPassphrase);
            
            new KinClient(kin_sdk.Environment.Production, keystoreProvider);
            Assert.AreEqual(kin_base.Network.Current.NetworkPassphrase, kin_sdk.Environment.Production.networkPassphrase);
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
            new KinClient(kin_sdk.Environment.Test, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "appId @@@@ is invalid, an appId can only contain letters and digits, and be 3 or 4 characters long")]
        public void TestInvalidAppId()
        {
            new KinClient(kin_sdk.Environment.Test, keystoreProvider, "@@@@");
        }

        [TestMethod]
        public void TestExplicitlyEmptyAppId()
        {
            KinClient client = new KinClient(kin_sdk.Environment.Test, keystoreProvider, "");
            Assert.AreEqual(client.appId, "");
        }

        [TestMethod]
        public async Task TestAddAccount()
        {
            KinClient kinClient = new KinClient(kin_sdk.Environment.Test, keystoreProvider);
            KeyPair keyPair = await kinClient.AddAccount();

            Assert.AreEqual(1, await keystoreProvider.GetAccountCount());
            Assert.AreEqual(keyPair.AccountId, (await keystoreProvider.GetAccountAtIndex(0)).AccountId);
        }

        [TestMethod]
        public async Task TestAddAccountWithExtras()
        {
            KinClient kinClient = new KinClient(kin_sdk.Environment.Test, keystoreProvider);
            Dictionary<string, object> extras = new Dictionary<string, object>();
            extras.Add("PlayerName", "Ron");

            KeyPair keyPair = await kinClient.AddAccount(extras);

            Assert.AreEqual(keyPair.AccountId, keystoreProvider.GetAccountByPlayerName("Ron").AccountId);
        }
    }
}
