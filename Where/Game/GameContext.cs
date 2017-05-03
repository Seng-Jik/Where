using System;
using System.Collections.Generic;
using System.Text;

namespace Where.Game
{
    public class GameContext:Engine.GameObjectList
    {
        public GameContext(int width,int height)
        {
            var map = MapGen.MapGen.NewMap(width,height);
            MapGen.MapGen.PaintMap(map);
            List<MapGen.Point> wallPoints = new List<MapGen.Point>();
            renderer = new Renderer.Renderer2D.Renderer2D();

            OpenTK.Vector2 playerPos = new OpenTK.Vector2();
            MapGen.Point targetPos = new MapGen.Point();
            for (int y = 0;y < map.Height; ++y)
            {
                for (int x = 0; x < map.Width; ++x)
                {
                    if (map.BlockCells[x, y] == MapGen.Block.Wall || map.BlockCells[x, y] == MapGen.Block.Border)
                    {
                        wallPoints.Add(new MapGen.Point() { X = x, Y = y });
                        Objects.Add(new Wall(new MapGen.Point() { X = x, Y = y }));
                    }
                    else if (map.BlockCells[x, y] == MapGen.Block.Begin)
                        playerPos = new OpenTK.Vector2(x, y);
                    else if (map.BlockCells[x, y] == MapGen.Block.Target)
                        targetPos = new MapGen.Point() { X = x, Y = y };

                }
            }

            renderer.SetWallBuffer(wallPoints,targetPos);

            player = new Player(renderer,playerPos);
            Objects.Add(player);
            Objects.Add(new Target(targetPos));
            
            //TODO:强引用，删除GameContext时内存泄漏。
            Engine.Engine.Window.RenderFrame += OnDraw;

            CurrentGame = new WeakReference(this);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        void OnDraw(object obj,object arg)
        {
            renderer.OnDraw();
        }

        public Renderer.IRenderer Renderer { get => renderer; }
        public Player Player { get => player; }

        readonly Player player;
        readonly Renderer.IRenderer renderer;

        public static WeakReference CurrentGame { get; private set; }
    }
}
