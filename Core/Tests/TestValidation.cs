using Core.Utility;
using NUnit.Framework;

namespace Core.Tests;

[TestFixture]
public class TestValidation
{
    [Test]
    public void Test()
    {
        {
            Validation.Split("localhost", out var host, out var port);
            Assert.IsTrue(host == "localhost" && port == null);
        }
        {
            Validation.Split("8080", out var host, out var port);
            Assert.IsTrue(host == null && port == 8080);
        }
        {
            Validation.Split("99999", out var host, out var port);
            Assert.IsTrue(host == "99999" && port == null);
        }
        {
            Validation.Split("127.0.0.1:23010", out var host, out var port);
            Assert.IsTrue(host == "127.0.0.1" && port == 23010);
        }
        {
            Validation.Split("goggle.com:1", out var host, out var port);
            Assert.IsTrue(host == "goggle.com" && port == 1);
        }
        {
            Validation.Split("nice.at:", out var host, out var port);
            Assert.IsTrue(host == "nice.at" && port == null);
        }

        {
            Assert.IsTrue(Validation.Validate("localhost", 23010, out _, out _, out _));
            Assert.IsTrue(Validation.Validate("127.0.0.1", 23010, out _, out _, out _));
            Assert.IsTrue(Validation.Validate("goggle.com", 23010, out _, out _, out _));
        }

        {
            Assert.IsFalse(Validation.Validate("localhost", 99999, out _, out _, out _));
            Assert.IsFalse(Validation.Validate("127", 23010, out _, out _, out _));
            Assert.IsFalse(Validation.Validate("goggle", 23010, out _, out _, out _));
            Assert.IsFalse(Validation.Validate("nice", 99999, out _, out _, out _));
        }
    }
}