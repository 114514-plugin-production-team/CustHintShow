using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustHintShow
{
    public static class Hints
    {
        public class Hint
        {
            public float X { get; set; } // X 坐标（屏幕空间）
            public float Y { get; set; } // Y 坐标（屏幕空间）
            public int Size { get; set; } // 字体大小
            public string Text { get; set; } // 提示内容
        }
    }
}
