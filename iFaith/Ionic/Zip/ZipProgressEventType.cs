namespace Ionic.Zip
{
    using System;

    public enum ZipProgressEventType
    {
        Reading_Started,
        Reading_BeforeReadEntry,
        Reading_AfterReadEntry,
        Reading_Completed,
        Reading_ArchiveBytesRead,
        Saving_Started,
        Saving_BeforeWriteEntry,
        Saving_AfterWriteEntry,
        Saving_Completed,
        Saving_AfterSaveTempArchive,
        Saving_BeforeRenameTempArchive,
        Saving_AfterRenameTempArchive,
        Saving_AfterCompileSelfExtractor,
        Saving_EntryBytesRead,
        Extracting_BeforeExtractEntry,
        Extracting_AfterExtractEntry,
        Extracting_EntryBytesWritten,
        Extracting_BeforeExtractAll,
        Extracting_AfterExtractAll
    }
}

