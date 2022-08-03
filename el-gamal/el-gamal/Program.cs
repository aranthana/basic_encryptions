using static System.Console;
using System;
namespace elgamal
{
    class Elgamal
    {
        public static void Main(string[] args)
        {
            ReadUserValues(out int message, out int baseValue,out double privateKey1,out double privateKey2);
           
            double publicKey1 = Math.Pow(baseValue, privateKey1);
            double publicKey2 = Math.Pow(baseValue, privateKey2);

            double encryptedMsg = EncryptMessage(message, publicKey2, privateKey1);
            WriteLine("Encrypted message :" + encryptedMsg);

            WriteLine("Decrypted message :" + Decrypt(encryptedMsg, publicKey1, privateKey2));
            

        }
         private static void ReadUserValues(out int message,out int baseValue, out double privateKey1, out double privateKey2)
        {
            Write("Enter a message:");
            // Read message from user.
            message = int.Parse(ReadLine()); 
            Write("Enter the base:");
            //Get base from user.
            baseValue = int.Parse(ReadLine()); 
            Write("Enter a private key 1:");
            //double private_key_1 = 2;// Need to change
            privateKey1 = double.Parse(ReadLine());
            Write("Enter a private key 2:");
            //double private_key_2 = 4; //Need to change
            privateKey2 = double.Parse(ReadLine());
        }
        // Encrypt the message using our private key and the builder's public key
        private static double EncryptMessage(int msg, double publicKey2, double privateKey1) 
        {
            double encryptedMsg = msg * Math.Pow(publicKey2, privateKey1);
            return encryptedMsg;
        }

        // Decrypt the message using builders private key and sender's public key
        private static double Decrypt(double encryptedMsg, double publicKey1, double privateKey2)
        {
            double decryptedMessage = encryptedMsg / Math.Pow(publicKey1, privateKey2);
            return decryptedMessage;
        }
    }
}
