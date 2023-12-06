using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppSCD
{
    internal class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public long managerID { get; set; }
        public string email { get; set; }
    }
}
