namespace CFManzana
{
    using CoreFoundation;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class MobileDevice
    {
        private const string DLLPath = "iTunesMobileDevice.dll";

        static MobileDevice()
        {
            FileInfo info = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\Apple\Mobile Device Support\iTunesMobileDevice.dll");
            DirectoryInfo info2 = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + @"\Apple\Apple Application Support\");
            string directoryName = info.DirectoryName;
            if (!info.Exists)
            {
                throw new FileNotFoundException("Could not find iTunesMobileDevice file");
            }
            Environment.SetEnvironmentVariable("Path", string.Join(";", new string[] { Environment.GetEnvironmentVariable("Path"), directoryName, info2.FullName }));
        }

        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCConnectionClose(void* conn);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCConnectionInvalidate(void* conn);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCConnectionIsValid(void* conn);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCConnectionOpen(void* handle, uint io_timeout, ref void* conn);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCDirectoryClose(void* conn, void* dir);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCDirectoryCreate(void* conn, string path);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCDirectoryOpen(void* conn, byte[] path, ref void* dir);
        public static unsafe int AFCDirectoryOpen(void* conn, string path, ref void* dir)
        {
            return AFCDirectoryOpen(conn, Encoding.UTF8.GetBytes(path), ref dir);
        }

        public static unsafe int AFCDirectoryRead(void* conn, void* dir, ref string buffer)
        {
            void* dirent = null;
            int num = AFCDirectoryRead(conn, dir, ref dirent);
            if ((num == 0) && (dirent != null))
            {
                buffer = Marshal.PtrToStringAnsi(new IntPtr(dirent));
                return num;
            }
            buffer = null;
            return num;
        }

        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCDirectoryRead(void* conn, void* dir, ref void* dirent);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileInfoOpen(void* conn, string path, ref void* dict);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefClose(void* conn, long handle);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefOpen(void* conn, string path, int mode, int unknown, out long handle);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefRead(void* conn, long handle, byte[] buffer, ref uint len);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefSeek(void* conn, long handle, uint pos, uint origin);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefSetFileSize(void* conn, long handle, uint size);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefTell(void* conn, long handle, ref uint position);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFileRefWrite(void* conn, long handle, byte[] buffer, uint len);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCFlushData(void* conn, long handle);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCKeyValueClose(void* dict);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCKeyValueRead(void* dict, out void* key, out void* val);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCRemovePath(void* conn, string path);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AFCRenamePath(void* conn, string old_path, string new_path);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceActivate(void* device, IntPtr wildcard_ticket);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceConnect(void* device);
        public static unsafe string AMDeviceCopyValue(void* device, string name)
        {
            IntPtr handle = AMDeviceCopyValue_IntPtr(device, 0, (IntPtr) new CFString(name));
            if (handle == IntPtr.Zero)
            {
                return "";
            }
            return new CFType(handle).ToString();
        }

        [DllImport("iTunesMobileDevice.dll", EntryPoint="AMDeviceCopyValue", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe IntPtr AMDeviceCopyValue_IntPtr(void* device, uint unknown, IntPtr cfstring);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceDeactivate(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceDisconnect(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceGetConnectionID(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceIsPaired(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceNotificationSubscribe(DeviceNotificationCallback callback, uint unused1, uint unused2, uint unused3, out void* am_device_notification_ptr);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceStartService(void* device, IntPtr service_name, ref void* handle, void* unknown);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceStartSession(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceStopSession(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMDeviceValidatePairing(void* device);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr AMRestoreCreateDefaultOptions();
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern int AMRestoreModeDeviceCreate(uint unknown0, int connection_id, uint unknown1);
        [DllImport("iTunesMobileDevice.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern unsafe int AMRestoreRegisterForDeviceNotifications(DeviceRestoreNotificationCallback dfu_connect, DeviceRestoreNotificationCallback recovery_connect, DeviceRestoreNotificationCallback dfu_disconnect, DeviceRestoreNotificationCallback recovery_disconnect, uint unknown0, void* user_info);
        public static bool Is32BitProcessOn64BitProcessor()
        {
            bool flag;
            IsWow64Process(Process.GetCurrentProcess().Handle, out flag);
            return flag;
        }

        public static bool Is64Bit()
        {
            if ((IntPtr.Size != 8) && ((IntPtr.Size != 4) || !Is32BitProcessOn64BitProcessor()))
            {
                return false;
            }
            return true;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError=true)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, out bool lpSystemInfo);
    }
}

