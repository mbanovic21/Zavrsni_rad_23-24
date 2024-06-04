using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    partial class Parent
    {
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
