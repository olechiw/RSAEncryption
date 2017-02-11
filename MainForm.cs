using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace RSA_Encryption
{
    public partial class MainForm : Form
    {
        private const int cProbablePrimeIterations = 1000;

        RSAParameters privKey;
        RSAParameters pubKey;

        // Constructor
        public MainForm()
        {
            InitializeComponent();

            
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);

            privKey = csp.ExportParameters(true);
            pubKey = csp.ExportParameters(false);

            csp.PersistKeyInCsp = false;

            csp = new RSACryptoServiceProvider();
        }

        // Convert a string to int safely
        private static int SafeConvertInt(string s)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(s);
            }
            catch (Exception) { }
            return i;
        }


        private void encryptButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(encryptionTextBox.Text.ToString()))
                return;
            string text = encryptionTextBox.Text.ToString();

            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(text));

            outputTextBox.Text = Convert.ToBase64String(encrypted).Replace("-", "");
        }

        private void decryptButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(decryptionTextBox.Text.ToString()))
                return;
            string text = decryptionTextBox.Text.ToString();

            byte[] textBytes = Convert.FromBase64String(text);

            byte[] decrypted = Decrypt(textBytes);

            outputTextBox.Text = Encoding.UTF8.GetString(decrypted);
        }

        private byte[] Encrypt(byte[] input)
        {
            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(pubKey);

                encrypted = rsa.Encrypt(input, true);
            }
            return encrypted;
        }

        private byte[] Decrypt(byte[] input)
        {

            byte[] decrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                rsa.ImportParameters(privKey);

                decrypted = rsa.Decrypt(input, true);
            }
            return decrypted;
        }
    }
}
