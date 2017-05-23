using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Where.Input
{
    public static class Roller
    {
        public static void Init()
        {
            Engine.Engine.Window.Mouse.ButtonDown += (obj, arg) => leftButton = true;
            Engine.Engine.Window.Mouse.ButtonUp += (obj, arg) => leftButton = false;


            Engine.Engine.Window.Mouse.Move += (obj, arg) =>
            {
                mouseXDelta = arg.XDelta;
                mouseYDelta = arg.YDelta;
            };
        }

        public static float XDelta
        {
            get
            {
                var ret = leftButton? mouseXDelta : 0;
                mouseXDelta = 0;
                return -ret;
            }
        }

        public static float YDelta
        {
            get
            {
                var ret = leftButton ? mouseYDelta : 0;
                mouseYDelta = 0;
                return -ret;
            }
        }

        static float mouseXDelta, mouseYDelta;
        static bool leftButton = false;
    }
}
