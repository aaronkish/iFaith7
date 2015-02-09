namespace Ionic.Zip
{
    using System;

    public class ExtractProgressEventArgs : ZipProgressEventArgs
    {
        private int _entriesExtracted;
        private bool _overwrite;
        private string _target;

        internal ExtractProgressEventArgs()
        {
        }

        internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
        {
        }

        internal ExtractProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesExtracted, ZipEntry entry, string extractLocation, bool wantOverwrite) : base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
        {
            base.EntriesTotal = entriesTotal;
            base.CurrentEntry = entry;
            this._entriesExtracted = entriesExtracted;
            this._overwrite = wantOverwrite;
            this._target = extractLocation;
        }

        internal static ExtractProgressEventArgs AfterExtractEntry(string archiveName, ZipEntry entry, string extractLocation, bool wantOverwrite)
        {
            ExtractProgressEventArgs args = new ExtractProgressEventArgs();
            args.ArchiveName = archiveName;
            args.EventType = ZipProgressEventType.Extracting_AfterExtractEntry;
            args.CurrentEntry = entry;
            args._target = extractLocation;
            args._overwrite = wantOverwrite;
            return args;
        }

        internal static ExtractProgressEventArgs BeforeExtractEntry(string archiveName, ZipEntry entry, string extractLocation, bool wantOverwrite)
        {
            ExtractProgressEventArgs args = new ExtractProgressEventArgs();
            args.ArchiveName = archiveName;
            args.EventType = ZipProgressEventType.Extracting_BeforeExtractEntry;
            args.CurrentEntry = entry;
            args._target = extractLocation;
            args._overwrite = wantOverwrite;
            return args;
        }

        internal static ExtractProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, int bytesWritten, long totalBytes)
        {
            ExtractProgressEventArgs args = new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten);
            args.ArchiveName = archiveName;
            args.CurrentEntry = entry;
            args.BytesTransferred = bytesWritten;
            args.TotalBytesToTransfer = totalBytes;
            return args;
        }

        internal static ExtractProgressEventArgs ExtractAllCompleted(string archiveName, string extractLocation, bool wantOverwrite)
        {
            ExtractProgressEventArgs args = new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll);
            args._overwrite = wantOverwrite;
            args._target = extractLocation;
            return args;
        }

        internal static ExtractProgressEventArgs ExtractAllStarted(string archiveName, string extractLocation, bool wantOverwrite)
        {
            ExtractProgressEventArgs args = new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll);
            args._overwrite = wantOverwrite;
            args._target = extractLocation;
            return args;
        }

        public int EntriesExtracted
        {
            get
            {
                return this._entriesExtracted;
            }
        }

        public string ExtractLocation
        {
            get
            {
                return this._target;
            }
        }

        public bool Overwrite
        {
            get
            {
                return this._overwrite;
            }
        }
    }
}

