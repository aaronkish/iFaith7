namespace ICSharpCode.SharpZipLib.Tar
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class TarArchive
    {
        protected bool asciiTranslate;
        protected bool debug;
        protected int groupId;
        protected string groupName;
        protected bool keepOldFiles;
        protected string pathPrefix;
        protected byte[] recordBuf;
        protected int recordSize;
        protected string rootPath;
        protected TarInputStream tarIn;
        protected TarOutputStream tarOut;
        protected int userId;
        protected string userName;
        protected bool verbose;

        public event ProgressMessageHandler ProgressMessageEvent;

        protected TarArchive()
        {
        }

        public void CloseArchive()
        {
            if (this.tarIn != null)
            {
                this.tarIn.Close();
            }
            else if (this.tarOut != null)
            {
                this.tarOut.Flush();
                this.tarOut.Close();
            }
        }

        public static TarArchive CreateInputTarArchive(Stream inputStream)
        {
            return CreateInputTarArchive(inputStream, TarBuffer.DEFAULT_BLKSIZE);
        }

        public static TarArchive CreateInputTarArchive(Stream inputStream, int blockSize)
        {
            return CreateInputTarArchive(inputStream, blockSize, TarBuffer.DEFAULT_RCDSIZE);
        }

        public static TarArchive CreateInputTarArchive(Stream inputStream, int blockSize, int recordSize)
        {
            TarArchive archive = new TarArchive();
            archive.tarIn = new TarInputStream(inputStream, blockSize, recordSize);
            archive.Initialize(recordSize);
            return archive;
        }

        public static TarArchive CreateOutputTarArchive(Stream outputStream)
        {
            return CreateOutputTarArchive(outputStream, TarBuffer.DEFAULT_BLKSIZE);
        }

        public static TarArchive CreateOutputTarArchive(Stream outputStream, int blockSize)
        {
            return CreateOutputTarArchive(outputStream, blockSize, TarBuffer.DEFAULT_RCDSIZE);
        }

        public static TarArchive CreateOutputTarArchive(Stream outputStream, int blockSize, int recordSize)
        {
            TarArchive archive = new TarArchive();
            archive.tarOut = new TarOutputStream(outputStream, blockSize, recordSize);
            archive.Initialize(recordSize);
            return archive;
        }

        private void EnsureDirectoryExists(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                try
                {
                    Directory.CreateDirectory(directoryName);
                }
                catch (Exception exception)
                {
                    throw new IOException("error making directory path '" + directoryName + "', " + exception.Message);
                }
            }
        }

        public void ExtractContents(string destDir)
        {
            TarEntry entry;
        Label_0000:
            entry = this.tarIn.GetNextEntry();
            if (entry == null)
            {
                if (this.debug)
                {
                    Console.Error.WriteLine("READ EOF RECORD");
                }
            }
            else
            {
                this.ExtractEntry(destDir, entry);
                goto Label_0000;
            }
        }

        private void ExtractEntry(string destDir, TarEntry entry)
        {
            int num;
            if (this.verbose)
            {
                this.OnProgressMessageEvent(entry.Name);
            }
            string str = entry.Name.Replace('/', Path.DirectorySeparatorChar);
            if (!destDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                destDir = destDir + Path.DirectorySeparatorChar;
            }
            string directoryName = destDir + str;
            if (entry.IsDirectory)
            {
                this.EnsureDirectoryExists(directoryName);
                return;
            }
            string str3 = Path.GetDirectoryName(directoryName);
            this.EnsureDirectoryExists(str3);
            if (this.keepOldFiles && File.Exists(directoryName))
            {
                if (this.verbose)
                {
                    this.OnProgressMessageEvent("not overwriting " + entry.Name);
                    return;
                }
                return;
            }
            bool flag = false;
            Stream stream = File.Create(directoryName);
            if (this.asciiTranslate)
            {
                flag = !this.IsBinary(directoryName);
            }
            StreamWriter writer = null;
            if (flag)
            {
                writer = new StreamWriter(stream);
            }
            byte[] buffer = new byte[0x8000];
        Label_00F8:
            num = this.tarIn.Read(buffer, 0, buffer.Length);
            if (num > 0)
            {
                if (flag)
                {
                    int index = 0;
                    for (int i = 0; i < num; i++)
                    {
                        if (buffer[i] == 10)
                        {
                            string str4 = Encoding.ASCII.GetString(buffer, index, i - index);
                            writer.WriteLine(str4);
                            index = i + 1;
                        }
                    }
                }
                else
                {
                    stream.Write(buffer, 0, num);
                }
                goto Label_00F8;
            }
            if (flag)
            {
                writer.Close();
            }
            else
            {
                stream.Close();
            }
        }

        private void Initialize(int recordSize)
        {
            this.rootPath = null;
            this.pathPrefix = null;
            this.userId = 0;
            this.userName = string.Empty;
            this.groupId = 0;
            this.groupName = string.Empty;
            this.debug = false;
            this.verbose = false;
            this.keepOldFiles = false;
            this.recordBuf = new byte[this.RecordSize];
        }

        private bool IsBinary(string filename)
        {
            FileStream stream = File.OpenRead(filename);
            byte[] buffer = new byte[(uint) stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);
            stream.Close();
            foreach (byte num2 in buffer)
            {
                switch (num2)
                {
                    case 0:
                    case 0xff:
                        return true;
                }
            }
            return false;
        }

        public void ListContents()
        {
            TarEntry entry;
        Label_0000:
            entry = this.tarIn.GetNextEntry();
            if (entry == null)
            {
                if (this.debug)
                {
                    Console.Error.WriteLine("READ EOF RECORD");
                }
            }
            else
            {
                this.OnProgressMessageEvent(entry.Name);
                goto Label_0000;
            }
        }

        protected virtual void OnProgressMessageEvent(string message)
        {
            if (this.ProgressMessageEvent != null)
            {
                this.ProgressMessageEvent(this, message);
            }
        }

        public void SetAsciiTranslation(bool asciiTranslate)
        {
            this.asciiTranslate = asciiTranslate;
        }

        public void SetDebug(bool debugF)
        {
            this.debug = debugF;
            if (this.tarIn != null)
            {
                this.tarIn.SetDebug(debugF);
            }
            if (this.tarOut != null)
            {
                this.tarOut.SetDebug(debugF);
            }
        }

        public void SetKeepOldFiles(bool keepOldFiles)
        {
            this.keepOldFiles = keepOldFiles;
        }

        public void SetUserInfo(int userId, string userName, int groupId, string groupName)
        {
            this.userId = userId;
            this.userName = userName;
            this.groupId = groupId;
            this.groupName = groupName;
        }

        public void WriteEntry(TarEntry entry, bool recurse)
        {
            string path = null;
            string file = entry.File;
            if ((file == null) || (file.Length == 0))
            {
                entry = TarEntry.CreateTarEntry(entry.Name);
            }
            else
            {
                string name = entry.Name;
                entry = TarEntry.CreateEntryFromFile(file);
                entry.Name = name;
            }
            if (this.verbose)
            {
                this.OnProgressMessageEvent(entry.Name);
            }
            if ((this.asciiTranslate && !entry.IsDirectory) && !this.IsBinary(file))
            {
                path = Path.GetTempFileName();
                StreamReader reader = File.OpenText(file);
                Stream stream = new BufferedStream(File.Create(path));
                while (true)
                {
                    string s = reader.ReadLine();
                    if (s == null)
                    {
                        break;
                    }
                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.WriteByte(10);
                }
                reader.Close();
                stream.Flush();
                stream.Close();
                entry.Size = new FileInfo(path).Length;
                file = path;
            }
            string str5 = null;
            if ((this.rootPath != null) && entry.Name.StartsWith(this.rootPath))
            {
                str5 = entry.Name.Substring(this.rootPath.Length + 1);
            }
            if (this.pathPrefix != null)
            {
                str5 = (str5 == null) ? (this.pathPrefix + "/" + entry.Name) : (this.pathPrefix + "/" + str5);
            }
            if (str5 != null)
            {
                entry.Name = str5;
            }
            this.tarOut.PutNextEntry(entry);
            if (entry.IsDirectory)
            {
                if (recurse)
                {
                    TarEntry[] directoryEntries = entry.GetDirectoryEntries();
                    for (int i = 0; i < directoryEntries.Length; i++)
                    {
                        this.WriteEntry(directoryEntries[i], recurse);
                    }
                }
            }
            else
            {
                Stream stream2 = File.OpenRead(file);
                int num2 = 0;
                byte[] buffer = new byte[0x8000];
                while (true)
                {
                    int count = stream2.Read(buffer, 0, buffer.Length);
                    if (count <= 0)
                    {
                        break;
                    }
                    this.tarOut.Write(buffer, 0, count);
                    num2 += count;
                }
                Console.WriteLine("written " + num2 + " bytes");
                stream2.Close();
                if ((path != null) && (path.Length > 0))
                {
                    File.Delete(path);
                }
                this.tarOut.CloseEntry();
            }
        }

        public int GroupId
        {
            get
            {
                return this.groupId;
            }
        }

        public string GroupName
        {
            get
            {
                return this.groupName;
            }
        }

        public bool IsVerbose
        {
            get
            {
                return this.verbose;
            }
            set
            {
                this.verbose = value;
            }
        }

        public int RecordSize
        {
            get
            {
                if (this.tarIn != null)
                {
                    return this.tarIn.GetRecordSize();
                }
                if (this.tarOut != null)
                {
                    return this.tarOut.GetRecordSize();
                }
                return TarBuffer.DEFAULT_RCDSIZE;
            }
        }

        public int UserId
        {
            get
            {
                return this.userId;
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
        }
    }
}

