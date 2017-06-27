namespace Where.Input
{
    public static class Runner
    {
        public static void Init()
        {
            Engine.Engine.Window.Mouse.WheelChanged += (obj, arg) =>
             {
                 if (arg.Delta > 0)
                 {
                     mouseWheelstate = StateType.Go;
                     MouseWheeled = true;
                 }
                 else if (arg.Delta < 0)
                 {
                     mouseWheelstate = StateType.Back;
                     MouseWheeled = true;
                 }
                 else
                     mouseWheelstate = StateType.Stop;
                 //Console.WriteLine(state);
             };
            Engine.Engine.Window.Keyboard.KeyDown += KeyDownEvent;
            Engine.Engine.Window.Keyboard.KeyUp += KeyUpEvent;
        }

        private static void KeyUpEvent(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.W:
                    up = false;
                    break;

                case OpenTK.Input.Key.S:
                    down = false;
                    break;

                case OpenTK.Input.Key.A:
                    left = false;
                    break;

                case OpenTK.Input.Key.D:
                    right = false;
                    break;
            }
            SetAngleFix();
        }

        private static void KeyDownEvent(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.W:
                    up = true;
                    break;

                case OpenTK.Input.Key.S:
                    down = true;
                    break;

                case OpenTK.Input.Key.A:
                    left = true;
                    break;

                case OpenTK.Input.Key.D:
                    right = true;
                    break;
            }
            SetAngleFix();
        }

        private static void SetAngleFix()
        {
            int i = 1, j = 1;
            if (up) j--;
            if (down) j++;
            if (left) i++;
            if (right) i--;
            AngleFix = angleFixes[i, j];
        }

        public enum StateType
        {
            Stop,
            Go,
            Back
        }

        private static StateType mouseWheelstate;

        public static StateType State
        {
            get
            {
                var ret = mouseWheelstate;
                mouseWheelstate = StateType.Stop;
                return ret;
            }
        }

        private static bool left = false, right = false, up = false, down = false;
        public static int AngleFix = 0;
        public static bool MouseWheeled { get; set; } = false;
        public static bool IsKeyDown{ get { return (left || right || up || down); } }
        private static int[,] angleFixes = { { 45, 90, 135 }, { 0, 0, 180 }, { -45, -90, -135 } };
    }
}