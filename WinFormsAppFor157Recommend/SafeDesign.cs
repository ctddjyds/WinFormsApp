using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace WinFormsAppFor157Recommend
{
    public class SafeDesign
    {
        /// <summary>
        /// 算术运算考虑checked
        /// </summary>
        public static void CheckNumMax()
        {
            ushort salary = 65534;
            checked
            {
                salary = (ushort)(salary + 1);
                Console.WriteLine(string.Format("第一次加薪，工资总数：{0}", salary));
                salary = (ushort)(salary + 1);
                Console.WriteLine(string.Format("第二次加薪，工资总数：{0}", salary));
            }
        }
        /// <summary>
        /// MD5不要作为密码加密
        /// </summary>
        /// <param name="args"></param>
        public static void Crypto(string[] args)
        {
            Console.WriteLine("请输入密码，按回车键结束……");
            string source = Console.ReadLine();
            if (VerifyMd5Hash(source, "D3A8E4D76A0AEF23B65D9F6D6BCB358F"))
            {
                Console.WriteLine("密码正确，准许登录系统。");
            }
            else
            {
                Console.WriteLine("密码有误，拒绝登录。");
            }

            //Console.WriteLine("开始穷举法破解用户密码……");
            //string key = string.Empty;
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //for (int i = 0; i < 9999; i++)
            //{
            //    if (VerifyMd5Hash(i.ToString(), "CF79AE6ADDBA60AD018347359BD144D2"))
            //    {
            //        key = i.ToString();
            //        break;
            //    }
            //}
            //watch.Stop();
            //Console.WriteLine("密码已破解，为：{0}，耗时{1}毫秒。", key, watch.ElapsedMilliseconds);

        }

        static string GetMd5Hash(string input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input))).Replace("-", "");
            }
        }

        //static string GetMd5Hash(string input)
        //{
        //    string hashKey = "Aa1@#$,.Klj+{>.45oP";
        //    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        //    {
        //        string hashCode = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input))).Replace("-", "") + BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(hashKey))).Replace("-", "");
        //        return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(hashCode))).Replace("-", "");
        //    }
        //}
        static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0 ? true : false;
        }
        /// <summary>
        /// MD5算法是一种最通用的HASH算法，被广泛应用在文件完整性的验证上；
        /// 通过算法求值，总能得到一个固定长度的MD5值，来验证文件是否被纂改过
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileHash(string filePath)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", "");
            }
        }

        /// <summary>
        /// SecureString:表示一个应保密的文本，它在初始化的时候就已经被加密，常用于保存密钥等机密字符串
        /// </summary>
        System.Security.SecureString m_secureString = new System.Security.SecureString();
        public void UseSecureStringMethod()
        {
            m_secureString.AppendChar('l');
            m_secureString.AppendChar('u');
            m_secureString.AppendChar('m');
            m_secureString.AppendChar('i');
            m_secureString.AppendChar('n');
            m_secureString.AppendChar('j');
            m_secureString.AppendChar('i');
            //取出机密文本
            IntPtr addr = Marshal.SecureStringToBSTR(m_secureString);
            string temp = Marshal.PtrToStringBSTR(addr);
            //使用该机密文本做一些事情
            ///=======开始清理内存
            //清理掉非托管代码中对应的内存的值
            Marshal.ZeroFreeBSTR(addr);
            //清理托管代码对应的内存的值（采用重写的方法）
            int id = GetProcessID();
            byte[] writeBytes = Encoding.Unicode.GetBytes("xxxxxx");
            IntPtr intPtr = Open(id);
            unsafe
            {
                fixed (char* c = temp)
                {
                    WriteMemory((IntPtr)c, writeBytes, writeBytes.Length);
                }
            }
            ///=======清理完毕
        }
        PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();
        public int GetProcessID()
        {
            Process p = Process.GetCurrentProcess();
            return p.Id;
        }
        public IntPtr Open(int processId)
        {
            IntPtr hProcess = IntPtr.Zero;
            hProcess = ProcessAPIHelper.OpenProcess(ProcessAccessFlags.All, false, processId);
            if (hProcess == IntPtr.Zero)
                throw new Exception("OpenProcess失º¡ì败ã¨¹");
            processInfo.hProcess = hProcess;
            processInfo.dwProcessId = processId;
            return hProcess;
        }
        public int WriteMemory(IntPtr addressBase, byte[] writeBytes, int writeLength)
        {
            int reallyWriteLength = 0;
            if (!ProcessAPIHelper.WriteProcessMemory(processInfo.hProcess, addressBase, writeBytes, writeLength, out reallyWriteLength))
            {
                throw new Exception();
            }
            return reallyWriteLength;
        }

        /// <summary>
        /// 代码访问安全性CAS(Code Acess Security)和基于角色安全性（Role-Based Security）来实现应用程序的访问权限
        /// </summary>
        public void UseSecurityMethod()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            SampleClass sample = new SampleClass();
            sample.SampleMethod();
            sample.SampleMethodSec();
            Console.WriteLine("代码成功运行...");
            //自定义主体和角色
            GenericIdentity examIdentity = new GenericIdentity("ExamUser");
            //String[] Users = { "Teacher", "Student" };
            String[] Users = { "Student" };
            GenericPrincipal MyPrincipal = new GenericPrincipal(examIdentity, Users);
            Thread.CurrentPrincipal = MyPrincipal;
            ScoreProcessor score = new ScoreProcessor();
            score.Update();
        }
        class SampleClass
        {
            public void SampleMethod()
            {
                Console.WriteLine("执行方法SampleMethod");
            }

            [PrincipalPermission(SecurityAction.Demand, Role = @"Administrator")]
            //[PrincipalPermission(SecurityAction.Demand, Role = @"Users")]
            public void SampleMethodSec()
            {
                Console.WriteLine("执行方法SampleMethodSec");
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }
    public class ProcessAPIHelper
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);
    }
    public class ScoreProcessor
    {
        public void Update()
        {
            try
            {
                PrincipalPermission MyPermission = new PrincipalPermission("ExamUser", "Teacher");
                MyPermission.Demand();
                //省略
                Console.WriteLine("修改成绩成功");
            }
            catch (SecurityException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
