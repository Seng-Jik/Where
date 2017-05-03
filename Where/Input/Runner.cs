using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Where.Input
{
    public static class Runner
    {
        public static void Init()
        {
            Engine.Engine.Window.Mouse.WheelChanged += (obj, arg) =>
             {
                 if (arg.Delta > 0)
                     state = StateType.Go;
                 else if (arg.Delta < 0)
                     state = StateType.Back;
                 else
                     state = StateType.Stop;
                 //Console.WriteLine(state);
             };
        }

        public enum StateType
        {
            Stop,
            Go,
            Back
        }

        static StateType state;
        public static StateType State
        {
            get
            {
                var ret = state;
                state = StateType.Stop;                
                return ret;
            }
        }
    }
}
