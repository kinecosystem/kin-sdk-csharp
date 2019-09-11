using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Kin.Sdk;
using Kin.Base;

namespace Kin.Sdk_tests
{
    [TestClass]
    public class GeneralBlockchainInfoRetriverTest
    {

        static TestingKeystoreProvider keystoreProvider = new TestingKeystoreProvider();
        static FakeHttpClient fakeClient = new FakeHttpClient();
        static KinClient kinClient = new KinClient(Kin.Sdk.Environment.Test, keystoreProvider, httpClient: fakeClient.httpClient);

        static GeneralBlockchainInfoRetriever blockchainInfoRetriever = kinClient.generalBlockchainInfoRetriever;

        [TestMethod]
        public async Task TestGetMinimumFee()
        {
            string jsonResponse = File.ReadAllText(Path.Combine("testdata", "minimumFee.json"));
            fakeClient.SetResponse(jsonResponse);
            
            UInt32 minimumFee = await blockchainInfoRetriever.GetMinimumFee();
            Assert.AreEqual(minimumFee, (UInt32) 100);   
        }

        [TestMethod]
        [ExpectedException(typeof(OperationFailedException))]
        public async Task TestGetMinimumFeeIOError()
        {
            fakeClient.SetResponse(new HttpRequestException());
            
            await blockchainInfoRetriever.GetMinimumFee();
        }
    }
}
