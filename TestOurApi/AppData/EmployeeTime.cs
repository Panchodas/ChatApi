using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOurApi.AppData
{
    public partial class Employee
    {
        public string HelloWord
        {
            get
            {
                string hello = "Доброй ночи";
                if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 12)
                {
                    hello = "Доброе утро";
                }
                else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 18)
                {
                    hello = "Добрый день";
                }
                else if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 23)
                {
                    hello = "Добрый вечер";
                }
                string salud = $"{hello}, {FullName}";
                return salud;
            }
        }
    }
}
