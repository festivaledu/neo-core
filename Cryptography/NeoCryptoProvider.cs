using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Neo.Core.Cryptography
{
    /// <summary>
    ///     Provides methods for AES and RSA en/decryption and SHA hashing.
    ///     <para>
    ///         This class uses a thread-safe singleton lazy pattern and can only be accessed through the <see cref="Instance"/> property.
    ///     </para>
    /// </summary>
    public sealed class NeoCryptoProvider
    {
        /// <summary>
        ///     Returns the only instance of the <see cref="NeoCryptoProvider"/>.
        /// </summary>
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

        /// <summary>
        ///     Decrypts a string asynchronously using the <see cref="Aes"/> algorithm.
        /// </summary>
        /// <param name="s">The string to decrypt.</param>
        /// <param name="parameters">The <see cref="AesParameters"/> structure holding the key and initialization vector.</param>
        /// <returns>Returns the decrypted string.</returns>
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

        /// <summary>
        ///     Encrypts a string asynchronously using the <see cref="Aes"/> algorithm.
        /// </summary>
        /// <param name="s">The string to encrypt.</param>
        /// <param name="parameters">The <see cref="AesParameters"/> structure holding the key and initialization vector.</param>
        /// <returns>Returns the encrypted string.</returns>
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

        /// <summary>
        ///     Generates a <see cref="AesParameters"/> structure with a random key and initialization vector.
        /// </summary>
        /// <returns>Returns the generated structure.</returns>
        public AesParameters GetRandomData() {
            aes.GenerateKey();
            aes.GenerateIV();

            return new AesParameters(aes.Key, aes.IV);
        }

        /// <summary>
        ///     Decrypts a string using the <see cref="RSA"/> algorithm.
        /// </summary>
        /// <param name="s">The string to decrypt.</param>
        /// <param name="parameters">The <see cref="RSAParameters"/> structure holding the private key.</param>
        /// <returns>Returns the decrypted string.</returns>
        public string RsaDecrypt(string s, RSAParameters parameters) {
            rsa.ImportParameters(parameters);
            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(s), RSAEncryptionPadding.Pkcs1));
        }

        /// <summary>
        ///     Encrypts a string using the <see cref="RSA"/> algorithm.
        /// </summary>
        /// <param name="s">The string to encrypt.</param>
        /// <param name="parameters">The <see cref="RSAParameters"/> structure holding the public or private key.</param>
        /// <returns>Returns the encrypted string.</returns>
        public string RsaEncrypt(string s, RSAParameters parameters) {
            rsa.ImportParameters(parameters);
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(s), RSAEncryptionPadding.Pkcs1));
        }

        /// <summary>
        ///     Computes a hash value using the <see cref="SHA512"/> algorithm.
        /// </summary>
        /// <param name="s">The string to compose the hash value of.</param>
        /// <returns>Returns the computed hash value.</returns>
        public byte[] Sha512ComputeHash(string s) => sha.ComputeHash(Encoding.UTF8.GetBytes(s));
    }
}
