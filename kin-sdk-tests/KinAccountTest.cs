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
    public class KinAccountTest
    {

        static TestingKeystoreProvider keystoreProvider = new TestingKeystoreProvider();
        static FakeHttpClient fakeClient = new FakeHttpClient();
        static KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, httpClient: fakeClient.httpClient);

        [TestMethod]
        public void TestGetPublicAddress()
        {
            KeyPair keyPair = KeyPair.Random();
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            Assert.AreEqual(kinAccount.PublicAddress, keyPair.Address);
        }

        [TestMethod]
        public async Task TestSendKin()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "txSuccess.json"));
            fakeClient.SetResponse(jsonResponse);

            KeyPair keyPair = KeyPair.Random();
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            string hash = await kinAccount.SendKin("GDEHCSVCMQZWYRUUOCXO6RP63FTUB3FMGFZWROXMCGUYTKNWOZOAGLDG", 150m, 100);
            Assert.AreEqual(hash, "8f1e0cd1d922f4c57cc1898ececcf47375e52ec4abf77a7e32d0d9bb4edecb69");
        }

        [TestMethod]
        [ExpectedException(typeof(OperationFailedException))]
        public async Task TestSendKinFail()
        {
                        string jsonResponse = File.ReadAllText(Path.Combine("testdata", "txUnderfunded.json"));
            fakeClient.SetResponse(jsonResponse);

            KeyPair keyPair = KeyPair.Random();
            KinAccount kinAccount = kinClient.GetAccount(keyPair);

            await kinAccount.SendKin("GDEHCSVCMQZWYRUUOCXO6RP63FTUB3FMGFZWROXMCGUYTKNWOZOAGLDG", 150m, 100);
        }
    }
}
