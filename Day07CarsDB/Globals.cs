using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07CarsDB
{
    class Globals
    {
        public static Database DB { get => Database.GetInstance(); }
    }
}