using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrippingTest
{
    class DataPointViewConfig
    {
        public int rangeX { get; set; }
        public int rangeY { get; set; }
        public bool showXText { get; set; }
        public bool showYText { get; set; }

        public DataPointViewConfig()
        {
            this.rangeX = 60;
            this.rangeY = 20;
            this.showXText = true;
            this.showYText = true;
        }

        public DataPointViewConfig(int rx, int ry, bool showXText, bool showYText)
        {
            this.rangeX = rx;
            this.rangeY = ry;
            this.showXText = showXText;
            this.showYText = showYText;
        }
    }
}
