namespace Ionic.Zip
{
    using System;

    public class SaveProgressEventArgs : ZipProgressEventArgs
    {
        private int _entriesSaved;

        internal SaveProgressEventArgs()
        {
        }

        internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
        {
        }

        internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry) : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
        {
            base.EntriesTotal = entriesTotal;
            base.CurrentEntry = entry;
            this._entriesSaved = entriesSaved;
        }

        internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
        {
            SaveProgressEventArgs args = new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead);
            args.ArchiveName = archiveName;
            args.CurrentEntry = entry;
            args.BytesTransferred = bytesXferred;
            args.TotalBytesToTransfer = totalBytes;
            return args;
        }

        internal static SaveProgressEventArgs Completed(string archiveName)
        {
            return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
        }

        internal static SaveProgressEventArgs Started(string archiveName)
        {
            return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
        }

        public int EntriesSaved
        {
            get
            {
                return this._entriesSaved;
            }
        }
    }
}

