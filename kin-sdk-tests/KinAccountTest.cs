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
    }
}
