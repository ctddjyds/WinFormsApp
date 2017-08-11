using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// 接口和抽象类的应用场景：如果对象存在多个功能相近且关系紧密的版本，使用抽象类
    /// 如果对象关系不紧密，但是若干功能拥有共同的声明，使用接口
    /// 抽象类适合于提供丰富功能的场合，接口则更倾向于提供单一的一组功能
    /// </summary>
    interface IMemberDesigner
    {
        string Name { get; set; }
        void MethodVirtual();
        void Method();
    }
    public abstract class MemberDesignerAbstract:IMemberDesigner
    {
        /// <summary>
        /// 抽象类的构造方法可见性类型不应该是public和internal应该是protected
        /// </summary>
        protected MemberDesignerAbstract()
        { }
        /// <summary>
        /// 可见字段应该都设置为属性
        /// </summary>
        protected string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name=value;
            }
        }
        public virtual void MethodVirtual()
        {
            Console.WriteLine("base MethodVirtual call");
        }
        public void Method()
        {
            Console.WriteLine("base Method call");
        }
        /// <summary>
        /// 使用多态特性代替条件语句，规避不断膨胀的条件语句遵从开闭原则
        /// </summary>
        public void Drive()
        {
            Commander commander = new StartCommander();
            commander.Execute();
            commander = new StopCommander();
            commander.Execute();
        }
    }
    public abstract class Commander
    {
        public abstract void Execute();
    }
    class StartCommander : Commander
    {
        public override void Execute()
        {
            //启动
        }
    }
    class StopCommander : Commander
    {
        public override void Execute()
        {
            //停止
        }
    }
    public class CricleMethod: MemberDesignerAbstract
    {
        /// <summary>
        /// 当类CricleMethod as为类MemberDesignerAbstract时，调用子类的override方法而不调用父类的虚方法
        /// </summary>
        public override void MethodVirtual()
        {
            Console.WriteLine("Cricle override MethodVirtual call");
        }
    }
    public class TriangleMethod: MemberDesignerAbstract
    {
        /// <summary>
        /// 由于使用了new关键字，因此父类和子类的方法以没有任何关联,父类方法被隐藏
        /// TriangleMethod as为类MemberDesignerAbstract时，调用父类的方法
        /// 访问父类则调用父类的方法，访问子类则调用子类的方法
        /// </summary>
        public new void MethodVirtual()
        {
            Console.WriteLine("Triangle new MethodVirtual call");
        }
        public new void Method()
        {
            Console.WriteLine("Triangle new Method call");
        }
    }
    public class DiamondMethod: MemberDesignerAbstract
    {
        /// <summary>
        /// 没有额外的修饰符，系统提示警告，编译器会默认添加new修饰符
        /// </summary>
        public void MethodVirtual()
        {
            Console.WriteLine("Diamond default MethodVirtual call");
        }
        public void Method()
        {
            Console.WriteLine("Diamond default Method call");
        }
    }
    /// <summary>
    /// 单例应该同时是一个sealed类型
    /// </summary>
    public sealed class RectangleMethod : MemberDesignerAbstract
    {
        private static RectangleMethod m_instance;
        private static readonly object m_objectPadLock = new object();
        public static RectangleMethod Instance
        {
            get
            {
                //return m_instance == null ? new RectangleMethod() : m_instance;//并不是线程安全的，在多线程模式下，可能产生第二个实例               
                if(m_instance==null)//采用双锁定（m_instance==null）技术保证线程安全
                {
                    lock(m_objectPadLock)//锁必须是引用对象
                    {
                        if (m_instance == null)
                            m_instance = new RectangleMethod();
                    }
                }
                return m_instance;
            }
        }
        /// <summary>
        /// 单例设计模式，最好把构造函数私有化
        /// </summary>
        private RectangleMethod()
        { }
    }
    /// <summary>
    /// 采用静态构造函数只能被执行一次且在运行库加载类成员时的特点，保证了Instance的线程安全，避免了不必要的锁检查开销
    /// </summary>
    public sealed class RectangleStaticMethod : MemberDesignerAbstract
    {
        private static readonly RectangleStaticMethod m_instance = new RectangleStaticMethod();
        public static RectangleStaticMethod Instance
        {
            get
            {
                return m_instance;
            }
        }
        static RectangleStaticMethod()
        { }
        /// <summary>
        /// 单例设计模式，最好把构造函数私有化
        /// </summary>
        private RectangleStaticMethod()
        { }
    }
}
