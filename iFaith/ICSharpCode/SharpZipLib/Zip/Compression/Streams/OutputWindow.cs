namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
    using System;

    public class OutputWindow
    {
        private byte[] window = new byte[WINDOW_SIZE];
        private int window_end = 0;
        private int window_filled = 0;
        private static int WINDOW_MASK = (WINDOW_SIZE - 1);
        private static int WINDOW_SIZE = 0x8000;

        public void CopyDict(byte[] dict, int offset, int len)
        {
            if (this.window_filled > 0)
            {
                throw new InvalidOperationException();
            }
            if (len > WINDOW_SIZE)
            {
                offset += len - WINDOW_SIZE;
                len = WINDOW_SIZE;
            }
            Array.Copy(dict, offset, this.window, 0, len);
            this.window_end = len & WINDOW_MASK;
        }

        public int CopyOutput(byte[] output, int offset, int len)
        {
            int num = this.window_end;
            if (len > this.window_filled)
            {
                len = this.window_filled;
            }
            else
            {
                num = ((this.window_end - this.window_filled) + len) & WINDOW_MASK;
            }
            int num2 = len;
            int length = len - num;
            if (length > 0)
            {
                Array.Copy(this.window, WINDOW_SIZE - length, output, offset, length);
                offset += length;
                len = num;
            }
            if (output.Length < this.window.Length)
            {
                output = this.window;
            }
            Array.Copy(this.window, num - len, output, offset, len);
            this.window_filled -= num2;
            if (this.window_filled < 0)
            {
                throw new InvalidOperationException();
            }
            return num2;
        }

        public int CopyStored(StreamManipulator input, int len)
        {
            int num2;
            len = Math.Min(Math.Min(len, WINDOW_SIZE - this.window_filled), input.AvailableBytes);
            int length = WINDOW_SIZE - this.window_end;
            if (len > length)
            {
                num2 = input.CopyBytes(this.window, this.window_end, length);
                if (num2 == length)
                {
                    num2 += input.CopyBytes(this.window, 0, len - length);
                }
            }
            else
            {
                num2 = input.CopyBytes(this.window, this.window_end, len);
            }
            this.window_end = (this.window_end + num2) & WINDOW_MASK;
            this.window_filled += num2;
            return num2;
        }

        public int GetAvailable()
        {
            return this.window_filled;
        }

        public int GetFreeSpace()
        {
            return (WINDOW_SIZE - this.window_filled);
        }

        public void Repeat(int len, int dist)
        {
            if ((this.window_filled += len) > WINDOW_SIZE)
            {
                throw new InvalidOperationException("Window full");
            }
            int num2 = (this.window_end - dist) & WINDOW_MASK;
            int num3 = WINDOW_SIZE - len;
            if ((num2 > num3) || (this.window_end >= num3))
            {
                this.SlowRepeat(num2, len, dist);
            }
            else if (len > dist)
            {
                while (len-- > 0)
                {
                    this.window[this.window_end++] = this.window[num2++];
                }
            }
            else
            {
                Array.Copy(this.window, num2, this.window, this.window_end, len);
                this.window_end += len;
            }
        }

        public void Reset()
        {
            this.window_filled = this.window_end = 0;
        }

        private void SlowRepeat(int rep_start, int len, int dist)
        {
            while (len-- > 0)
            {
                this.window[this.window_end++] = this.window[rep_start++];
                this.window_end &= WINDOW_MASK;
                rep_start &= WINDOW_MASK;
            }
        }

        public void Write(int abyte)
        {
            if (this.window_filled++ == WINDOW_SIZE)
            {
                throw new InvalidOperationException("Window full");
            }
            this.window[this.window_end++] = (byte) abyte;
            this.window_end &= WINDOW_MASK;
        }
    }
}

