namespace Ionic.Zip
{
    using System;
    using System.IO;

    internal class ZipCipherStream : Stream
    {
        private ZipCrypto _cipher;
        private CryptoMode _mode;
        private Stream _s;

        public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
        {
            this._cipher = cipher;
            this._s = s;
            this._mode = mode;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._mode == CryptoMode.Encrypt)
            {
                throw new NotImplementedException();
            }
            byte[] buffer2 = new byte[count];
            int length = this._s.Read(buffer2, 0, count);
            byte[] buffer3 = this._cipher.DecryptMessage(buffer2, length);
            for (int i = 0; i < length; i++)
            {
                buffer[offset + i] = buffer3[i];
            }
            return length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this._mode == CryptoMode.Decrypt)
            {
                throw new NotImplementedException();
            }
            byte[] plaintext = null;
            if (offset != 0)
            {
                plaintext = new byte[count];
                for (int i = 0; i < count; i++)
                {
                    plaintext[i] = buffer[offset + i];
                }
            }
            else
            {
                plaintext = buffer;
            }
            byte[] buffer3 = this._cipher.EncryptMessage(plaintext, count);
            this._s.Write(buffer3, 0, buffer3.Length);
        }

        public override bool CanRead
        {
            get
            {
                return (this._mode == CryptoMode.Decrypt);
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return (this._mode == CryptoMode.Encrypt);
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

