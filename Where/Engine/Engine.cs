using System;
using System.Collections.Generic;
using OpenTK;

namespace Where.Engine
{
    public class Engine
    {
        public static GameWindow Window { get; private set; }
        public static GameObjectList Root { get; private set; }

        public delegate void Task();
        public static void TaskToMainThread(Task task)
        {
            lock (tasks) tasks.Enqueue(task);
        }

        static void Main(string[] args)
        {
            Window = new GameWindow(1024, 768, OpenTK.Graphics.GraphicsMode.Default, "Where",GameWindowFlags.FixedWindow);

            Root = new GameObjectList();
            Window.UpdateFrame += (obj, arg) => { Root.OnUpdate(); };
            Root.Objects.Add(new Game.GameContext(63,63));
            Window.RenderFrame += (obj, arg) => { Window.SwapBuffers(); };

            Window.UpdateFrame += (obj, arg) =>
            {
                lock (tasks)
                    while (tasks.Count > 0)
                        tasks.Dequeue()();
            };
            
            Window.Run(60, 60);
        }

        static Queue<Task> tasks = new Queue<Task>();
    }
}
