using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataObjects
{
    public class ParameterObject
    {
        public string Name { get; set; }
        public DbType DatabaseType { get; set; }
        public object Value { get; set; }
    }
}
