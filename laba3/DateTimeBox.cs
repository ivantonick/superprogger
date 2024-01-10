using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public class DateTimeBox
    {
        [Key]
        public required DateTime Value { get; set; }
    }
}
