using Sporty.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporty.DataModel
{
    public partial class Exercise
    {
        public DateTime DateLocal
        {
            get
            {
                return DateTimeConverter.GetLocalDateTime(this.Date, User.LocalTimeZone);
            }
            set
            {
                this.Date = DateTimeConverter.GetUtcDateTime(value, User.LocalTimeZone);
            }

        }
    }
}
