using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// 如果类调用了非托管资源，或者需要显式的释放托管资源
    /// CLR对于实现了Dispose模式的类型，每次在创建该类型的对象时，都会将该对象的一个指针放到终结列表中；
    /// GC在回收该对象的内存前，会首先将终结列表中的指针放到一个freachable队列中，同时分配专门的线程读取freachable队列，并调用对象的终结器；
    /// 这时对象才真正被标记为垃圾，并且在下一次调用GC时才会释放对象占用的内存
    /// 可以看到Dispose模式的类型对象，至少需要两次GC才能真正回收掉对象内存
    /// </summary>
    public class DisposeManageClass : IDisposable
    {
        //定义一个非托管资源
        private IntPtr m_nativeResource = Marshal.AllocHGlobal(100);
        //定义一个托管资源
        private ManagerIn m_manageResource = new ManagerIn();
        private bool m_isDisposed = false;//标记资源是否被释放
        /// <summary>
        /// 显示释放方法，减少一次垃圾回收调用
        /// 实现IDisposable中的Dispose方法
        /// </summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 不是必要的，提供一个Close方法仅仅是为了更符合其他语言（如C++）的规范
        /// </summary>
        public void Close()
        {
            Dispose();
        }
        /// <summary>
        /// 析构器，必须有，防止忘记显式的调用Dispose方法
        /// </summary>
        ~DisposeManageClass()
        {
            //必须为fase
            Dispose(false);
        }
        /// <summary>
        /// 使用受保护的虚方法，考虑类继承调用
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="isDispose"></param>
        protected virtual void Dispose(bool isDispose)
        {
            if (m_isDisposed)
                return;
            //显式的调用Dispose需要手工的清理对象本身托管资源，而终结器调用不需要手工清理
            if(isDispose)
            {
                //清理托管资源
                if (m_manageResource != null)
                {
                    //对象被dispose不表示该对象为null
                    m_manageResource.Dispose();
                    m_manageResource = null;
                } 
            }
            //清理非托管资源
            if (m_nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_nativeResource);
                m_nativeResource = IntPtr.Zero;
            }
            //指示资源已释放
            m_isDisposed = true;
        }
        public void SamplePublicMethod()
        {
            if (m_isDisposed)
            {
                throw new ObjectDisposedException("SampleClass", "SampleClass is disposed");
            }
            //省略
        }
    }
    /// <summary>
    /// 基于类的继承
    /// </summary>
    public class DerivedSampleClass : DisposeManageClass
    {
        //子类的非托管资源
        private IntPtr derivedNativeResource = Marshal.AllocHGlobal(100);
        //子类的托管资源
        private ManagerIn derivedManagedResource = new ManagerIn();
        //定义自己的是否释放的标识变量
        private bool derivedDisposed = false;

        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (derivedDisposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                if (derivedManagedResource != null)
                {
                    derivedManagedResource.Dispose();
                    derivedManagedResource = null;
                }
            }
            // 清理非托管资源
            if (derivedNativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(derivedNativeResource);
                derivedNativeResource = IntPtr.Zero;
            }
            //调用父类的清理代码
            base.Dispose(disposing);
            //让类型知道自己已经被释放
            derivedDisposed = true;
        }
    }
}
