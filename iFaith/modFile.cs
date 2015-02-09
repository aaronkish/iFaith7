namespace iFaith
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    //using iFaith.My;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Microsoft.Win32;
    using Ionic.Zip;
    

    [StandardModule]
    internal sealed class modFile
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_CLIENTEDGE = 0x200;

        public static string BuildFilter(string strExtension)
        {
            string str = "";
            if (strExtension.PadLeft(1) != ".")
            {
                return ("(*." + strExtension + ")|*." + strExtension);
            }
            if (strExtension.PadLeft(1) == ".")
            {
                str = "(*" + strExtension + ")|*" + strExtension;
            }
            return str;
        }

        public static bool Create_Directory(string strDirectoryName, [Optional, DefaultParameterValue("")] ref string strError)
        {
            bool flag2 = false;
            if (strDirectoryName.Length != 0)
            {
                try
                {
                    if (!Directory_Exists(strDirectoryName))
                    {
                        Directory.CreateDirectory(strDirectoryName);
                        flag2 = true;
                    }
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    strError = exception.Message;
                    flag2 = false;
                    ProjectData.ClearProjectError();
                }
            }
            return flag2;
        }

        public static bool Directory_Exists(string StrFolder)
        {
            if (StrFolder.Length == 0)
            {
                bool flag = false;
                return flag;
            }
            DirectoryInfo info = new DirectoryInfo(StrFolder);
            return info.Exists;
        }

        public static bool File_Copy(string strSource, string strDestination, bool bolOverwrite, [Optional, DefaultParameterValue("")] ref string strError)
        {
            bool flag = false;
            if (strDestination.Length != 0 && strSource.Length != 0)
            {
                try
                {
                    File.Copy(strSource, strDestination, bolOverwrite);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    strError = exception.Message;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
            }
            return flag;
        }
        

        public static bool File_Delete(string strFilename, [Optional, DefaultParameterValue("")] ref string strError)
        {
            bool flag = false;
            if (strFilename.Length != 0)
            {
                try
                {
                    File.Delete(strFilename);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    strError = exception.Message;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
            }
            return flag;
        }

        public static bool File_Exists(string strFile)
        {
            if (strFile.Length == 0)
            {
                bool flag = false;
                return flag;
            }
            FileInfo info = new FileInfo(strFile);
            return info.Exists;
        }

        public static bool File_Move(string strSource, string strDestination, [Optional, DefaultParameterValue("")] ref string strError)
        {
            bool flag = false;
            if (strDestination.Length != 0 && strSource.Length != 0)
            {
                try
                {
                    File.Move(strSource, strDestination);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    strError = exception.Message;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
            }
            return flag;
        }

        public static bool File_Rename(string strSource, string strNewName, [Optional, DefaultParameterValue("")] ref string strError)
        {
            bool flag = false;
            if (strNewName.Length != 0 && strSource.Length != 0)
            {
                try
                {
                    File.Move(strSource, strNewName);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    strError = exception.Message;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
            }
            return flag;
        }

        public static string FileOpenDialog(string strExtension, string strInitDir)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string str = BuildFilter(strExtension);
            string fileName = "";
            OpenFileDialog dialog2 = dialog;
            dialog2.Filter = "iDevice Software File (*.ipsw) |*.ipsw;";
            dialog2.DefaultExt = strExtension;
            dialog2.InitialDirectory = strInitDir;
            dialog2.ShowDialog();
            fileName = dialog2.FileName;
            dialog2 = null;
            return fileName;
        }

        public static bool Folder_Delete(string strFolderame, [Optional, DefaultParameterValue("")] ref string strError)
        {
            bool flag = false;
            if (strFolderame.Length != 0)
            {
                try
                {
                    new DirectoryInfo(strFolderame).Delete(true);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    strError = exception.Message;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
            }
            return flag;
        }

        public static string GetFileContents(string FullPath, [Optional, DefaultParameterValue("")] ref string ErrInfo)
        {
            try
            {
                StreamReader reader = new StreamReader(FullPath);
                string str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ErrInfo = exception.Message;
                ProjectData.ClearProjectError();
            }
            return "";
        }

        public static long GetFileSize(string MyFilePath)
        {
            FileInfo info = new FileInfo(MyFilePath);
            return info.Length;
        }

        [DllImport("user32", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        public static void SaveToDisk(string resourceName, string fileName)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            foreach (string str in executingAssembly.GetManifestResourceNames())
            {
                if (str.ToLower().IndexOf(resourceName.ToLower()) != -1)
                {
                    using (Stream stream = executingAssembly.GetManifestResourceStream(str))
                    {
                        if (stream != null)
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                byte[] buffer = reader.ReadBytes((int) stream.Length);
                                using (FileStream stream2 = new FileStream(fileName, FileMode.Create))
                                {
                                    using (BinaryWriter writer = new BinaryWriter(stream2))
                                    {
                                        writer.Write(buffer);
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
            while (!File_Exists(fileName))
            {
                Application.DoEvents();
            }
        }

        public static void SetMdiClientBorder(bool showBorder)
        {
            IEnumerator enumerator = null;
            try
            {
                MDIMain objMDI = new MDIMain();
                enumerator = objMDI.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Control current = (Control) enumerator.Current;
                    if (current is MdiClient)
                    {
                        long windowLong = GetWindowLong(current.Handle, -20);
                        if (showBorder)
                        {
                            windowLong |= 0x200L;
                        }
                        else
                        {
                            windowLong &= -513L;
                        }
                        SetWindowLong(current.Handle, -20, windowLong);
                        current.Width++;
                        return;
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
        }

        [DllImport("user32", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);
    }
}

