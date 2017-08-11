using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WinFormsAppFor157Recommend
{
    public class Tip10Compare
    {
        public void CompareUseDefault()
        {
            ArrayList companySalary = new ArrayList();
            companySalary.Add(new Salary() { Name = "Mike", BaseSalary = 3000 });
            companySalary.Add(new Salary() { Name = "Rose", BaseSalary = 2000 });
            companySalary.Add(new Salary() { Name = "Jeffry", BaseSalary = 1000 });
            companySalary.Add(new Salary() { Name = "Steve", BaseSalary = 4000 });
            companySalary.Sort();
            foreach (Salary item in companySalary)
            {
                Console.WriteLine(item.Name + "\t BaseSalary: " + item.BaseSalary.ToString());
            }
        }
        public void CompareUseCustom()
        {
            ArrayList companySalary = new ArrayList();
            companySalary.Add(new Salary() { Name = "Mike", BaseSalary = 3000, Bonus = 1000 });
            companySalary.Add(new Salary() { Name = "Rose", BaseSalary = 2000, Bonus = 4000 });
            companySalary.Add(new Salary() { Name = "Jeffry", BaseSalary = 1000, Bonus = 6000 });
            companySalary.Add(new Salary() { Name = "Steve", BaseSalary = 4000, Bonus = 3000 });
            companySalary.Sort(new BonusComparer());    //提供一个非默认的比较器
            foreach (Salary item in companySalary)
            {
                Console.WriteLine(string.Format("Name:{0} \tBaseSalary:{1} \tBonus:{2}", item.Name, item.BaseSalary, item.Bonus));
            }
        }
        /// <summary>
        /// 尽量不适用非泛型集合类，因为在使用非泛型集合类的过程中可能会发生转型，影响性能
        /// </summary>
        /// <param name="args"></param>
        public void CompareUseGeneric(string[] args)
        {
            List<SalaryGeneric> companySalary = new List<SalaryGeneric>()
                {
                    new SalaryGeneric() { Name = "Mike", BaseSalary = 3000, Bonus = 1000 },
                    new SalaryGeneric() { Name = "Rose", BaseSalary = 2000, Bonus = 4000 },
                    new SalaryGeneric() { Name = "Jeffry", BaseSalary = 1000, Bonus = 6000 },
                    new SalaryGeneric() { Name = "Steve", BaseSalary = 4000, Bonus = 3000 }
                };
            companySalary.Sort(new BonusGenericComparer());    //提供一个非默认的比较器
            foreach (SalaryGeneric item in companySalary)
            {
                Console.WriteLine(string.Format("Name:{0} \tBaseSalary:{1} \tBonus:{2}", item.Name, item.BaseSalary, item.Bonus));
            }
        }
    }

    class Salary : IComparable
    {
        public string Name { get; set; }
        public int BaseSalary { get; set; }
        public int Bonus { get; set; }

        #region IComparable 成员

        public int CompareTo(object obj)
        {
            Salary staff = obj as Salary;
            if (BaseSalary > staff.BaseSalary)
            {
                return 1;
            }
            else if (BaseSalary == staff.BaseSalary)
            {
                return 0;
            }
            else
            {
                return -1;
            }
            //return BaseSalary.CompareTo(staff.BaseSalary);
            //为了更好的说明CompareTo的原理，不使用这条语句,基元类型已经内置了比较器
        }

        #endregion
    }

    class BonusComparer : IComparer
    {
        #region IComparer 成员
        //发生转型，影响性能
        public int Compare(object x, object y)
        {
            Salary s1 = x as Salary;
            Salary s2 = y as Salary;
            return s1.Bonus.CompareTo(s2.Bonus);
        }

        #endregion
    }
    class SalaryGeneric : IComparable<SalaryGeneric>
    {
        public string Name { get; set; }
        public int BaseSalary { get; set; }
        public int Bonus { get; set; }

        #region IComparable<Salary> 成员

        public int CompareTo(SalaryGeneric other)
        {
            return BaseSalary.CompareTo(other.BaseSalary);
        }

        #endregion
    }

    class BonusGenericComparer : IComparer<SalaryGeneric>
    {
        #region IComparer<Salary> 成员

        public int Compare(SalaryGeneric x, SalaryGeneric y)
        {
            return x.Bonus.CompareTo(y.Bonus);
        }

        #endregion
    }
}
