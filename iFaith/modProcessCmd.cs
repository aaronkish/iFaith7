namespace iFaith
{
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [StandardModule]
    internal sealed class modProcessCmd
    {
        public static string board = "nxxap";
        public static string cmdline = "";
        public const int HIGH_PRIORITY_CLASS = 0x80;
        public static string iDevice = "";
        public const short INFINITE = -1;
        public static string IPSWVersion = "";
        public const int NORMAL_PRIORITY_CLASS = 0x20;
        private const short STARTF_USESHOWWINDOW = 1;
        private const short SW_HIDE = 0;

        [DllImport("kernel32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
        public static extern int CloseHandle(int hObject);
        [DllImport("kernel32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
        public static extern int CreateProcessA(int lpApplicationName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpCommandLine, int lpProcessAttributes, int lpThreadAttributes, int bInheritHandles, int dwCreationFlags, int lpEnvironment, int lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, ref PROCESS_INFORMATION lpProcessInformation);
        public static void Delay(double dblSecs)
        {
            DateAndTime.Now.AddSeconds(1.1574074074074074E-05);
            DateTime time3 = DateAndTime.Now.AddSeconds(1.1574074074074074E-05).AddSeconds(dblSecs);
            while (DateTime.Compare(DateAndTime.Now, time3) <= 0)
            {
                if (iFaith.OhNoesShutDOWN)
                {
                    return;
                }
                Application.DoEvents();
            }
        }

        public static int ExecCmd(string cmdline, [Optional, DefaultParameterValue(false)] bool HideWindow)
        {
            PROCESS_INFORMATION process_information = new PROCESS_INFORMATION();
            int num2=0;
            STARTUPINFO expression = new STARTUPINFO();
            if (HideWindow)
            {
                expression.dwFlags = 1;
                expression.wShowWindow = 0;
            }
            expression.cb = Strings.Len(expression);
            short num = (short) CreateProcessA(0, ref cmdline, 0, 0, 1, 0x80, 0, 0, ref expression, ref process_information);
            do
            {
                num = (short) WaitForSingleObject(process_information.hProcess, 0);
                Application.DoEvents();
                Application.DoEvents();
            }
            while (num == 0x102);
            num = (short) CloseHandle(process_information.hProcess);
            return num2;
        }

        [DllImport("kernel32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
        public static extern void Sleep(long dwMilliseconds);
        [DllImport("kernel32", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]

        //--------------------------
        //-------------------------
        public static extern int WaitForSingleObject(int hHandle, int dwMilliseconds);

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public int hProcess;
            public int hThread;
            public int dwProcessID;
            public int dwThreadID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public int lpReserved2;
            public int hStdInput;
            public int hStdOutput;
            public int hStdError;
        }
    }
}

