namespace Ionic.Zip
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    internal class WinZipAesCrypto
    {
        private bool _cryptoGenerated;
        internal byte[] _generatedPv;
        private byte[] _keyBytes;
        internal int _KeyStrengthInBits;
        private byte[] _MacInitializationVector;
        private string _Password;
        internal byte[] _providedPv;
        internal byte[] _Salt;
        private byte[] _StoredMac;
        public byte[] CalculatedMac;
        private short PasswordVerificationGenerated;
        private short PasswordVerificationStored;
        private int Rfc2898KeygenIterations;

        private WinZipAesCrypto()
        {
            this.Rfc2898KeygenIterations = 0x3e8;
            this._cryptoGenerated = false;
        }

        private WinZipAesCrypto(string password, int KeyStrengthInBits)
        {
            this.Rfc2898KeygenIterations = 0x3e8;
            this._cryptoGenerated = false;
            this._Password = password;
            this._KeyStrengthInBits = KeyStrengthInBits;
        }

        private void _GenerateCryptoBytes()
        {
            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(this._Password, this.Salt, this.Rfc2898KeygenIterations);
            this._keyBytes = bytes.GetBytes(this._KeyStrengthInBytes);
            this._MacInitializationVector = bytes.GetBytes(this._KeyStrengthInBytes);
            this._generatedPv = bytes.GetBytes(2);
            this._cryptoGenerated = true;
        }

        public static WinZipAesCrypto Generate(string password, int KeyStrengthInBits)
        {
            WinZipAesCrypto crypto = new WinZipAesCrypto(password, KeyStrengthInBits);
            int num = crypto._KeyStrengthInBytes / 2;
            crypto._Salt = new byte[num];
            new Random().NextBytes(crypto._Salt);
            return crypto;
        }

        public void ReadAndVerifyMac(Stream s)
        {
            bool flag = false;
            long position = s.Position;
            this._StoredMac = new byte[10];
            int num2 = s.Read(this._StoredMac, 0, this._StoredMac.Length);
            if (this._StoredMac.Length != this.CalculatedMac.Length)
            {
                flag = true;
            }
            if (!flag)
            {
                for (int i = 0; i < this._StoredMac.Length; i++)
                {
                    if (this._StoredMac[i] != this.CalculatedMac[i])
                    {
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                throw new Exception(string.Format("The MAC does not match '{0}' != '{1}'", Util.FormatByteArray(this._StoredMac), Util.FormatByteArray(this.CalculatedMac)));
            }
        }

        public static WinZipAesCrypto ReadFromStream(string password, int KeyStrengthInBits, Stream s)
        {
            WinZipAesCrypto crypto = new WinZipAesCrypto(password, KeyStrengthInBits);
            int num = crypto._KeyStrengthInBytes / 2;
            crypto._Salt = new byte[num];
            crypto._providedPv = new byte[2];
            int num2 = s.Read(crypto._Salt, 0, crypto._Salt.Length);
            num2 = s.Read(crypto._providedPv, 0, crypto._providedPv.Length);
            crypto.PasswordVerificationStored = (short) (crypto._providedPv[0] + (crypto._providedPv[1] * 0x100));
            if (password != null)
            {
                crypto.PasswordVerificationGenerated = (short) (crypto.GeneratedPV[0] + (crypto.GeneratedPV[1] * 0x100));
                if (crypto.PasswordVerificationGenerated != crypto.PasswordVerificationStored)
                {
                    throw new Exception("bad password");
                }
            }
            return crypto;
        }

        private int _KeyStrengthInBytes
        {
            get
            {
                return (this._KeyStrengthInBits / 8);
            }
        }

        public byte[] GeneratedPV
        {
            get
            {
                if (!this._cryptoGenerated)
                {
                    this._GenerateCryptoBytes();
                }
                return this._generatedPv;
            }
        }

        public byte[] KeyBytes
        {
            get
            {
                if (!this._cryptoGenerated)
                {
                    this._GenerateCryptoBytes();
                }
                return this._keyBytes;
            }
        }

        public byte[] MacIv
        {
            get
            {
                if (!this._cryptoGenerated)
                {
                    this._GenerateCryptoBytes();
                }
                return this._MacInitializationVector;
            }
        }

        public string Password
        {
            set
            {
                this._Password = value;
                if (this._Password != null)
                {
                    this.PasswordVerificationGenerated = (short) (this.GeneratedPV[0] + (this.GeneratedPV[1] * 0x100));
                    if (this.PasswordVerificationGenerated != this.PasswordVerificationStored)
                    {
                        throw new Exception("bad password");
                    }
                }
            }
        }

        public byte[] Salt
        {
            get
            {
                return this._Salt;
            }
        }

        public int SizeOfEncryptionMetadata
        {
            get
            {
                return (((this._KeyStrengthInBytes / 2) + 10) + 2);
            }
        }

        public byte[] StoredMac
        {
            get
            {
                return this._StoredMac;
            }
        }
    }
}

