using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// 委托是方法指针
    /// 委托是一个类，进行实例化时要将引用方法作为它的构造方法的参数
    /// </summary>
    public delegate T GetEmployeeNoOutHandler<T>(string name);
    public delegate T GetEmployeeHandler<out T>(string name);//out让类型参数T支持可变性
    /// <summary>
    /// 泛型类和非泛型类最主要的不同：类型参数化。
    /// 类型定义时，将指定类型形参（Type Parameter(T)）,紧随类名，并包含在<>符号内
    /// </summary>
    public class CovariantContravariantClass
    {
        public void PrintNoOut()
        {
            ISalaryNoOut<Programmer> sNoOut = new BaseCounterNoOut<Programmer>();
            ISalaryNoOut<Employee> sENoOut = new BaseCounterNoOut<Employee>();
            //PrintSalaryNoOut(sNoOut);无法通过编译,当T指定了一种类型就只能使用该类型传递参数
            PrintSalaryNoOut(sENoOut);
            PrintSalaryNoOut<Programmer>(sNoOut);
            // PrintSalaryNoOut<Employee>(sNoOut);//无法编译
            PrintSalaryNoOut<Employee>(sENoOut);
        }
        static void PrintSalaryNoOut(ISalaryNoOut<Employee> s)
        {
            s.Pay();
        }
        static void PrintSalaryNoOut<T>(ISalaryNoOut<T> s)
        {
            s.Pay();
        }
        /// <summary>
        /// out-泛型参数支持协变
        /// </summary>
        public void PayUseOut()
        {
            ISalary<Programmer> sP = new BaseCounter<Programmer>();
            ISalary<Employee> sE=new BaseCounter<Programmer>();
            PrintSalary(sP);
            PrintSalary(sE);
            PrintSalary<Employee>(sE);
            //PrintSalary<Programmer>(sE);Error
            PrintSalary<Programmer>(sP);
        }
        static void PrintSalary(ISalary<Employee> s)
        {
            s.Pay();
            s.PayT();
        }
        static void PrintSalary<T>(ISalary<T> s) //泛型参数<T>兼容泛型接口参数T的不可变性
        {
            s.Pay();
            s.PayT();
        }
        /// <summary>
        /// 协变
        /// </summary>
        public void DelegateCovariant()
        {
            GetEmployeeHandler<Employee> getEmployee = GetAManager;
            GetEmployeeHandler<Manager> getManager = GetAManager;
            GetEmployeeNoOutHandler<Employee> getEmployeeNoOUt = GetAManager;
            GetEmployeeNoOutHandler<Manager> getEmployeeNoOUt2 = GetAManager;
            getEmployee("Make");
            getManager("pzi");
            getEmployeeNoOUt("dd");
            getEmployeeNoOUt2("tt");
        }
        public Manager GetAManager(string name)
        {
            Console.WriteLine("I'm manager:" + name);
            return new Manager() { Name = name };
        }
        public Employee GetAEmployee(string name)
        {
            Console.WriteLine("I'm employee:" + name);
            return new Employee() { Name = name };
        }
        static void Test<T>(IMyComparable<T> m, T n)
        {
            //default
        }
        public void TestIn()
        {
            ProgrammerIn programmer = new ProgrammerIn() { Name = "Make" };
            ManagerIn manager = new ManagerIn() { Name = "pzi" };
            Test(programmer, manager);//如果不为IMyComparable接口的泛型参数T指定in关键字，将会导致编译出错
        }
    }
    /// <summary>
    /// 类型参数T不支持协变
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ISalaryNoOut<T>
    {
        void Pay();
    }
    public class BaseCounterNoOut<T> : ISalaryNoOut<T>
    {
        public void Pay()
        {
            Console.WriteLine("Pay Salary Counter! ");
        }
    }
    public class Employee
    {
        public int NameId { get; set; }
        public string Name { get; set; }
    }
    public class Programmer : Employee
    {

    }
    public class Manager : Employee
    {

    }
    /// <summary>
    /// 协变：让返回值类型返回比声明的类型派生程度更大的类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface ISalary<out T> //out让类型参数T支持可变性，增大了接口的使用范围
    {
        //void PayX(T t);当一个泛型类型被out修饰时，不能作为方法的输入参数，只能作为返回值
        T PayT();
        void Pay();
        
    }
    public class BaseCounter<T> : ISalary<T>
    {
        public void Pay()
        {
            Console.WriteLine("Pay Salary Counter! ");
        }

        public T PayT()
        {
            T t = default(T);
            Console.WriteLine("Pay Salary Counter T! ");
            return t;
        }
    }
    //接口没有逆变性
    public interface IMyComparableNoIn<T>
    {
        int ComPare(T other);
    }
    /// <summary>
    /// 逆变是指方法的参数可以是委托或泛型接口的参数类型的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMyComparable<in T>//引入接口的逆变
    {
        int ComPare(T other);
    }
    public class EmployeeIn : IMyComparable<EmployeeIn>
    {
        public string Name { get; set; }

        public int ComPare(EmployeeIn other)
        {
            return Name.CompareTo(other.Name);
        }
    }
    class ProgrammerIn : EmployeeIn, IMyComparable<ProgrammerIn>
    {
        public int ComPare(ProgrammerIn other)
        {
            return Name.CompareTo(other.Name);
        }
    }
    public class ManagerIn : EmployeeIn, IDisposable
    {
        public void Dispose()
        {

        }
    }
}
