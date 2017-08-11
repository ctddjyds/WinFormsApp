using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    public class Tip9Operate
    {
        public void UseOperate()
        {
            Salary mikeIncome = new Salary() { RMB = 22 };
            Salary roseIncome = new Salary() { RMB = 33 };
            //Salary familyIncome = Salary.Add(mikeIncome, roseIncome);
            Salary familyIncome = mikeIncome + roseIncome;
        }
        class Salary
        {
            public int RMB { get; set; }

            public static Salary operator +(Salary s1, Salary s2)
            {
                s2.RMB += s1.RMB;
                return s2;
            }
        }
    }   
}
