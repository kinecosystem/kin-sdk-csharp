using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using kin_sdk;

namespace kin_sdk_tests
{
    [TestClass]
    public class EnvironmentTest
    {
        [TestMethod]
        public void TestDefaults()
        {
            Assert.AreEqual(kin_sdk.Environment.Production.networkUrl, "https://horizon.kinfederation.com");
            Assert.AreEqual(kin_sdk.Environment.Production.networkPassphrase, "Kin Mainnet ; December 2018");

            Assert.AreEqual(kin_sdk.Environment.Test.networkUrl, "https://horizon-testnet.kininfrastructure.com");
            Assert.AreEqual(kin_sdk.Environment.Test.networkPassphrase, "Kin Testnet ; December 2018");
        }

        [TestMethod]
        public void TestValidConstractor()
        {
            new kin_sdk.Environment("http://example.com", "passhprase");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "networkUrl is not a valid URL")]
        public void TestNullUrl()
        {
            new kin_sdk.Environment(null, " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "networkUrl not_a_url is not a valid URL")]
        public void TestInvalidUrl()
        {
            new kin_sdk.Environment("not_a_url", " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "networkPassphrase cannot be null or empty")]
        public void TestInvalidPassphrase()
        {
            new kin_sdk.Environment("http://example.com", "");
        }

        [TestMethod]
        public void TestIsMainNet()
        {
            Assert.IsTrue(kin_sdk.Environment.Production.IsMainNet());
            Assert.IsFalse(kin_sdk.Environment.Test.IsMainNet());

            var wrongUrl = new kin_sdk.Environment("https://example.com", kin_sdk.Environment.Production.networkPassphrase);
            var wrongPass = new kin_sdk.Environment(kin_sdk.Environment.Production.networkUrl, " ");
            
            Assert.IsFalse(wrongUrl.IsMainNet());
            Assert.IsFalse(wrongPass.IsMainNet());
        }

        [TestMethod]
        public void TestGetNetwork()
        {
            kin_base.Network network  = kin_sdk.Environment.Test.GetNetwork();
            Assert.AreEqual(network.NetworkPassphrase , kin_sdk.Environment.Test.networkPassphrase);
        }
        
    }
}
