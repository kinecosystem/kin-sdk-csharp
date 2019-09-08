using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using kin_sdk;
using kin_base;

namespace kin_sdk_tests
{
    [TestClass]
    public class KinAccountTest
    {

        static TestingKeystoreProvider keystoreProvider = new TestingKeystoreProvider();
        static FakeHttpClient fakeClient = new FakeHttpClient();
        static KinClient kinClient = new KinClient(kin_sdk.Environment.Test, keystoreProvider, httpClient: fakeClient.httpClient);

        [TestMethod]
        public void TestGetPublicAddress()
        {
            KeyPair keyPair = KeyPair.Random();
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            Assert.AreEqual(kinAccount.GetPublicAddress, keyPair.Address);
        }

        [TestMethod]
        public async Task TestGetBalance()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountInfo.json"));
            fakeClient.SetResponse(jsonResponse);

            KeyPair keyPair = KeyPair.Random();
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            Decimal balance = await kinAccount.GetBalance();

            Assert.AreEqual(balance, Decimal.Parse("150.47"));
        }

        [TestMethod]
        [ExpectedException(typeof(AccountNotFoundException), "Account GAT36NRQTWB2TYWD3UM32HU7OS7DCYJIYGASXC4BXUGCADXJLO52WUEC was not found")]
        public async Task TestGetBalanceAccountNotFound()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountNotFound.json"));
            fakeClient.SetResponse(jsonResponse, HttpStatusCode.NotFound);

            KeyPair keyPair = KeyPair.FromAccountId("GAT36NRQTWB2TYWD3UM32HU7OS7DCYJIYGASXC4BXUGCADXJLO52WUEC");
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            await kinAccount.GetBalance();
        }

        [TestMethod]
        public async Task TestGetAccountStatus()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountInfo.json"));
            fakeClient.SetResponse(jsonResponse);

            KeyPair keyPair = KeyPair.Random();
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            AccountStatus status = await kinAccount.GetStatus();
            Assert.AreEqual(status, AccountStatus.Created);

            jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountNotFound.json"));
            fakeClient.SetResponse(jsonResponse, HttpStatusCode.NotFound);

            status = await kinAccount.GetStatus();
            Assert.AreEqual(status, AccountStatus.NotCreated);
        }
    }
}
