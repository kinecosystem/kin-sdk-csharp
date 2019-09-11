using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kin.Sdk;

namespace Kin.Sdk_tests
{
    [TestClass]
    public class EnvironmentTest
    {
        [TestMethod]
        public void TestDefaults()
        {
            Assert.AreEqual(Kin.Sdk.Environment.Production.NetworkUrl, "https://horizon.kinfederation.com");
            Assert.AreEqual(Kin.Sdk.Environment.Production.NetworkPassphrase, "Kin Mainnet ; December 2018");

            Assert.AreEqual(Kin.Sdk.Environment.Test.NetworkUrl, "https://horizon-testnet.kininfrastructure.com");
            Assert.AreEqual(Kin.Sdk.Environment.Test.NetworkPassphrase, "Kin Testnet ; December 2018");
        }

        [TestMethod]
        public void TestValidConstractor()
        {
            new Kin.Sdk.Environment("http://example.com", "passhprase");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "networkUrl is not a valid URL")]
        public void TestNullUrl()
        {
            new Kin.Sdk.Environment(null, " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "networkUrl not_a_url is not a valid URL")]
        public void TestInvalidUrl()
        {
            new Kin.Sdk.Environment("not_a_url", " ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "networkPassphrase cannot be null or empty")]
        public void TestInvalidPassphrase()
        {
            new Kin.Sdk.Environment("http://example.com", "");
        }

        [TestMethod]
        public void TestIsMainNet()
        {
            Assert.IsTrue(Kin.Sdk.Environment.Production.IsMainNet);
            Assert.IsFalse(Kin.Sdk.Environment.Test.IsMainNet);

            var wrongUrl = new Kin.Sdk.Environment("https://example.com", Kin.Sdk.Environment.Production.NetworkPassphrase);
            var wrongPass = new Kin.Sdk.Environment(Kin.Sdk.Environment.Production.NetworkUrl, " ");
            
            Assert.IsFalse(wrongUrl.IsMainNet);
            Assert.IsFalse(wrongPass.IsMainNet);
        }

        [TestMethod]
        public void TestGetNetwork()
        {
            Kin.Base.Network network  = Kin.Sdk.Environment.Test.Network;
            Assert.AreEqual(network.NetworkPassphrase , Kin.Sdk.Environment.Test.NetworkPassphrase);
        }
        
    }
}
