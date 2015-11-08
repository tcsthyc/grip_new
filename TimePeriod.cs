using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrippingTest
{
    public class TimePeriod
    {
        public float start{get;set;}
        public float end { get; set; }

        public TimePeriod(float start, float end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
