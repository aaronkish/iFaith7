namespace iFaith.My
{
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.ApplicationServices;
    using Microsoft.VisualBasic.CompilerServices;
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    [HideModuleName, StandardModule, GeneratedCode("MyTemplate", "8.0.0.0")]
    internal sealed class MyProject
    {
        private static readonly ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider = new ThreadSafeObjectProvider<MyApplication>();
        private static readonly ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider = new ThreadSafeObjectProvider<MyComputer>();
        private static ThreadSafeObjectProvider<MyForms> m_MyFormsObjectProvider = new ThreadSafeObjectProvider<MyForms>();
        private static readonly ThreadSafeObjectProvider<MyWebServices> m_MyWebServicesObjectProvider = new ThreadSafeObjectProvider<MyWebServices>();
        private static readonly ThreadSafeObjectProvider<Microsoft.VisualBasic.ApplicationServices.User> m_UserObjectProvider = new ThreadSafeObjectProvider<Microsoft.VisualBasic.ApplicationServices.User>();

        [HelpKeyword("My.Application")]
        internal static MyApplication Application
        {
            [DebuggerHidden]
            get
            {
                return m_AppObjectProvider.GetInstance;
            }
        }

        [HelpKeyword("My.Computer")]
        internal static MyComputer Computer
        {
            [DebuggerHidden]
            get
            {
                return m_ComputerObjectProvider.GetInstance;
            }
        }

        [HelpKeyword("My.Forms")]
        internal static MyForms Forms
        {
            [DebuggerHidden]
            get
            {
                return m_MyFormsObjectProvider.GetInstance;
            }
        }

        [HelpKeyword("My.User")]
        internal static Microsoft.VisualBasic.ApplicationServices.User User
        {
            [DebuggerHidden]
            get
            {
                return m_UserObjectProvider.GetInstance;
            }
        }

        [HelpKeyword("My.WebServices")]
        internal static MyWebServices WebServices
        {
            [DebuggerHidden]
            get
            {
                return m_MyWebServicesObjectProvider.GetInstance;
            }
        }
        
        [MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms"), EditorBrowsable(EditorBrowsableState.Never)]
        internal sealed class MyForms
        {
            //public About m_About;
            public DFU m_dfu;
            [ThreadStatic]
            private static Hashtable m_FormBeingCreated;
            //public iAcqua m_iAcqua;
            //public iAcquaAssistant m_iAcquaAssistant;
            public MDIMain m_MDIMain;
            public Run m_Run;
            //public sn0wbreeze m_sn0wbreeze;
            //public Welcome m_Welcome;
            //public Welcome_ipsw m_Welcome_ipsw;
            /*
            [DebuggerHidden]
            private static T Create__Instance__<T>(T Instance) where T: Form, new()
            {
                T local;
                if ((Instance != null) && !Instance.IsDisposed)
                {
                    return Instance;
                }
                if (m_FormBeingCreated != null)
                {
                    if (m_FormBeingCreated.ContainsKey(typeof(T)))
                    {
                        throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate", new string[0]));
                    }
                }
                else
                {
                    m_FormBeingCreated = new Hashtable();
                }
                m_FormBeingCreated.Add(typeof(T), null);
                try
                {
                    local = Activator.CreateInstance<T>();
                }
                catch //when (?)
                {
                    TargetInvocationException exception;
                    throw new InvalidOperationException(Utils.GetResourceString("WinForms_SeeInnerException", new string[] { exception.InnerException.Message }), exception.InnerException);
                }
                finally
                {
                    m_FormBeingCreated.Remove(typeof(T));
                }
                return local;
            }
            
            [DebuggerHidden]
            private void Dispose__Instance__<T>(ref T instance) where T: Form
            {
                instance.Dispose();
                instance = default(T);
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override bool Equals(object o)
            {
                return base.Equals(RuntimeHelpers.GetObjectValue(o));
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            internal Type GetType()
            {
                return typeof(MyProject.MyForms);
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString()
            {
                return base.ToString();
            }

            public iFaith.About About
            {
                get
                {
                    this.m_About = Create__Instance__<iFaith.About>(this.m_About);
                    return this.m_About;
                }
                set
                {
                    if (value != this.m_About)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.About>(ref this.m_About);
                    }
                }
            }

            public iFaith.dfu dfu
            {
                get
                {
                    this.m_dfu = Create__Instance__<iFaith.dfu>(this.m_dfu);
                    return this.m_dfu;
                }
                set
                {
                    if (value != this.m_dfu)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.dfu>(ref this.m_dfu);
                    }
                }
            }

            public iFaith.iAcqua iAcqua
            {
                get
                {
                    this.m_iAcqua = Create__Instance__<iFaith.iAcqua>(this.m_iAcqua);
                    return this.m_iAcqua;
                }
                set
                {
                    if (value != this.m_iAcqua)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.iAcqua>(ref this.m_iAcqua);
                    }
                }
            }

            public iFaith.iAcquaAssistant iAcquaAssistant
            {
                get
                {
                    this.m_iAcquaAssistant = Create__Instance__<iFaith.iAcquaAssistant>(this.m_iAcquaAssistant);
                    return this.m_iAcquaAssistant;
                }
                set
                {
                    if (value != this.m_iAcquaAssistant)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.iAcquaAssistant>(ref this.m_iAcquaAssistant);
                    }
                }
            }

            public iFaith.MDIMain MDIMain
            {
                get
                {
                    this.m_MDIMain = Create__Instance__<iFaith.MDIMain>(this.m_MDIMain);
                    return this.m_MDIMain;
                }
                set
                {
                    if (value != this.m_MDIMain)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.MDIMain>(ref this.m_MDIMain);
                    }
                }
            }

            public iFaith.Run Run
            {
                get
                {
                    this.m_Run = Create__Instance__<iFaith.Run>(this.m_Run);
                    return this.m_Run;
                }
                set
                {
                    if (value != this.m_Run)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.Run>(ref this.m_Run);
                    }
                }
            }

            public iFaith.sn0wbreeze sn0wbreeze
            {
                get
                {
                    this.m_sn0wbreeze = Create__Instance__<iFaith.sn0wbreeze>(this.m_sn0wbreeze);
                    return this.m_sn0wbreeze;
                }
                set
                {
                    if (value != this.m_sn0wbreeze)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.sn0wbreeze>(ref this.m_sn0wbreeze);
                    }
                }
            }

            public iFaith.Welcome Welcome
            {
                get
                {
                    this.m_Welcome = Create__Instance__<iFaith.Welcome>(this.m_Welcome);
                    return this.m_Welcome;
                }
                set
                {
                    if (value != this.m_Welcome)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.Welcome>(ref this.m_Welcome);
                    }
                }
            }

            public iFaith.Welcome_ipsw Welcome_ipsw
            {
                get
                {
                    this.m_Welcome_ipsw = Create__Instance__<iFaith.Welcome_ipsw>(this.m_Welcome_ipsw);
                    return this.m_Welcome_ipsw;
                }
                set
                {
                    if (value != this.m_Welcome_ipsw)
                    {
                        if (value != null)
                        {
                            throw new ArgumentException("Property can only be set to Nothing");
                        }
                        this.Dispose__Instance__<iFaith.Welcome_ipsw>(ref this.m_Welcome_ipsw);
                    }
                }
            }
        }

        [MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", ""), EditorBrowsable(EditorBrowsableState.Never)]
        internal sealed class MyWebServices
        {
            [DebuggerHidden]
            private static T Create__Instance__<T>(T instance) where T: new()
            {
                if (instance == null)
                {
                    return Activator.CreateInstance<T>();
                }
                return instance;
            }

            [DebuggerHidden]
            private void Dispose__Instance__<T>(ref T instance)
            {
                instance = default(T);
            }

            [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
            public override bool Equals(object o)
            {
                return base.Equals(RuntimeHelpers.GetObjectValue(o));
            }

            [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            [EditorBrowsable(EditorBrowsableState.Never), DebuggerHidden]
            internal Type GetType()
            {
                return typeof(MyProject.MyWebServices);
            }

            [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString()
            {
                return base.ToString();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), ComVisible(false)]
        internal sealed class ThreadSafeObjectProvider<T> where T: new()
        {
            [CompilerGenerated, ThreadStatic]
            private static T m_ThreadStaticValue;

            internal T GetInstance
            {
                [DebuggerHidden]
                get
                {
                    if (MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue == null)
                    {
                        MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue = Activator.CreateInstance<T>();
                    }
                    return MyProject.ThreadSafeObjectProvider<T>.m_ThreadStaticValue;
                }
            }*/
        }
        
    }
}

