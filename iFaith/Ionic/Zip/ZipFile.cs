namespace Ionic.Zip
{
    using Ionic.Zlib;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class ZipFile : IEnumerable<ZipEntry>, IEnumerable, IDisposable
    {
        private bool _CaseSensitiveRetrieval;
        private string _Comment;
        private bool _contentsChanged;
        private bool _disposed;
        private List<ZipEntry> _entries;
        private bool _extractOperationCanceled;
        private bool _fileAlreadyExists;
        private bool _ForceNoCompression;
        internal bool _inExtractAll;
        private bool _JustSaved;
        private long _lengthOfReadStream;
        private string _name;
        private string _Password;
        private Encoding _provisionalAlternateEncoding;
        private Stream _readstream;
        private bool _ReadStreamIsOurs;
        private bool _saveOperationCanceled;
        private TextWriter _StatusMessageTextWriter;
        private string _TempFileFolder;
        private string _temporaryFileName;
        private Stream _writestream;
        internal Zip64Option _zip64;
        
        public static readonly Encoding DefaultEncoding = Encoding.GetEncoding("IBM437");
        private object LOCK;
        private static ExtractorSettings[] SettingsList;

        public event EventHandler<ExtractProgressEventArgs> ExtractProgress;

        public event EventHandler<ReadProgressEventArgs> ReadProgress;

        public event EventHandler<SaveProgressEventArgs> SaveProgress;

        static ZipFile()
        {
            ExtractorSettings[] settingsArray = new ExtractorSettings[2];
            ExtractorSettings settings = new ExtractorSettings();
            settings.Flavor = SelfExtractorFlavor.WinFormsApplication;
            List<string> list = new List<string>();
            list.Add("System.Windows.Forms.dll");
            list.Add("System.dll");
            list.Add("System.Drawing.dll");
            settings.ReferencedAssemblies = list;
            List<string> list2 = new List<string>();
            list2.Add("Ionic.Zip.WinFormsSelfExtractorStub.resources");
            list2.Add("Ionic.Zip.PasswordDialog.resources");
            list2.Add("Ionic.Zip.ZipContentsDialog.resources");
            settings.CopyThroughResources = list2;
            List<string> list3 = new List<string>();
            list3.Add("Ionic.Zip.Resources.WinFormsSelfExtractorStub.cs");
            list3.Add("Ionic.Zip.WinFormsSelfExtractorStub");
            list3.Add("Ionic.Zip.Resources.PasswordDialog.cs");
            list3.Add("Ionic.Zip.PasswordDialog");
            list3.Add("Ionic.Zip.Resources.ZipContentsDialog.cs");
            list3.Add("Ionic.Zip.ZipContentsDialog");
            settings.ResourcesToCompile = list3;
            settingsArray[0] = settings;
            ExtractorSettings settings2 = new ExtractorSettings();
            settings2.Flavor = SelfExtractorFlavor.ConsoleApplication;
            settings2.ReferencedAssemblies = null;
            settings2.CopyThroughResources = null;
            List<string> list4 = new List<string>();
            list4.Add("Ionic.Zip.Resources.CommandLineSelfExtractorStub.cs");
            settings2.ResourcesToCompile = list4;
            settingsArray[1] = settings2;
            SettingsList = settingsArray;
        }

        public ZipFile()
        {
            this._lengthOfReadStream = -99L;
            this._ReadStreamIsOurs = true;
            this.LOCK = new object();
            this._inExtractAll = false;
            this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
            this._zip64 = Zip64Option.Default;
            this.InitFile(null, null);
        }

        public ZipFile(string zipFileName)
        {
            this._lengthOfReadStream = -99L;
            this._ReadStreamIsOurs = true;
            this.LOCK = new object();
            this._inExtractAll = false;
            this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
            this._zip64 = Zip64Option.Default;
            try
            {
                this.InitFile(zipFileName, null);
            }
            catch (Exception exception)
            {
                throw new ZipException(string.Format("{0} is not a valid zip file", zipFileName), exception);
            }
        }

        public ZipFile(Encoding encoding)
        {
            this._lengthOfReadStream = -99L;
            this._ReadStreamIsOurs = true;
            this.LOCK = new object();
            this._inExtractAll = false;
            this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
            this._zip64 = Zip64Option.Default;
            this.InitFile(null, null);
            this.ProvisionalAlternateEncoding = encoding;
        }

        public ZipFile(string zipFileName, TextWriter statusMessageWriter)
        {
            this._lengthOfReadStream = -99L;
            this._ReadStreamIsOurs = true;
            this.LOCK = new object();
            this._inExtractAll = false;
            this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
            this._zip64 = Zip64Option.Default;
            try
            {
                this.InitFile(zipFileName, statusMessageWriter);
            }
            catch (Exception exception)
            {
                throw new ZipException(string.Format("{0} is not a valid zip file", zipFileName), exception);
            }
        }

        public ZipFile(string zipFileName, Encoding encoding)
        {
            this._lengthOfReadStream = -99L;
            this._ReadStreamIsOurs = true;
            this.LOCK = new object();
            this._inExtractAll = false;
            this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
            this._zip64 = Zip64Option.Default;
            try
            {
                this.InitFile(zipFileName, null);
                this.ProvisionalAlternateEncoding = encoding;
            }
            catch (Exception exception)
            {
                throw new ZipException(string.Format("{0} is not a valid zip file", zipFileName), exception);
            }
        }

        public ZipFile(string zipFileName, TextWriter statusMessageWriter, Encoding encoding)
        {
            this._lengthOfReadStream = -99L;
            this._ReadStreamIsOurs = true;
            this.LOCK = new object();
            this._inExtractAll = false;
            this._provisionalAlternateEncoding = Encoding.GetEncoding("IBM437");
            this._zip64 = Zip64Option.Default;
            try
            {
                this.InitFile(zipFileName, statusMessageWriter);
                this.ProvisionalAlternateEncoding = encoding;
            }
            catch (Exception exception)
            {
                throw new ZipException(string.Format("{0} is not a valid zip file", zipFileName), exception);
            }
        }

        public ZipEntry AddDirectory(string directoryName)
        {
            return this.AddDirectory(directoryName, null);
        }

        public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
        {
            return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
        }

        public ZipEntry AddDirectoryByName(string directoryNameInArchive)
        {
            ZipEntry entry = ZipEntry.Create(directoryNameInArchive, directoryNameInArchive);
            entry._Source = EntrySource.Filesystem;
            entry.MarkAsDirectory();
            entry._zipfile = this;
            this.InsureUniqueEntry(entry);
            this._entries.Add(entry);
            this._contentsChanged = true;
            return entry;
        }

        public ZipEntry AddFile(string fileName)
        {
            return this.AddFile(fileName, null);
        }

        public ZipEntry AddFile(string fileName, string directoryPathInArchive)
        {
            string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
            ZipEntry entry = ZipEntry.Create(fileName, nameInArchive);
            entry.ForceNoCompression = this.ForceNoCompression;
            entry.WillReadTwiceOnInflation = this.WillReadTwiceOnInflation;
            entry.WantCompression = this.WantCompression;
            entry.ProvisionalAlternateEncoding = this.ProvisionalAlternateEncoding;
            entry._Source = EntrySource.Filesystem;
            entry._zipfile = this;
            entry.Encryption = this.Encryption;
            entry.Password = this._Password;
            if (this.Verbose)
            {
                this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
            }
            this.InsureUniqueEntry(entry);
            this._entries.Add(entry);
            this._contentsChanged = true;
            return entry;
        }

        public ZipEntry AddFileFromString(string fileName, string directoryPathInArchive, string content)
        {
            MemoryStream stream = SharedUtilities.StringToMemoryStream(content);
            return this.AddFileStream(fileName, directoryPathInArchive, stream);
        }

        public ZipEntry AddFileStream(string fileName, string directoryPathInArchive, Stream stream)
        {
            string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
            ZipEntry entry = ZipEntry.Create(fileName, nameInArchive, stream);
            entry.ForceNoCompression = this.ForceNoCompression;
            entry.WillReadTwiceOnInflation = this.WillReadTwiceOnInflation;
            entry.WantCompression = this.WantCompression;
            entry.ProvisionalAlternateEncoding = this.ProvisionalAlternateEncoding;
            entry._Source = EntrySource.Stream;
            entry._zipfile = this;
            entry.Encryption = this.Encryption;
            entry.Password = this._Password;
            if (this.Verbose)
            {
                this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
            }
            this.InsureUniqueEntry(entry);
            this._entries.Add(entry);
            this._contentsChanged = true;
            return entry;
        }

        public ZipEntry AddItem(string fileOrDirectoryName)
        {
            return this.AddItem(fileOrDirectoryName, null);
        }

        public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
        {
            if (File.Exists(fileOrDirectoryName))
            {
                return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
            }
            if (!Directory.Exists(fileOrDirectoryName))
            {
                throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", fileOrDirectoryName));
            }
            return this.AddDirectory(fileOrDirectoryName, directoryPathInArchive);
        }

        private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
        {
            if (rootDirectoryPathInArchive == null)
            {
                rootDirectoryPathInArchive = "";
            }
            return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, 0);
        }

        private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, int level)
        {
            if (this.Verbose)
            {
                this.StatusMessageTextWriter.WriteLine("{0} {1}...", (action == AddOrUpdateAction.AddOnly) ? "adding" : "Adding or updating", directoryName);
            }
            string fileName = rootDirectoryPathInArchive;
            ZipEntry item = null;
            if (level > 0)
            {
                int length = directoryName.Length;
                for (int i = level; i > 0; i--)
                {
                    length = directoryName.LastIndexOfAny(@"/\".ToCharArray(), length - 1, length - 1);
                }
                fileName = directoryName.Substring(length + 1);
                fileName = Path.Combine(rootDirectoryPathInArchive, fileName);
            }
            if ((level > 0) || (rootDirectoryPathInArchive != ""))
            {
                item = ZipEntry.Create(directoryName, fileName);
                item.ProvisionalAlternateEncoding = this.ProvisionalAlternateEncoding;
                item._Source = EntrySource.Filesystem;
                item.MarkAsDirectory();
                item._zipfile = this;
                ZipEntry entry2 = this[item.FileName];
                if (entry2 == null)
                {
                    this._entries.Add(item);
                    this._contentsChanged = true;
                }
                fileName = item.FileName;
            }
            string[] files = Directory.GetFiles(directoryName);
            foreach (string str2 in files)
            {
                if (action == AddOrUpdateAction.AddOnly)
                {
                    this.AddFile(str2, fileName);
                }
                else
                {
                    this.UpdateFile(str2, fileName);
                }
            }
            string[] directories = Directory.GetDirectories(directoryName);
            foreach (string str3 in directories)
            {
                this.AddOrUpdateDirectoryImpl(str3, rootDirectoryPathInArchive, action, level + 1);
            }
            return item;
        }

        private static bool BlocksAreEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void CleanupAfterSaveOperation()
        {
            if ((this._temporaryFileName != null) && (this._name != null))
            {
                if (this._writestream != null)
                {
                    try
                    {
                        this._writestream.Close();
                    }
                    catch
                    {
                    }
                    try
                    {
                        this._writestream.Dispose();
                    }
                    catch
                    {
                    }
                }
                this._writestream = null;
                this.RemoveTempFile();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (!this._disposed)
            {
                if (disposeManagedResources)
                {
                    if (this._ReadStreamIsOurs && (this._readstream != null))
                    {
                        this._readstream.Dispose();
                        this._readstream = null;
                    }
                    if (((this._temporaryFileName != null) && (this._name != null)) && (this._writestream != null))
                    {
                        this._writestream.Dispose();
                        this._writestream = null;
                    }
                }
                this._disposed = true;
            }
        }

        public void Extract(string fileName)
        {
            ZipEntry entry = this[fileName];
            entry.Password = this._Password;
            entry.Extract();
        }

        public void Extract(string fileName, bool wantOverwrite)
        {
            ZipEntry entry = this[fileName];
            entry.Password = this._Password;
            entry.Extract(wantOverwrite);
        }

        public void Extract(string fileName, Stream outputStream)
        {
            if (!((outputStream != null) && outputStream.CanWrite))
            {
                throw new ZipException("Cannot extract.", new ArgumentException("The OutputStream must be a writable stream.", "outputStream"));
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ZipException("Cannot extract.", new ArgumentException("The file name must be neither null nor empty.", "fileName"));
            }
            ZipEntry entry = this[fileName];
            entry.Password = this._Password;
            entry.Extract(outputStream);
        }

        public void Extract(string fileName, string directoryName)
        {
            ZipEntry entry = this[fileName];
            entry.Password = this._Password;
            entry.Extract(directoryName);
        }

        public void Extract(string fileName, string directoryName, bool wantOverwrite)
        {
            ZipEntry entry = this[fileName];
            entry.Password = this._Password;
            entry.Extract(directoryName, wantOverwrite);
        }

        public void ExtractAll(string path)
        {
            this.ExtractAll(path, false);
        }

        public void ExtractAll(string path, bool wantOverwrite)
        {
            bool verbose = this.Verbose;
            this._inExtractAll = true;
            try
            {
                this.OnExtractAllStarted(path, wantOverwrite);
                int current = 0;
                foreach (ZipEntry entry in this._entries)
                {
                    if (verbose)
                    {
                        this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", new object[] { "Name", "Modified", "Size", "Ratio", "Packed" });
                        this.StatusMessageTextWriter.WriteLine(new string('-', 0x48));
                        verbose = false;
                    }
                    if (this.Verbose)
                    {
                        this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", new object[] { entry.FileName, entry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"), entry.UncompressedSize, entry.CompressionRatio, entry.CompressedSize });
                        if (!string.IsNullOrEmpty(entry.Comment))
                        {
                            this.StatusMessageTextWriter.WriteLine("  Comment: {0}", entry.Comment);
                        }
                    }
                    entry.Password = this._Password;
                    this.OnExtractEntry(current, true, entry, path, wantOverwrite);
                    entry.Extract(path, wantOverwrite);
                    current++;
                    this.OnExtractEntry(current, false, entry, path, wantOverwrite);
                    if (this._extractOperationCanceled)
                    {
                        break;
                    }
                }
                this.OnExtractAllCompleted(path, wantOverwrite);
            }
            finally
            {
                this._inExtractAll = false;
            }
        }

        ~ZipFile()
        {
            this.Dispose(false);
        }

        internal static string GenerateUniquePathname(string extension, string ContainingDirectory)
        {
            string path = null;
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            string str3 = (ContainingDirectory == null) ? Environment.GetEnvironmentVariable("TEMP") : ContainingDirectory;
            if (str3 == null)
            {
                return null;
            }
            int num = 0;
            do
            {
                num++;
                string str5 = string.Format("{0}-{1}-{2}.{3}", new object[] { name, DateTime.Now.ToString("yyyyMMMdd-HHmmss"), num, extension });
                path = Path.Combine(str3, str5);
            }
            while (File.Exists(path) || Directory.Exists(path));
            return path;
        }

        public IEnumerator<ZipEntry> GetEnumerator()
        {
            foreach (ZipEntry iteratorVariable0 in this._entries)
            {
                yield return iteratorVariable0;
            }
        }

        private void InitFile(string zipFileName, TextWriter statusMessageWriter)
        {
            this._name = zipFileName;
            this._StatusMessageTextWriter = statusMessageWriter;
            this._contentsChanged = true;
            this.CompressionLevel = Ionic.Zlib.CompressionLevel.DEFAULT;
            if (File.Exists(this._name))
            {
                ReadIntoInstance(this);
                this._fileAlreadyExists = true;
            }
            else
            {
                this._entries = new List<ZipEntry>();
            }
        }

        private void InsureUniqueEntry(ZipEntry ze1)
        {
            foreach (ZipEntry entry in this._entries)
            {
                if (SharedUtilities.TrimVolumeAndSwapSlashes(ze1.FileName) == entry.FileName)
                {
                    throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", ze1.FileName));
                }
            }
        }

        public static bool IsZipFile(string fileName)
        {
            return IsZipFile(fileName, false);
        }

        public static bool IsZipFile(string fileName, bool testExtract)
        {
            bool flag = false;
            try
            {
                if (!File.Exists(fileName))
                {
                    return false;
                }
                Stream @null = Stream.Null;
                using (ZipFile file = Read(fileName, (TextWriter) null, Encoding.GetEncoding("IBM437")))
                {
                    if (testExtract)
                    {
                        foreach (ZipEntry entry in file)
                        {
                            if (!entry.IsDirectory)
                            {
                                entry.Extract(@null);
                            }
                        }
                    }
                }
                flag = true;
            }
            catch (Exception)
            {
            }
            return flag;
        }

        internal void NotifyEntryChanged()
        {
            this._contentsChanged = true;
        }

        private void OnExtractAllCompleted(string path, bool wantOverwrite)
        {
            if (this.ExtractProgress != null)
            {
                lock (this.LOCK)
                {
                    ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path, wantOverwrite);
                    this.ExtractProgress(this, e);
                }
            }
        }

        private void OnExtractAllStarted(string path, bool wantOverwrite)
        {
            if (this.ExtractProgress != null)
            {
                lock (this.LOCK)
                {
                    ExtractProgressEventArgs e = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path, wantOverwrite);
                    this.ExtractProgress(this, e);
                }
            }
        }

        internal bool OnExtractBlock(ZipEntry entry, int bytesWritten, long totalBytesToWrite)
        {
            if (this.ExtractProgress != null)
            {
                lock (this.LOCK)
                {
                    ExtractProgressEventArgs e = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
                    this.ExtractProgress(this, e);
                    if (e.Cancel)
                    {
                        this._extractOperationCanceled = true;
                    }
                }
            }
            return this._extractOperationCanceled;
        }

        private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path, bool overwrite)
        {
            if (this.ExtractProgress != null)
            {
                lock (this.LOCK)
                {
                    ExtractProgressEventArgs e = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path, overwrite);
                    this.ExtractProgress(this, e);
                    if (e.Cancel)
                    {
                        this._extractOperationCanceled = true;
                    }
                }
            }
        }

        internal void OnReadBytes(ZipEntry entry)
        {
            if (this.ReadProgress != null)
            {
                lock (this.LOCK)
                {
                    ReadProgressEventArgs e = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
                    this.ReadProgress(this, e);
                }
            }
        }

        private void OnReadCompleted()
        {
            if (this.ReadProgress != null)
            {
                lock (this.LOCK)
                {
                    ReadProgressEventArgs e = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
                    this.ReadProgress(this, e);
                }
            }
        }

        internal void OnReadEntry(bool before, ZipEntry entry)
        {
            if (this.ReadProgress != null)
            {
                lock (this.LOCK)
                {
                    ReadProgressEventArgs e = before ? ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count) : ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count);
                    this.ReadProgress(this, e);
                }
            }
        }

        private void OnReadStarted()
        {
            if (this.ReadProgress != null)
            {
                lock (this.LOCK)
                {
                    ReadProgressEventArgs e = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
                    this.ReadProgress(this, e);
                }
            }
        }

        internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
        {
            if (this.SaveProgress != null)
            {
                lock (this.LOCK)
                {
                    SaveProgressEventArgs e = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
                    this.SaveProgress(this, e);
                    if (e.Cancel)
                    {
                        this._saveOperationCanceled = true;
                    }
                }
            }
            return this._saveOperationCanceled;
        }

        private void OnSaveCompleted()
        {
            if (this.SaveProgress != null)
            {
                lock (this.LOCK)
                {
                    SaveProgressEventArgs e = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
                    this.SaveProgress(this, e);
                }
            }
        }

        private void OnSaveEntry(int current, ZipEntry entry, bool before)
        {
            if (this.SaveProgress != null)
            {
                lock (this.LOCK)
                {
                    SaveProgressEventArgs e = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
                    this.SaveProgress(this, e);
                    if (e.Cancel)
                    {
                        this._saveOperationCanceled = true;
                    }
                }
            }
        }

        private void OnSaveEvent(ZipProgressEventType eventFlavor)
        {
            if (this.SaveProgress != null)
            {
                lock (this.LOCK)
                {
                    SaveProgressEventArgs e = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
                    this.SaveProgress(this, e);
                    if (e.Cancel)
                    {
                        this._saveOperationCanceled = true;
                    }
                }
            }
        }

        private void OnSaveStarted()
        {
            if (this.SaveProgress != null)
            {
                lock (this.LOCK)
                {
                    SaveProgressEventArgs e = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
                    this.SaveProgress(this, e);
                }
            }
        }

        internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before, bool overwrite)
        {
            if (this.ExtractProgress != null)
            {
                lock (this.LOCK)
                {
                    ExtractProgressEventArgs e = before ? ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path, overwrite) : ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path, overwrite);
                    this.ExtractProgress(this, e);
                    if (e.Cancel)
                    {
                        this._extractOperationCanceled = true;
                    }
                }
            }
            return this._extractOperationCanceled;
        }

        public static ZipFile Read(Stream zipStream)
        {
            return Read(zipStream, (TextWriter) null, DefaultEncoding);
        }

        public static ZipFile Read(string zipFileName)
        {
            return Read(zipFileName, (TextWriter) null, DefaultEncoding);
        }

        public static ZipFile Read(byte[] buffer)
        {
            return Read(buffer, null, DefaultEncoding);
        }

        public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter)
        {
            return Read(zipStream, statusMessageWriter, DefaultEncoding);
        }

        public static ZipFile Read(Stream zipStream, Encoding encoding)
        {
            return Read(zipStream, (TextWriter) null, encoding);
        }

        public static ZipFile Read(byte[] buffer, TextWriter statusMessageWriter)
        {
            return Read(buffer, statusMessageWriter, DefaultEncoding);
        }

        public static ZipFile Read(Stream zipStream, EventHandler<ReadProgressEventArgs> readProgress)
        {
            return Read(zipStream, null, DefaultEncoding, readProgress);
        }

        public static ZipFile Read(string zipFileName, EventHandler<ReadProgressEventArgs> readProgress)
        {
            return Read(zipFileName, null, DefaultEncoding, readProgress);
        }

        public static ZipFile Read(string zipFileName, TextWriter statusMessageWriter)
        {
            return Read(zipFileName, statusMessageWriter, DefaultEncoding);
        }

        public static ZipFile Read(string zipFileName, Encoding encoding)
        {
            return Read(zipFileName, (TextWriter) null, encoding);
        }

        public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding)
        {
            return Read(zipStream, statusMessageWriter, encoding, null);
        }

        public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, EventHandler<ReadProgressEventArgs> readProgress)
        {
            return Read(zipStream, statusMessageWriter, DefaultEncoding, readProgress);
        }

        public static ZipFile Read(byte[] buffer, TextWriter statusMessageWriter, Encoding encoding)
        {
            ZipFile zf = new ZipFile();
            zf._StatusMessageTextWriter = statusMessageWriter;
            zf._provisionalAlternateEncoding = encoding;
            zf._readstream = new MemoryStream(buffer);
            zf._ReadStreamIsOurs = true;
            ReadIntoInstance(zf);
            return zf;
        }

        public static ZipFile Read(Stream zipStream, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
        {
            return Read(zipStream, null, encoding, readProgress);
        }

        public static ZipFile Read(string zipFileName, TextWriter statusMessageWriter, EventHandler<ReadProgressEventArgs> readProgress)
        {
            return Read(zipFileName, statusMessageWriter, DefaultEncoding, readProgress);
        }

        public static ZipFile Read(string zipFileName, TextWriter statusMessageWriter, Encoding encoding)
        {
            return Read(zipFileName, statusMessageWriter, encoding, null);
        }

        public static ZipFile Read(string zipFileName, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
        {
            return Read(zipFileName, null, encoding, readProgress);
        }

        public static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
        {
            if (zipStream == null)
            {
                throw new ZipException("Cannot read.", new ArgumentException("The stream must be non-null", "zipStream"));
            }
            ZipFile zf = new ZipFile();
            zf._provisionalAlternateEncoding = encoding;
            if (readProgress != null)
            {
                zf.ReadProgress = (EventHandler<ReadProgressEventArgs>) Delegate.Combine(zf.ReadProgress, readProgress);
            }
            zf._StatusMessageTextWriter = statusMessageWriter;
            zf._readstream = zipStream;
            zf._ReadStreamIsOurs = false;
            ReadIntoInstance(zf);
            return zf;
        }

        public static ZipFile Read(string zipFileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
        {
            ZipFile zf = new ZipFile();
            zf.ProvisionalAlternateEncoding = encoding;
            zf._StatusMessageTextWriter = statusMessageWriter;
            zf._name = zipFileName;
            if (readProgress != null)
            {
                zf.ReadProgress = readProgress;
            }
            try
            {
                ReadIntoInstance(zf);
                zf._fileAlreadyExists = true;
            }
            catch (Exception exception)
            {
                throw new ZipException(string.Format("{0} is not a valid zip file", zipFileName), exception);
            }
            return zf;
        }

        private static void ReadCentralDirectory(ZipFile zf)
        {
            ZipEntry entry;
            zf._entries = new List<ZipEntry>();
            while ((entry = ZipEntry.ReadDirEntry(zf.ReadStream, zf.ProvisionalAlternateEncoding)) != null)
            {
                entry.ResetDirEntry();
                entry._zipfile = zf;
                entry._Source = EntrySource.Zipfile;
                entry._archiveStream = zf.ReadStream;
                zf.OnReadEntry(true, null);
                if (zf.Verbose)
                {
                    zf.StatusMessageTextWriter.WriteLine("  {0}", entry.FileName);
                }
                zf._entries.Add(entry);
            }
            ReadCentralDirectoryFooter(zf);
            if (!(!zf.Verbose || string.IsNullOrEmpty(zf.Comment)))
            {
                zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
            }
            zf.OnReadCompleted();
        }

        private static void ReadCentralDirectoryFooter(ZipFile zf)
        {
            Stream readStream = zf.ReadStream;
            int num = SharedUtilities.ReadSignature(readStream);
            byte[] buffer = null;
            int num2 = 0;
            if (num == 0x6064b50L)
            {
                buffer = new byte[0x34];
                readStream.Read(buffer, 0, buffer.Length);
                long num3 = BitConverter.ToInt64(buffer, 0);
                if (num3 < 0x2cL)
                {
                    throw new ZipException("Bad DataSize in the ZIP64 Central Directory.");
                }
                num2 = 8;
                num2 += 2;
                num2 += 2;
                num2 += 4;
                num2 += 4;
                num2 += 8;
                num2 += 8;
                num2 += 8;
                num2 += 8;
                buffer = new byte[num3 - 0x2cL];
                readStream.Read(buffer, 0, buffer.Length);
                if (SharedUtilities.ReadSignature(readStream) != 0x7064b50L)
                {
                    throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
                }
                buffer = new byte[0x10];
                readStream.Read(buffer, 0, buffer.Length);
                num = SharedUtilities.ReadSignature(readStream);
            }
            if (num != 0x6054b50L)
            {
                readStream.Seek(-4L, SeekOrigin.Current);
                throw new BadReadException(string.Format("  ZipFile::Read(): Bad signature ({0:X8}) at position 0x{1:X8}", num, readStream.Position));
            }
            buffer = new byte[0x10];
            zf.ReadStream.Read(buffer, 0, buffer.Length);
            ReadZipFileComment(zf);
        }

        private static void ReadIntoInstance(ZipFile zf)
        {
            Stream readStream = zf.ReadStream;
            try
            {
                if (!readStream.CanSeek)
                {
                    ReadIntoInstance_Orig(zf);
                    return;
                }
                long position = readStream.Position;
                if (VerifyBeginningOfZipFile(readStream) == 0x6054b50)
                {
                    return;
                }
                int num3 = 0;
                bool flag2 = false;
                long offset = readStream.Length - 0x40L;
                long num5 = Math.Max((long) (readStream.Length - 0x4000L), (long) 10L);
                do
                {
                    readStream.Seek(offset, SeekOrigin.Begin);
                    if (SharedUtilities.FindSignature(readStream, 0x6054b50) != -1L)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        num3++;
                        offset -= (0x20 * (num3 + 1)) * num3;
                        if (offset < 0L)
                        {
                            offset = 0L;
                        }
                    }
                }
                while (!flag2 && (offset > num5));
                if (flag2)
                {
                    byte[] buffer = new byte[0x10];
                    zf.ReadStream.Read(buffer, 0, buffer.Length);
                    int num7 = 12;
                    uint num8 = (uint) (((buffer[num7++] + (buffer[num7++] * 0x100)) + ((buffer[num7++] * 0x100) * 0x100)) + (((buffer[num7++] * 0x100) * 0x100) * 0x100));
                    if (num8 == uint.MaxValue)
                    {
                        Zip64SeekToCentralDirectory(readStream);
                    }
                    else
                    {
                        readStream.Seek((long) num8, SeekOrigin.Begin);
                    }
                    ReadCentralDirectory(zf);
                }
                else
                {
                    readStream.Seek(position, SeekOrigin.Begin);
                    ReadIntoInstance_Orig(zf);
                }
            }
            catch (Exception)
            {
                if (zf._ReadStreamIsOurs && (zf._readstream != null))
                {
                    try
                    {
                        zf._readstream.Close();
                        zf._readstream.Dispose();
                        zf._readstream = null;
                    }
                    finally
                    {
                    }
                }
                throw;
            }
            zf._contentsChanged = false;
        }

        private static void ReadIntoInstance_Orig(ZipFile zf)
        {
            ZipEntry entry;
            ZipEntry entry3;
            zf.OnReadStarted();
            zf._entries = new List<ZipEntry>();
            if (zf.Verbose)
            {
                if (zf.Name == null)
                {
                    zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
                }
                else
                {
                    zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
                }
            }
            for (bool flag2 = true; (entry = ZipEntry.Read(zf, flag2)) != null; flag2 = false)
            {
                if (zf.Verbose)
                {
                    zf.StatusMessageTextWriter.WriteLine("  {0}", entry.FileName);
                }
                zf._entries.Add(entry);
            }
            while ((entry3 = ZipEntry.ReadDirEntry(zf.ReadStream, zf.ProvisionalAlternateEncoding)) != null)
            {
                foreach (ZipEntry entry2 in zf._entries)
                {
                    if (entry2.FileName == entry3.FileName)
                    {
                        entry2._Comment = entry3.Comment;
                        if (entry3.AttributesIndicateDirectory)
                        {
                            entry2.MarkAsDirectory();
                        }
                        break;
                    }
                }
            }
            ReadCentralDirectoryFooter(zf);
            if (!(!zf.Verbose || string.IsNullOrEmpty(zf.Comment)))
            {
                zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
            }
            zf.OnReadCompleted();
        }

        private static void ReadZipFileComment(ZipFile zf)
        {
            byte[] buffer = new byte[2];
            zf.ReadStream.Read(buffer, 0, buffer.Length);
            short num = (short) (buffer[0] + (buffer[1] * 0x100));
            if (num > 0)
            {
                buffer = new byte[num];
                zf.ReadStream.Read(buffer, 0, buffer.Length);
                string s = DefaultEncoding.GetString(buffer, 0, buffer.Length);
                byte[] bytes = DefaultEncoding.GetBytes(s);
                if (BlocksAreEqual(buffer, bytes))
                {
                    zf.Comment = s;
                }
                else
                {
                    zf.Comment = ((zf._provisionalAlternateEncoding.CodePage == 0x1b5) ? Encoding.UTF8 : zf._provisionalAlternateEncoding).GetString(buffer, 0, buffer.Length);
                }
            }
        }

        public void RemoveEntry(ZipEntry entry)
        {
            if (!this._entries.Contains(entry))
            {
                throw new ArgumentException("The entry you specified does not exist in the zip archive.");
            }
            this._entries.Remove(entry);
            this._contentsChanged = true;
        }

        public void RemoveEntry(string fileName)
        {
            string str = ZipEntry.NameInArchive(fileName, null);
            ZipEntry entry = this[str];
            if (entry == null)
            {
                throw new ArgumentException("The entry you specified was not found in the zip archive.");
            }
            this.RemoveEntry(entry);
        }

        private void RemoveTempFile()
        {
            try
            {
                if (File.Exists(this._temporaryFileName))
                {
                    File.Delete(this._temporaryFileName);
                }
            }
            catch (Exception exception)
            {
                this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", exception.Message);
            }
        }

        internal void Reset()
        {
            if (this._JustSaved)
            {
                ZipFile zf = new ZipFile();
                zf._name = this._name;
                zf.ProvisionalAlternateEncoding = this.ProvisionalAlternateEncoding;
                ReadIntoInstance(zf);
                foreach (ZipEntry entry in zf)
                {
                    foreach (ZipEntry entry2 in this)
                    {
                        if (entry.FileName == entry2.FileName)
                        {
                            entry2.CopyMetaData(entry);
                        }
                    }
                }
                this._JustSaved = false;
            }
        }

        public void Save()
        {
            try
            {
                this._saveOperationCanceled = false;
                this.OnSaveStarted();
                if (this.WriteStream == null)
                {
                    throw new BadStateException("You haven't specified where to save the zip.");
                }
                if (this._contentsChanged)
                {
                    if (this.Verbose)
                    {
                        this.StatusMessageTextWriter.WriteLine("Saving....");
                    }
                    if ((this._entries.Count >= 0xffff) && (this._zip64 == Zip64Option.Default))
                    {
                        throw new ZipException("The number of entries is 0xFFFF or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
                    }
                    int current = 0;
                    foreach (ZipEntry entry in this._entries)
                    {
                        this.OnSaveEntry(current, entry, true);
                        entry.Write(this.WriteStream);
                        entry._zipfile = this;
                        current++;
                        this.OnSaveEntry(current, entry, false);
                        if (this._saveOperationCanceled)
                        {
                            break;
                        }
                    }
                    if (!this._saveOperationCanceled)
                    {
                        this.WriteCentralDirectoryStructure(this.WriteStream);
                        this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
                        if ((this._temporaryFileName != null) && (this._name != null))
                        {
                            this.WriteStream.Close();
                            this.WriteStream.Dispose();
                            this.WriteStream = null;
                            if (this._saveOperationCanceled)
                            {
                                return;
                            }
                            if (this._fileAlreadyExists && (this._readstream != null))
                            {
                                this._readstream.Close();
                                this._readstream = null;
                            }
                            if (this._fileAlreadyExists)
                            {
                                File.Delete(this._name);
                                this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
                                File.Move(this._temporaryFileName, this._name);
                                this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
                            }
                            else
                            {
                                File.Move(this._temporaryFileName, this._name);
                            }
                            this._fileAlreadyExists = true;
                        }
                        this.OnSaveCompleted();
                        this._JustSaved = true;
                    }
                }
            }
            finally
            {
                this.CleanupAfterSaveOperation();
            }
        }

        public void Save(Stream outputStream)
        {
            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("The outputStream must be a writable stream.");
            }
            this._name = null;
            this._writestream = new CountingStream(outputStream);
            this._contentsChanged = true;
            this._fileAlreadyExists = false;
            this.Save();
        }

        public void Save(string zipFileName)
        {
            if (this._name == null)
            {
                this._writestream = null;
            }
            this._name = zipFileName;
            if (Directory.Exists(this._name))
            {
                throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "zipFileName"));
            }
            this._contentsChanged = true;
            this._fileAlreadyExists = File.Exists(this._name);
            this.Save();
        }

        public void SaveSelfExtractor(string exeToGenerate, SelfExtractorFlavor flavor)
        {
            if (File.Exists(exeToGenerate) && this.Verbose)
            {
                this.StatusMessageTextWriter.WriteLine("The existing file ({0}) will be overwritten.", exeToGenerate);
            }
            if (!exeToGenerate.EndsWith(".exe") && this.Verbose)
            {
                this.StatusMessageTextWriter.WriteLine("Warning: The generated self-extracting file will not have an .exe extension.");
            }
            string str = this.SfxSaveTemporary();
            this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
            if (str != null)
            {
                Assembly assembly = typeof(ZipFile).Assembly;
                CSharpCodeProvider provider = new CSharpCodeProvider();
                ExtractorSettings settings = null;
                foreach (ExtractorSettings settings2 in SettingsList)
                {
                    if (settings2.Flavor == flavor)
                    {
                        settings = settings2;
                        break;
                    }
                }
                if (settings == null)
                {
                    throw new BadStateException(string.Format("While saving a Self-Extracting Zip, Cannot find that flavor ({0})?", flavor));
                }
                CompilerParameters options = new CompilerParameters();
                options.ReferencedAssemblies.Add(assembly.Location);
                if (settings.ReferencedAssemblies != null)
                {
                    foreach (string str2 in settings.ReferencedAssemblies)
                    {
                        options.ReferencedAssemblies.Add(str2);
                    }
                }
                options.GenerateInMemory = false;
                options.GenerateExecutable = true;
                options.IncludeDebugInformation = false;
                options.OutputAssembly = exeToGenerate;
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string path = GenerateUniquePathname("tmp", null);
                if ((settings.CopyThroughResources != null) && (settings.CopyThroughResources.Count != 0))
                {
                    Directory.CreateDirectory(path);
                    int count = 0;
                    byte[] buffer = new byte[0x400];
                    foreach (string str4 in settings.CopyThroughResources)
                    {
                        string str5 = Path.Combine(path, str4);
                        using (Stream stream = executingAssembly.GetManifestResourceStream(str4))
                        {
                            using (FileStream stream2 = File.OpenWrite(str5))
                            {
                                do
                                {
                                    count = stream.Read(buffer, 0, buffer.Length);
                                    stream2.Write(buffer, 0, count);
                                }
                                while (count > 0);
                            }
                        }
                        options.EmbeddedResources.Add(str5);
                    }
                }
                options.EmbeddedResources.Add(str);
                options.EmbeddedResources.Add(assembly.Location);
                StringBuilder builder = new StringBuilder();
                foreach (string str6 in settings.ResourcesToCompile)
                {
                    using (StreamReader reader = new StreamReader(executingAssembly.GetManifestResourceStream(str6)))
                    {
                        while (reader.Peek() >= 0)
                        {
                            builder.Append(reader.ReadLine()).Append("\n");
                        }
                    }
                    builder.Append("\n\n");
                }
                string str7 = builder.ToString();
                CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { str7 });
                if (results == null)
                {
                    throw new SfxGenerationException("Cannot compile the extraction logic!");
                }
                if (this.Verbose)
                {
                    foreach (string str8 in results.Output)
                    {
                        this.StatusMessageTextWriter.WriteLine(str8);
                    }
                }
                if (results.Errors.Count != 0)
                {
                    throw new SfxGenerationException("Errors compiling the extraction logic!");
                }
                this.OnSaveEvent(ZipProgressEventType.Saving_AfterCompileSelfExtractor);
                try
                {
                    if (Directory.Exists(path))
                    {
                        try
                        {
                            Directory.Delete(path, true);
                        }
                        catch
                        {
                        }
                    }
                    if (File.Exists(str))
                    {
                        try
                        {
                            File.Delete(str);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
                this.OnSaveCompleted();
                if (this.Verbose)
                {
                    this.StatusMessageTextWriter.WriteLine("Created self-extracting zip file {0}.", results.PathToAssembly);
                }
            }
        }

        private string SfxSaveTemporary()
        {
            string path = Path.Combine(this.TempFileFolder, Path.GetRandomFileName() + ".zip");
            Stream outstream = null;
            try
            {
                bool flag = this._contentsChanged;
                outstream = new FileStream(path, FileMode.CreateNew);
                if (outstream == null)
                {
                    throw new BadStateException(string.Format("Cannot open the temporary file ({0}) for writing.", path));
                }
                if (this.Verbose)
                {
                    this.StatusMessageTextWriter.WriteLine("Saving temp zip file....");
                }
                int current = 0;
                foreach (ZipEntry entry in this._entries)
                {
                    this.OnSaveEntry(current, entry, true);
                    entry.Write(outstream);
                    current++;
                    this.OnSaveEntry(current, entry, false);
                    if (this._saveOperationCanceled)
                    {
                        break;
                    }
                }
                if (!this._saveOperationCanceled)
                {
                    this.WriteCentralDirectoryStructure(outstream);
                    outstream.Close();
                    outstream = null;
                }
                this._contentsChanged = flag;
            }
            finally
            {
                if (outstream != null)
                {
                    try
                    {
                        outstream.Close();
                    }
                    catch
                    {
                    }
                    try
                    {
                        outstream.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
            return path;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public ZipEntry UpdateDirectory(string directoryName)
        {
            return this.UpdateDirectory(directoryName, null);
        }

        public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
        {
            return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
        }

        public ZipEntry UpdateFile(string fileName)
        {
            return this.UpdateFile(fileName, null);
        }

        public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
        {
            string str = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
            if (this[str] != null)
            {
                this.RemoveEntry(str);
            }
            return this.AddFile(fileName, directoryPathInArchive);
        }

        public ZipEntry UpdateFileStream(string fileName, string directoryPathInArchive, Stream stream)
        {
            string str = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
            if (this[str] != null)
            {
                this.RemoveEntry(str);
            }
            return this.AddFileStream(fileName, directoryPathInArchive, stream);
        }

        public void UpdateItem(string itemName)
        {
            this.UpdateItem(itemName, null);
        }

        public void UpdateItem(string itemName, string directoryPathInArchive)
        {
            if (File.Exists(itemName))
            {
                this.UpdateFile(itemName, directoryPathInArchive);
            }
            else
            {
                if (!Directory.Exists(itemName))
                {
                    throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", itemName));
                }
                this.UpdateDirectory(itemName, directoryPathInArchive);
            }
        }

        private static uint VerifyBeginningOfZipFile(Stream s)
        {
            uint num = (uint) SharedUtilities.ReadInt(s);
            if ((((num != 0x30304b50) && (num != 0x4034b50)) && (num != 0x6054b50)) && ((num & 0xffff) != 0x5a4d))
            {
                throw new BadReadException(string.Format("  ZipFile::Read(): Bad signature (0x{0:X8}) at start of file at position 0x{1:X8}", num, s.Position));
            }
            return num;
        }

        private void WriteCentralDirectoryFooter(Stream s, long StartOfCentralDirectory, long EndOfCentralDirectory)
        {
            int index = 0;
            int num2 = 0x18;
            byte[] bytes = null;
            short length = 0;
            if ((this.Comment != null) && (this.Comment.Length != 0))
            {
                bytes = this.ProvisionalAlternateEncoding.GetBytes(this.Comment);
                length = (short) bytes.Length;
            }
            num2 += length;
            byte[] buffer2 = new byte[num2];
            int count = 0;
            buffer2[count++] = 80;
            buffer2[count++] = 0x4b;
            buffer2[count++] = 5;
            buffer2[count++] = 6;
            buffer2[count++] = 0;
            buffer2[count++] = 0;
            buffer2[count++] = 0;
            buffer2[count++] = 0;
            if ((this._entries.Count >= 0xffff) || (this._zip64 == Zip64Option.Always))
            {
                for (index = 0; index < 4; index++)
                {
                    buffer2[count++] = 0xff;
                }
            }
            else
            {
                buffer2[count++] = (byte) (this._entries.Count & 0xff);
                buffer2[count++] = (byte) ((this._entries.Count & 0xff00) >> 8);
                buffer2[count++] = (byte) (this._entries.Count & 0xff);
                buffer2[count++] = (byte) ((this._entries.Count & 0xff00) >> 8);
            }
            long num5 = EndOfCentralDirectory - StartOfCentralDirectory;
            if ((num5 >= 0xffffffffL) || (StartOfCentralDirectory >= 0xffffffffL))
            {
                for (index = 0; index < 8; index++)
                {
                    buffer2[count++] = 0xff;
                }
            }
            else
            {
                buffer2[count++] = (byte) (num5 & 0xffL);
                buffer2[count++] = (byte) ((num5 & 0xff00L) >> 8);
                buffer2[count++] = (byte) ((num5 & 0xff0000L) >> 0x10);
                buffer2[count++] = (byte) ((num5 & 0xff000000L) >> 0x18);
                buffer2[count++] = (byte) (StartOfCentralDirectory & 0xffL);
                buffer2[count++] = (byte) ((StartOfCentralDirectory & 0xff00L) >> 8);
                buffer2[count++] = (byte) ((StartOfCentralDirectory & 0xff0000L) >> 0x10);
                buffer2[count++] = (byte) ((StartOfCentralDirectory & 0xff000000L) >> 0x18);
            }
            if ((this.Comment == null) || (this.Comment.Length == 0))
            {
                buffer2[count++] = 0;
                buffer2[count++] = 0;
            }
            else
            {
                if (((length + count) + 2) > buffer2.Length)
                {
                    length = (short) ((buffer2.Length - count) - 2);
                }
                buffer2[count++] = (byte) (length & 0xff);
                buffer2[count++] = (byte) ((length & 0xff00) >> 8);
                if (length != 0)
                {
                    index = 0;
                    while ((index < length) && ((count + index) < buffer2.Length))
                    {
                        buffer2[count + index] = bytes[index];
                        index++;
                    }
                    count += index;
                }
            }
            s.Write(buffer2, 0, count);
        }

        private void WriteCentralDirectoryStructure(Stream s)
        {
            CountingStream stream = s as CountingStream;
            long startOfCentralDirectory = (stream != null) ? stream.BytesWritten : s.Position;
            foreach (ZipEntry entry in this._entries)
            {
                entry.WriteCentralDirectoryEntry(s);
            }
            long endOfCentralDirectory = (stream != null) ? stream.BytesWritten : s.Position;
            long num3 = endOfCentralDirectory - startOfCentralDirectory;
            if ((((this._zip64 == Zip64Option.Always) || (this._entries.Count >= 0xffff)) || (num3 > 0xffffffffL)) || (startOfCentralDirectory > 0xffffffffL))
            {
                this.WriteZip64EndOfCentralDirectory(s, startOfCentralDirectory, endOfCentralDirectory);
            }
            this.WriteCentralDirectoryFooter(s, startOfCentralDirectory, endOfCentralDirectory);
        }

        private void WriteZip64EndOfCentralDirectory(Stream s, long StartOfCentralDirectory, long EndOfCentralDirectory)
        {
            int num = 0x4c;
            byte[] destinationArray = new byte[num];
            int destinationIndex = 0;
            destinationArray[destinationIndex++] = 80;
            destinationArray[destinationIndex++] = 0x4b;
            destinationArray[destinationIndex++] = 6;
            destinationArray[destinationIndex++] = 6;
            long num3 = 0x2cL;
            Array.Copy(BitConverter.GetBytes(num3), 0, destinationArray, destinationIndex, 8);
            destinationIndex += 8;
            destinationArray[destinationIndex++] = 0x2d;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0x2d;
            destinationArray[destinationIndex++] = 0;
            for (int i = 0; i < 8; i++)
            {
                destinationArray[destinationIndex++] = 0;
            }
            long count = this._entries.Count;
            Array.Copy(BitConverter.GetBytes(count), 0, destinationArray, destinationIndex, 8);
            destinationIndex += 8;
            Array.Copy(BitConverter.GetBytes(count), 0, destinationArray, destinationIndex, 8);
            destinationIndex += 8;
            long num6 = EndOfCentralDirectory - StartOfCentralDirectory;
            Array.Copy(BitConverter.GetBytes(num6), 0, destinationArray, destinationIndex, 8);
            destinationIndex += 8;
            Array.Copy(BitConverter.GetBytes(StartOfCentralDirectory), 0, destinationArray, destinationIndex, 8);
            destinationIndex += 8;
            destinationArray[destinationIndex++] = 80;
            destinationArray[destinationIndex++] = 0x4b;
            destinationArray[destinationIndex++] = 6;
            destinationArray[destinationIndex++] = 7;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0;
            Array.Copy(BitConverter.GetBytes(EndOfCentralDirectory), 0, destinationArray, destinationIndex, 8);
            destinationIndex += 8;
            destinationArray[destinationIndex++] = 1;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0;
            destinationArray[destinationIndex++] = 0;
            s.Write(destinationArray, 0, destinationIndex);
        }

        private static void Zip64SeekToCentralDirectory(Stream s)
        {
            byte[] buffer = new byte[0x10];
            s.Seek(-40L, SeekOrigin.Current);
            s.Read(buffer, 0, 0x10);
            long offset = BitConverter.ToInt64(buffer, 8);
            s.Seek(offset, SeekOrigin.Begin);
            uint num2 = (uint) SharedUtilities.ReadInt(s);
            if (num2 != 0x6064b50)
            {
                throw new BadReadException(string.Format("  ZipFile::Read(): Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", num2, s.Position));
            }
            s.Read(buffer, 0, 8);
            buffer = new byte[BitConverter.ToInt64(buffer, 0)];
            s.Read(buffer, 0, buffer.Length);
            offset = BitConverter.ToInt64(buffer, 0x24);
            s.Seek(offset, SeekOrigin.Begin);
        }

        private string ArchiveNameForEvent
        {
            get
            {
                return ((this._name != null) ? this._name : "(stream)");
            }
        }

        public bool CaseSensitiveRetrieval
        {
            get
            {
                return this._CaseSensitiveRetrieval;
            }
            set
            {
                this._CaseSensitiveRetrieval = value;
            }
        }

        public string Comment
        {
            get
            {
                return this._Comment;
            }
            set
            {
                this._Comment = value;
                this._contentsChanged = true;
            }
        }

        public Ionic.Zlib.CompressionLevel CompressionLevel
        {
            get { return this.CompressionLevel; }
            set { this.CompressionLevel = value; }
        }

        public int Count
        {
            get
            {
                return this._entries.Count;
            }
        }

        public EncryptionAlgorithm Encryption
        {
            get { return this.Encryption; }
            set { this.Encryption = value;}
        }

        public ReadOnlyCollection<ZipEntry> Entries
        {
            get
            {
                return this._entries.AsReadOnly();
            }
        }

        public ReadOnlyCollection<string> EntryFileNames
        {
            get
            {
                return this._entries.ConvertAll<string>(delegate (ZipEntry e) {
                    return e.FileName;
                }).AsReadOnly();
            }
        }

        public bool ForceNoCompression
        {
            get
            {
                return this._ForceNoCompression;
            }
            set
            {
                this._ForceNoCompression = value;
            }
        }

        public ZipEntry this[int ix]
        {
            get
            {
                return this._entries[ix];
            }
            set
            {
                if (value != null)
                {
                    throw new ArgumentException("You may not set this to a non-null ZipEntry value.");
                }
                this.RemoveEntry(this._entries[ix]);
            }
        }

        public ZipEntry this[string fileName]
        {
            get
            {
                foreach (ZipEntry entry in this._entries)
                {
                    string str;
                    if (this.CaseSensitiveRetrieval)
                    {
                        if (entry.FileName == fileName)
                        {
                            return entry;
                        }
                        if (fileName.Replace(@"\", "/") == entry.FileName)
                        {
                            return entry;
                        }
                        if (entry.FileName.Replace(@"\", "/") == fileName)
                        {
                            return entry;
                        }
                        if (entry.FileName.EndsWith("/"))
                        {
                            str = entry.FileName.Trim("/".ToCharArray());
                            if (str == fileName)
                            {
                                return entry;
                            }
                            if (fileName.Replace(@"\", "/") == str)
                            {
                                return entry;
                            }
                            if (str.Replace(@"\", "/") == fileName)
                            {
                                return entry;
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(entry.FileName, fileName, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            return entry;
                        }
                        if (string.Compare(fileName.Replace(@"\", "/"), entry.FileName, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            return entry;
                        }
                        if (string.Compare(entry.FileName.Replace(@"\", "/"), fileName, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            return entry;
                        }
                        if (entry.FileName.EndsWith("/"))
                        {
                            str = entry.FileName.Trim("/".ToCharArray());
                            if (string.Compare(str, fileName, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                return entry;
                            }
                            if (string.Compare(fileName.Replace(@"\", "/"), str, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                return entry;
                            }
                            if (string.Compare(str.Replace(@"\", "/"), fileName, StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                return entry;
                            }
                        }
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    throw new ArgumentException("You may not set this to a non-null ZipEntry value.");
                }
                this.RemoveEntry(fileName);
            }
        }

        private long LengthOfReadStream
        {
            get
            {
                if (this._lengthOfReadStream == -99L)
                {
                    if (this._ReadStreamIsOurs)
                    {
                        FileInfo info = new FileInfo(this._name);
                        this._lengthOfReadStream = info.Length;
                    }
                    else
                    {
                        this._lengthOfReadStream = -1L;
                    }
                }
                return this._lengthOfReadStream;
            }
        }

        public static Version LibraryVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string Password
        {
            set
            {
                this._Password = value;
                if (this._Password == null)
                {
                    this.Encryption = EncryptionAlgorithm.None;
                }
                else if (this.Encryption == EncryptionAlgorithm.None)
                {
                    this.Encryption = EncryptionAlgorithm.PkzipWeak;
                }
            }
        }

        public Encoding ProvisionalAlternateEncoding
        {
            get
            {
                return this._provisionalAlternateEncoding;
            }
            set
            {
                this._provisionalAlternateEncoding = value;
            }
        }

        internal Stream ReadStream
        {
            get
            {
                if ((this._readstream == null) && (this._name != null))
                {
                    try
                    {
                        this._readstream = File.OpenRead(this._name);
                        this._ReadStreamIsOurs = true;
                    }
                    catch (IOException exception)
                    {
                        throw new ZipException("Error opening the file", exception);
                    }
                }
                return this._readstream;
            }
        }

        public TextWriter StatusMessageTextWriter
        {
            get
            {
                return this._StatusMessageTextWriter;
            }
            set
            {
                this._StatusMessageTextWriter = value;
            }
        }

        public string TempFileFolder
        {
            get
            {
                if (this._TempFileFolder == null)
                {
                    this._TempFileFolder = Path.GetTempPath();
                    if (this._TempFileFolder == null)
                    {
                        this._TempFileFolder = ".";
                    }
                }
                return this._TempFileFolder;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("You may not set the TempFileFolder to a null value.");
                }
                if (!Directory.Exists(value))
                {
                    throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", value));
                }
                this._TempFileFolder = value;
            }
        }

        public bool UseUnicodeAsNecessary
        {
            get
            {
                return (this._provisionalAlternateEncoding == Encoding.GetEncoding("UTF-8"));
            }
            set
            {
                this._provisionalAlternateEncoding = value ? Encoding.GetEncoding("UTF-8") : DefaultEncoding;
            }
        }

        public Zip64Option UseZip64WhenSaving
        {
            get
            {
                return this._zip64;
            }
            set
            {
                this._zip64 = value;
            }
        }

        private bool Verbose
        {
            get
            {
                return (this._StatusMessageTextWriter != null);
            }
        }

        public WantCompressionCallback WantCompression
        {
            get
            {
                return this.WantCompression;
            }
            set
            {
                this.WantCompression = value;
            }
        }

        public ReReadApprovalCallback WillReadTwiceOnInflation
        {
            get
            {
                return this.WillReadTwiceOnInflation;
            }
            set
            {
                this.WillReadTwiceOnInflation = value;
            }
        }

        private Stream WriteStream
        {
            get
            {
                if ((this._writestream == null) && (this._name != null))
                {
                    this._temporaryFileName = (this.TempFileFolder != ".") ? Path.Combine(this.TempFileFolder, SharedUtilities.GetTempFilename()) : SharedUtilities.GetTempFilename();
                    this._writestream = new FileStream(this._temporaryFileName, FileMode.CreateNew);
                }
                return this._writestream;
            }
            set
            {
                if (value != null)
                {
                    throw new ZipException("Whoa!", new ArgumentException("Cannot set the stream to a non-null value.", "value"));
                }
                this._writestream = null;
            }
        }


        private class ExtractorSettings
        {
            public List<string> CopyThroughResources;
            public SelfExtractorFlavor Flavor;
            public List<string> ReferencedAssemblies;
            public List<string> ResourcesToCompile;
        }
    }
}

