namespace Ionic.Zip
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    internal class WinZipAesCipherStream : Stream
    {
        internal RijndaelManaged _aesCipher;
        private bool _finalBlock;
        private long _length;
        internal HMACSHA1 _mac;
        private CryptoMode _mode;
        private bool _NextXformWillBeFinal;
        private int _nonce;
        private WinZipAesCrypto _params;
        private int _pendingCount;
        private byte[] _PendingWriteBuffer;
        private Stream _s;
        private long _totalBytesXferred;
        internal ICryptoTransform _xform;
        private const int BLOCK_SIZE_IN_BYTES = 0x10;
        private byte[] counter;
        private byte[] counterOut;

        internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, CryptoMode mode)
        {
            this._finalBlock = false;
            this._NextXformWillBeFinal = false;
            this.counter = new byte[0x10];
            this.counterOut = new byte[0x10];
            this._totalBytesXferred = 0L;
            this._pendingCount = 0;
            this._params = cryptoParams;
            this._s = s;
            this._mode = mode;
            this._nonce = 1;
            int num = this._params.KeyBytes.Length * 8;
            if (((num != 0x100) && (num != 0x80)) && (num != 0xc0))
            {
                throw new Exception("Invalid key size");
            }
            this._mac = new HMACSHA1(this._params.MacIv);
            this._aesCipher = new RijndaelManaged();
            this._aesCipher.BlockSize = 0x80;
            this._aesCipher.KeySize = num;
            this._aesCipher.Mode = CipherMode.ECB;
            this._aesCipher.Padding = PaddingMode.None;
            byte[] rgbIV = new byte[0x10];
            this._xform = this._aesCipher.CreateEncryptor(this._params.KeyBytes, rgbIV);
            if (this._mode == CryptoMode.Encrypt)
            {
                this._PendingWriteBuffer = new byte[0x10];
            }
        }

        internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, long length, CryptoMode mode) : this(s, cryptoParams, mode)
        {
            this._length = length;
        }

        public override void Close()
        {
            if (this._pendingCount != 0)
            {
                this._NextXformWillBeFinal = true;
                this.ProcessOneBlockWriting(this._PendingWriteBuffer, 0, this._pendingCount);
                this._s.Write(this._PendingWriteBuffer, 0, this._pendingCount);
                this._totalBytesXferred += this._pendingCount;
            }
            this._s.Close();
        }

        public override void Flush()
        {
            this._s.Flush();
        }

        public void NotifyFinal()
        {
        }

        private int ProcessOneBlockReading(byte[] buffer, int offset, int count)
        {
            if (this._finalBlock)
            {
                throw new NotSupportedException();
            }
            int num = count - offset;
            int inputCount = (num > 0x10) ? 0x10 : num;
            if ((this._length > 0L) && (((this._totalBytesXferred + count) == this._length) && (inputCount == num)))
            {
                this._NextXformWillBeFinal = true;
            }
            Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
            if (this._NextXformWillBeFinal && (inputCount == (count - offset)))
            {
                this._mac.TransformFinalBlock(buffer, offset, inputCount);
                this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 0x10);
                this._finalBlock = true;
            }
            else
            {
                this._mac.TransformBlock(buffer, offset, inputCount, null, 0);
                this._xform.TransformBlock(this.counter, 0, 0x10, this.counterOut, 0);
            }
            for (int i = 0; i < inputCount; i++)
            {
                buffer[offset + i] = (byte) (this.counterOut[i] ^ buffer[offset + i]);
            }
            return inputCount;
        }

        private int ProcessOneBlockWriting(byte[] buffer, int offset, int last)
        {
            if (this._finalBlock)
            {
                throw new Exception("The final block has already been transformed.");
            }
            int num = last - offset;
            int length = (num > 0x10) ? 0x10 : num;
            Array.Copy(BitConverter.GetBytes(this._nonce++), 0, this.counter, 0, 4);
            if (length == (last - offset))
            {
                if (this._NextXformWillBeFinal)
                {
                    this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 0x10);
                    this._finalBlock = true;
                }
                else if ((buffer != this._PendingWriteBuffer) || (length != 0x10))
                {
                    Array.Copy(buffer, offset, this._PendingWriteBuffer, this._pendingCount, length);
                    this._pendingCount += length;
                    this._nonce--;
                    return 0;
                }
            }
            if (!this._finalBlock)
            {
                this._xform.TransformBlock(this.counter, 0, 0x10, this.counterOut, 0);
            }
            for (int i = 0; i < length; i++)
            {
                buffer[offset + i] = (byte) (this.counterOut[i] ^ buffer[offset + i]);
            }
            if (this._finalBlock)
            {
                this._mac.TransformFinalBlock(buffer, offset, length);
            }
            else
            {
                this._mac.TransformBlock(buffer, offset, length, null, 0);
            }
            return length;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this._mode == CryptoMode.Encrypt)
            {
                throw new NotSupportedException();
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (count < 0))
            {
                throw new ArgumentException("Invalid parameters");
            }
            if (buffer.Length < (offset + count))
            {
                throw new ArgumentException("The buffer is too small");
            }
            int num = count;
            if (this._totalBytesXferred >= this._length)
            {
                return 0;
            }
            long num3 = this._length - this._totalBytesXferred;
            if (num3 < count)
            {
                num = (int) num3;
            }
            int num4 = this._s.Read(buffer, offset, num);
            this.TransformInPlace(buffer, offset, num);
            this._totalBytesXferred += num4;
            return num4;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        private void TransformInPlace(byte[] buffer, int offset, int count)
        {
            for (int i = offset; (i < buffer.Length) && (i < (count + offset)); i += 0x10)
            {
                if (this._mode == CryptoMode.Encrypt)
                {
                    this.ProcessOneBlockWriting(buffer, i, count + offset);
                }
                else
                {
                    this.ProcessOneBlockReading(buffer, i, count + offset);
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this._mode == CryptoMode.Decrypt)
            {
                throw new NotSupportedException();
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (count < 0))
            {
                throw new ArgumentException("Invalid parameters");
            }
            if (buffer.Length < (offset + count))
            {
                throw new ArgumentException("The offset and count are too large");
            }
            if (count != 0)
            {
                if (this._pendingCount != 0)
                {
                    if ((count + this._pendingCount) <= 0x10)
                    {
                        Array.Copy(buffer, offset, this._PendingWriteBuffer, this._pendingCount, count);
                        this._pendingCount += count;
                        return;
                    }
                    int length = 0x10 - this._pendingCount;
                    Array.Copy(buffer, offset, this._PendingWriteBuffer, this._pendingCount, length);
                    this._pendingCount = 0;
                    offset += length;
                    count -= length;
                    this.ProcessOneBlockWriting(this._PendingWriteBuffer, 0, 0x10);
                    this._s.Write(this._PendingWriteBuffer, 0, 0x10);
                    this._totalBytesXferred += 0x10L;
                }
                this.TransformInPlace(buffer, offset, count);
                this._s.Write(buffer, offset, count - this._pendingCount);
                this._totalBytesXferred += count - this._pendingCount;
            }
        }

        public override bool CanRead
        {
            get
            {
                if (this._mode != CryptoMode.Decrypt)
                {
                    return false;
                }
                return true;
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

        public byte[] FinalAuthentication
        {
            get
            {
                if (!this._finalBlock)
                {
                    if (this._totalBytesXferred != 0L)
                    {
                        throw new Exception("The final hash has not been computed.");
                    }
                    return new byte[10];
                }
                byte[] destinationArray = new byte[10];
                Array.Copy(this._mac.Hash, 0, destinationArray, 0, 10);
                return destinationArray;
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

