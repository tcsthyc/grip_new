using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrippingTest
{
    public class AssistLineStratogy
    {
        //策略名
        public String name;

        //当前策略所有辅助线集合
        public List<AssistLineSecion> sections { get; set; }

        public AssistLineStratogy()
        {
            this.sections = new List<AssistLineSecion>();
            this.name = "";
        }

        public String toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.name);
            sb.Append(Environment.NewLine);
            foreach (AssistLineSecion als in sections)
            {
                sb.Append(als.expression + "," + als.start + "," + als.end + "," + als.xPixelSpan + Environment.NewLine);
            }

            return sb.ToString();
        }
    
    }
}
