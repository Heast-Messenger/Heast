using System.Security.Cryptography;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Core.Tests;

[TestFixture]
public class TestEncryption
{
    [Test]
    public void Test()
    {
        var bytes = new byte[] { 16, 131, 14, 214, 58, 178, 242, 222, 249, 45, 131, 25, 164, 226, 39, 222 };

        using (var aes = Aes.Create())
        {
            using (var transform = aes.CreateDecryptor())
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.FlushFinalBlock();
                        var encrypted = memoryStream.ToArray();
                        Console.Out.WriteLine($"""
                                               plaintext: {JsonConvert.SerializeObject(bytes)}
                                               encrypted: {JsonConvert.SerializeObject(encrypted)}
                                               encrypted: {string.Join(", ", encrypted)}
                                               """);
                    }
                }
            }
        }
    }
}