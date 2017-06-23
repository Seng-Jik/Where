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
                if (arg.XDelta > 0)
                {
                }
            };
        }

        public static float XDelta
        {
            get
            {
                var ret = leftButton ? mouseXDelta : 0;
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

        private static float mouseXDelta, mouseYDelta;
        private static bool leftButton = false;
    }
}