using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.ES20;
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
            Window = new GameWindow(1024, 768, new OpenTK.Graphics.GraphicsMode(32, 16, 0, 4), "Where", GameWindowFlags.Default);

            GL.Enable(EnableCap.MultisampleSgis);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            Input.Roller.Init();
            Input.Runner.Init();

            Root = new GameObjectList();
            
            Window.UpdateFrame += (obj, arg) => { GL.ClearColor(0.0F, 0.0F, 0.0F, 1.0F); GL.Clear(ClearBufferMask.ColorBufferBit); };
            Window.UpdateFrame += (obj, arg) => { Root.OnUpdate(); };
            Root.Objects.Add(new Game.GameContext(65,65));
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
