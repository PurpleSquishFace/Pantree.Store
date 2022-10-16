using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Pantree.Helpers
{
    /// <summary>
    /// Helper class providing functionality to hash passwords, validate hashes and verify passwords. <br/>
    /// Can be used as a static class, or an instanctiated object, adding the ability to customize properties.
    /// </summary>
    public class SecurePassword
    {
        /// <summary>
        /// Tag used to identify the hashed string
        /// </summary>
        private const string Identifier = "$AnHa|v1$";

        /// <summary>
        /// The default size of the salt
        /// </summary>
        private const int DefaultSaltSize = 16;
        /// <summary>
        /// The default size of the Hash
        /// </summary>
        private const int DefaultHashSize = 20;
        /// <summary>
        /// The default number of iterations
        /// </summary>
        private const int DefaultIterations = 10000;

        /// <summary>
        /// The manually set size of the salt
        /// </summary>
        public int SaltSize { get; private set; }
        /// <summary>
        /// The manually set size of the Hash
        /// </summary>
        public int HashSize { get; private set; }
        /// <summary>
        /// The manually set number of iterations
        /// </summary>
        public int Iterations { get; private set; }

        /// <summary>
        /// Provides enumerated values to provide or set the strength of a password
        /// </summary>
        public enum PasswordStrength
        {
            /// <summary>
            /// No password is provided
            /// </summary>
            Blank = 0,
            /// <summary>
            /// Password does not meet the minimum required length
            /// </summary>
            TooShort = 1,
            /// <summary>
            /// Passwork is very weak
            /// </summary>
            VeryWeak = 2,
            /// <summary>
            /// Password is weak
            /// </summary>
            Weak = 3,
            /// <summary>
            /// Password is medium strength
            /// </summary>
            Medium = 4,
            /// <summary>
            /// Password is strong
            /// </summary>
            Strong = 5,
            /// <summary>
            /// Password is very strong
            /// </summary>
            VeryStrong = 6
        };

        /// <summary>
        /// Default minimum length for a password
        /// </summary>
        private const int DefaultMinimumPasswordLength = 4;
        /// <summary>
        /// Default target length for a password
        /// </summary>
        private const int DefaultTargetPasswordLength = 12;

        /// <summary>
        /// The manually set minimum length for a password
        /// </summary>
        public int MinimumPasswordLength { get; private set; }
        /// <summary>
        /// The manually set target length for a password
        /// </summary>
        public int TargetPasswordLength { get; private set; }

        /// <summary>
        /// Instantiates the SecurePassword class, using default values
        /// </summary>
        public SecurePassword()
        {
            SaltSize = DefaultSaltSize;
            HashSize = DefaultHashSize;
            Iterations = DefaultIterations;
            MinimumPasswordLength = DefaultMinimumPasswordLength;
            TargetPasswordLength = DefaultTargetPasswordLength;
        }

        /// <summary>
        /// Instantiates the SecurePassword class, using passed in values for hashing
        /// </summary>
        /// <param name="saltSize">The size of the salt</param>
        /// <param name="hashSize">The size of the hash</param>
        /// <param name="iterations">The number of iterations</param>
        public SecurePassword(int saltSize, int hashSize, int iterations)
        {
            SaltSize = saltSize;
            HashSize = hashSize;
            Iterations = iterations;
            MinimumPasswordLength = DefaultMinimumPasswordLength;
            TargetPasswordLength = DefaultTargetPasswordLength;
        }

        /// <summary>
        /// Instantiates the SecurePassword class, using passed in values for password testing
        /// </summary>
        /// <param name="minimumPasswordLength">The minimum length for a password</param>
        /// <param name="targetPasswordLength">The target length for a password</param>
        public SecurePassword(int minimumPasswordLength, int targetPasswordLength)
        {
            MinimumPasswordLength = minimumPasswordLength;
            TargetPasswordLength = targetPasswordLength;
            SaltSize = DefaultSaltSize;
            HashSize = DefaultHashSize;
            Iterations = DefaultIterations;
        }

        /// <summary>
        /// Instantiates the SecurePassword class, using passed in values for both hashing and password testing
        /// </summary>
        /// <param name="saltSize">The size of the salt</param>
        /// <param name="hashSize">The size of the hash</param>
        /// <param name="iterations">The number of iterations</param>
        /// <param name="minimumPasswordLength">The minimum length for a password</param>
        /// <param name="targetPasswordLength">The target length for a password</param>
        public SecurePassword(int saltSize, int hashSize, int iterations, int minimumPasswordLength, int targetPasswordLength)
        {
            SaltSize = saltSize;
            HashSize = hashSize;
            Iterations = iterations;
            MinimumPasswordLength = minimumPasswordLength;
            TargetPasswordLength = targetPasswordLength;
        }

        /// <summary>
        /// Creates a hashed string from a password
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>The hashed password string</returns>
        public string HashPassword(string password)
        {
            return Hash(password, Iterations, SaltSize, HashSize);
        }

        /// <summary>
        /// Checks if string is a valid hash
        /// </summary>
        /// <param name="hashString">The hashed string</param>
        /// <returns>Whether string is validated or not</returns>
        public bool ValidHashString(string hashString)
        {
            return ValidHash(hashString);
        }

        /// <summary>
        /// Verifies a password against a hashed string
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="hashString">The hashed string</param>
        /// <returns>Whether password could be verified</returns>
        public bool VerifyPassword(string password, string hashString)
        {
            return Verify(password, hashString, SaltSize, HashSize);
        }

        /// <summary>
        /// Creates a hashed string from a password, with a default of 10000 iterations
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>The hashed password string</returns>
        public static string Hash(string password)
        {
            return Hash(password, DefaultIterations, DefaultSaltSize, DefaultHashSize);
        }

        /// <summary>
        /// Creates a hashed string from a password
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="iterations">The number of iterations</param>
        /// <returns>The hashed password string</returns>
        private static string Hash(string password, int iterations, int saltSize, int hashSize)
        {
            using var rng = new RNGCryptoServiceProvider();
            byte[] salt;
            rng.GetBytes(salt = new byte[saltSize]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);

            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            var generatedHash = Convert.ToBase64String(hashBytes);
            return $"{Identifier}{iterations}${generatedHash}";
        }

        /// <summary>
        /// Checks if string is a valid hash
        /// </summary>
        /// <param name="hashString">The hashed string</param>
        /// <returns>Whether string is validated or not</returns>
        public static bool ValidHash(string hashString)
        {
            return hashString.Contains(Identifier);
        }

        /// <summary>
        /// Verifies a password against a hashed string
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="hashString">The hashed string</param>
        /// <returns>Whether password could be verified</returns>
        public static bool Verify(string password, string hashString)
        {
            return Verify(password, hashString, DefaultSaltSize, DefaultHashSize);
        }

        /// <summary>
        /// Verifies a password against a hashed string
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="hashString">The hashed string</param>
        /// <param name="saltSize">The size of the salt</param>
        /// <param name="hashSize">The size of the hash</param>
        /// <returns>Whether password could be verified</returns>
        private static bool Verify(string password, string hashString, int saltSize, int hashSize)
        {
            if (!ValidHash(hashString)) return false;

            var splitString = hashString.Replace(Identifier, "").Split('$');
            var iterations = int.Parse(splitString[0]);
            var base64HashString = splitString[1];

            var hashedBytes = Convert.FromBase64String(base64HashString);

            var salt = new byte[saltSize];
            Array.Copy(hashedBytes, 0, salt, 0, saltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(hashSize);

            for (var i = 0; i < hashSize; i++)
                if (hashedBytes[i + saltSize] != hash[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Checks the strength of a password
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>Enum value representing the strength of the provided password</returns>
        public PasswordStrength CheckPasswordStrength(string password)
        {
            return CheckStrength(password, MinimumPasswordLength, TargetPasswordLength);
        }

        /// <summary>
        /// Checks if the provided password is valid
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>Whether the password is valid</returns>
        public bool CheckValidPassword(string password)
        {
            return ValidPassword(password);
        }

        /// <summary>
        /// Checks the strength of a password
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>Enum value representing the strength of the provided password</returns>
        public static PasswordStrength CheckStrength(string password)
        {
            return CheckStrength(password, DefaultMinimumPasswordLength, DefaultTargetPasswordLength);
        }

        /// <summary>
        /// Checks the strength of a password
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="minimumLength">The minimum length of a valid password</param>
        /// <param name="targetLength">The target length of the password</param>
        /// <returns>Enum value representing the strength of the provided password</returns>
        private static PasswordStrength CheckStrength(string password, int minimumLength, int targetLength)
        {
            var score = 0;

            if (password.Length < 1) return PasswordStrength.Blank;
            if (password.Length < minimumLength) return PasswordStrength.TooShort;

            var lowerBoundLength = (targetLength - minimumLength) / 4;
            if (password.Length < minimumLength + lowerBoundLength) return PasswordStrength.VeryWeak;

            if (password.Length >= minimumLength + (2 * lowerBoundLength))
                score++;
            if (password.Length >= minimumLength + (3 * lowerBoundLength))
                score++;
            if (password.Length >= targetLength)
                score++;

            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordStrength)score;
        }

        /// <summary>
        /// Checks if the provided password is valid
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>Whether the password is valid</returns>
        public static bool ValidPassword(string password)
        {
            var strength = CheckStrength(password);
            return (int)strength >= 2;
        }
    }
}