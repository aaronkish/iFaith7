namespace Ionic.Zip
{
    using System;
    using System.IO;

    internal class ZipCrypto
    {
        private uint[] _Keys = new uint[] { 0x12345678, 0x23456789, 0x34567890 };
        private CRC32 crc32 = new CRC32();

        private ZipCrypto()
        {
        }

        public byte[] DecryptMessage(byte[] cipherText, int length)
        {
            if (cipherText == null)
            {
                throw new ZipException("Cannot decrypt.", new ArgumentException("Bad length during Decryption: cipherText must be non-null.", "cipherText"));
            }
            if (length > cipherText.Length)
            {
                throw new ZipException("Cannot decrypt.", new ArgumentException("Bad length during Decryption: the length parameter must be smaller than or equal to the size of the destination array.", "length"));
            }
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                byte byeValue = (byte) (cipherText[i] ^ this.MagicByte);
                this.UpdateKeys(byeValue);
                buffer[i] = byeValue;
            }
            return buffer;
        }

        public byte[] EncryptMessage(byte[] plaintext, int length)
        {
            if (plaintext == null)
            {
                throw new ZipException("Cannot encrypt.", new ArgumentException("Bad length during Encryption: the plainText must be non-null.", "plaintext"));
            }
            if (length > plaintext.Length)
            {
                throw new ZipException("Cannot encrypt.", new ArgumentException("Bad length during Encryption: The length parameter must be smaller than or equal to the size of the destination array.", "length"));
            }
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                byte byeValue = plaintext[i];
                buffer[i] = (byte) (plaintext[i] ^ this.MagicByte);
                this.UpdateKeys(byeValue);
            }
            return buffer;
        }

        public static ZipCrypto ForRead(string password, ZipEntry e)
        {
            Stream s = e._archiveStream;
            e._WeakEncryptionHeader = new byte[12];
            byte[] buffer = e._WeakEncryptionHeader;
            ZipCrypto crypto = new ZipCrypto();
            if (password == null)
            {
                throw new BadPasswordException("This entry requires a password.");
            }
            crypto.InitCipher(password);
            ZipEntry.ReadWeakEncryptionHeader(s, buffer);
            byte[] buffer2 = crypto.DecryptMessage(buffer, buffer.Length);
            if (buffer2[11] != ((byte) ((e._Crc32 >> 0x18) & 0xff)))
            {
                if ((e._BitField & 8) != 8)
                {
                    throw new BadPasswordException("The password did not match.");
                }
                if (buffer2[11] != ((byte) ((e._TimeBlob >> 8) & 0xff)))
                {
                    throw new BadPasswordException("The password did not match.");
                }
            }
            return crypto;
        }

        public static ZipCrypto ForWrite(string password)
        {
            ZipCrypto crypto = new ZipCrypto();
            if (password == null)
            {
                throw new BadPasswordException("This entry requires a password.");
            }
            crypto.InitCipher(password);
            return crypto;
        }

        public void InitCipher(string passphrase)
        {
            byte[] buffer = SharedUtilities.StringToByteArray(passphrase);
            for (int i = 0; i < passphrase.Length; i++)
            {
                this.UpdateKeys(buffer[i]);
            }
        }

        private void UpdateKeys(byte byeValue)
        {
            this._Keys[0] = (uint) this.crc32.ComputeCrc32(this._Keys[0], byeValue);
            this._Keys[1] += (byte) this._Keys[0];
            this._Keys[1] = (this._Keys[1] * 0x8088405) + 1;
            this._Keys[2] = (uint) this.crc32.ComputeCrc32(this._Keys[2], (byte) (this._Keys[1] >> 0x18));
        }

        private byte MagicByte
        {
            get
            {
                ushort num = (ushort) (((ushort) (this._Keys[2] & 0xffff)) | 2);
                return (byte) ((num * (num ^ 1)) >> 8);
            }
        }
    }
}

