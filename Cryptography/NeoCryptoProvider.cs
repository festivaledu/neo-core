using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Neo.Core.Cryptography
{
    public sealed class NeoCryptoProvider
    {
        public static NeoCryptoProvider Instance => instance.Value;

        private static readonly Lazy<NeoCryptoProvider> instance = new Lazy<NeoCryptoProvider>(() => new NeoCryptoProvider());

        private readonly Aes aes = Aes.Create();
        private readonly RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        private readonly SHA512Managed sha = new SHA512Managed();

        private NeoCryptoProvider() { }
        
        ~NeoCryptoProvider() {
            aes.Dispose();
            rsa.Dispose();
            sha.Dispose();
        }

        public async Task<string> AesDecryptAsync(string s, CryptographicData data) {
            var transform = aes.CreateDecryptor(data.AesKey, data.AesIV);
        public async Task<string> AesDecryptAsync(string s, AesParameters parameters) {
            var transform = aes.CreateDecryptor(parameters.AesKey, parameters.AesIV);
            var memoryStream = new MemoryStream(Convert.FromBase64String(s));
            var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            var decrypted = await reader.ReadToEndAsync();
            reader.Dispose();
            cryptoStream.Dispose();
            memoryStream.Dispose();
            transform.Dispose();

            return decrypted;
        }

        public async Task<string> AesEncryptAsync(string s, CryptographicData data) {
            var transform = aes.CreateEncryptor(data.AesKey, data.AesIV);
        public async Task<string> AesEncryptAsync(string s, AesParameters parameters) {
            var transform = aes.CreateEncryptor(parameters.AesKey, parameters.AesIV);
            var memoryStream = new MemoryStream();

            using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write)) {
                using (var writer = new StreamWriter(cryptoStream)) {
                    await writer.WriteAsync(s);
                }
            }

            var encrypted = Convert.ToBase64String(memoryStream.ToArray());
            memoryStream.Dispose();
            transform.Dispose();

            return encrypted;
        }

        public CryptographicData GetRandomData() {
        public AesParameters GetRandomData() {
            aes.GenerateKey();
            aes.GenerateIV();

            return new AesParameters(aes.Key, aes.IV);
        }

        public string RsaDecrypt(string s, RSAParameters @params) {
            rsa.ImportParameters(@params);
        public string RsaDecrypt(string s, RSAParameters parameters) {
            rsa.ImportParameters(parameters);
            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(s), RSAEncryptionPadding.Pkcs1));
        }

        public string RsaEncrypt(string s, RSAParameters @params) {
            rsa.ImportParameters(@params);
        public string RsaEncrypt(string s, RSAParameters parameters) {
            rsa.ImportParameters(parameters);
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(s), RSAEncryptionPadding.Pkcs1));
        }

        public byte[] Sha512ComputeHash(string s) => sha.ComputeHash(Encoding.UTF8.GetBytes(s));
    }
}
