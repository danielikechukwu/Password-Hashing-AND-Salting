using System.Security.Cryptography;
using System.Text;

namespace PasswordHashingImplementation.Models
{
    // Provides methods for creating and verifying password hashes using HMACSHA512.
    public class PasswordHasher
    {
        // Creates a hashed version of the provided password along with a unique salt.
        // password: The plaintext password to be hashed.
        // passwordHash: Outputs the resulting password hash as a byte array.
        // passwordSalt: Outputs the unique salt used in hashing as a byte array.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Instantiate HMACSHA512 to generate a cryptographic hash and a unique key (salt).
            using (var hmac = new HMACSHA512())
            {
                // The Key property of HMACSHA512 provides a randomly generated salt.
                passwordSalt = hmac.Key; // Assign the generated salt to the output parameter.

                // Convert the plaintext password into a byte array using UTF-8 encoding.
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash of the password bytes using the HMACSHA512 instance.
                passwordHash = hmac.ComputeHash(passwordBytes); // Assign the computed hash to the output parameter.

            }
        }

        // Verifies whether the provided password matches the stored hash using the stored salt.
        // password: The plaintext password to verify.
        // storedHash: The stored password hash to compare against.
        // storedSalt: The stored salt used during the original hashing process.
        // Return: True if the password is valid and matches the stored hash; otherwise, false.
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            // Instantiate HMACSHA512 with the stored salt as the key to ensure the same hashing parameters.
            using (var hmac = new HMACSHA512(storedSalt))
            {
                // Convert the plaintext password into a byte array using UTF-8 encoding.
                byte[] passwordByte = Encoding.UTF8.GetBytes(password);

                // Compute the hash of the password bytes using the HMACSHA512 instance initialized with the stored salt.
                byte[] computedHash = hmac.ComputeHash(passwordByte);

                // Compare the computed hash with the stored hash byte by byte.
                // SequenceEqual ensures that both byte arrays are identical in sequence and value.
                bool hashesMatch = computedHash.SequenceEqual(storedHash);

                // Return the result of the comparison.
                return hashesMatch; 
            }
        }
    }
}
