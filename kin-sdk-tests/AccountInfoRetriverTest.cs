using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Kin.Sdk;
using Kin.Base;

namespace kin_sdk_tests
{
    [TestClass]
    public class AccountInfoRetriverTest
    {

        static TestingKeystoreProvider keystoreProvider = new TestingKeystoreProvider();
        static FakeHttpClient fakeClient = new FakeHttpClient();
        static KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, httpClient: fakeClient.httpClient);

        static AccountInfoRetriver accountInfoRetriver = kinClient.accountInfoRetriver;

        const string AccountId = "GAT36NRQTWB2TYWD3UM32HU7OS7DCYJIYGASXC4BXUGCADXJLO52WUEC";

        [TestMethod]
        public async Task TestGetBalance()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountInfo.json"));
            fakeClient.SetResponse(jsonResponse);

            KeyPair keyPair = KeyPair.Random();
            Decimal balance = await accountInfoRetriver.GetBalance(keyPair.AccountId);

            Assert.AreEqual(balance, Decimal.Parse("150.47"));
        }

        [TestMethod]
        [ExpectedException(typeof(AccountNotFoundException), "Account GAT36NRQTWB2TYWD3UM32HU7OS7DCYJIYGASXC4BXUGCADXJLO52WUEC was not found")]
        public async Task TestGetBalanceAccountNotFound()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountNotFound.json"));
            fakeClient.SetResponse(jsonResponse, HttpStatusCode.NotFound);

            await accountInfoRetriver.GetBalance(AccountId);
        }

        [TestMethod]
        [ExpectedException(typeof(OperationFailedException))]
        public async Task TestGetBalanceIOError()
        {
            fakeClient.SetResponse(new HttpRequestException());
            
            await accountInfoRetriver.GetBalance(AccountId);
        }

        [TestMethod]
        public async Task TestGetAccountStatus()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountInfo.json"));
            fakeClient.SetResponse(jsonResponse);

            AccountStatus status = await accountInfoRetriver.GetStatus(AccountId);
            Assert.AreEqual(status, AccountStatus.Created);

            jsonResponse = File.ReadAllText(Path.Combine("testdata", "accountNotFound.json"));
            fakeClient.SetResponse(jsonResponse, HttpStatusCode.NotFound);

            status = await accountInfoRetriver.GetStatus(AccountId);
            Assert.AreEqual(status, AccountStatus.NotCreated);
        }
    }
}
