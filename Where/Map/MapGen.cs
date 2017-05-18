using System;
using System.Collections.Generic;
using System.Text;


namespace MapGen
{
    public enum Block
    {
        Empty,Wall,Target,Begin,Border
    }
    public class Map
    {
        public Block[,] BlockCells;
        public int Width, Height;
    }
    public struct Point
    {
        public int X;
        public int Y;
    }
    class MapGenerator
    {

        public double NowX { get; private set; }
        public double NowY { get; private set; }

        private Map map;
        public Map MyMap{get=>map;set=>map=value;}
        private Dictionary<Point, bool> visited = new Dictionary<Point, bool>();
        private Point[] fix = new Point[4];
        public MapGenerator(int MapWidth,int MapHeight){
            fix[0].X = -1; fix[0].Y = 0;
            fix[1].X = 1; fix[1].Y = 0;
            fix[2].X = 0; fix[2].Y = -1;
            fix[3].X = 0; fix[3].Y = 1;
            if ((MapWidth - 1) % 2 == 0 && (MapHeight - 1) % 2 == 0 && MapWidth>=3 && MapHeight>=3)
            {
                map = new Map()
                {
                    BlockCells = new Block[MapWidth, MapHeight],
                    Width = MapWidth,
                    Height = MapHeight
                };
                InitMap();
                MakeMap();
            }
            else throw new Exception("An error occurred");
        }
        private void InitMap() {
            Point beg = new Point();
            Point end = new Point();
            for (int y = 0; y * 2 < map.Height; y++) 
                for (int x = 0; x < map.Width; x++)
                    map.BlockCells[x, y * 2] = Block.Wall;
            for (int x = 0; x * 2 < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    map.BlockCells[x * 2, y] = Block.Wall;
            for (int x = 0; x < map.Width; x++)
            {
                map.BlockCells[x, 0] = Block.Border;
                map.BlockCells[x, map.Height - 1] = Block.Border;
            }
            for (int y = 0; y < map.Height; y++) 
            {
                map.BlockCells[0, y] = Block.Border;
                map.BlockCells[map.Width - 1, y] = Block.Border;
            }
            do
            {
                beg.X = rnd.Next((map.Width - 1) / 2) * 2 + 1;
                beg.Y = rnd.Next((map.Height - 1) / 2) * 2 + 1;
            }while(!pointUseful(beg));
       
            map.BlockCells[beg.X, beg.Y] = Block.Begin;
            end.X = rnd.Next((map.Width - 1) / 2) * 2 + 1;
            end.Y = rnd.Next((map.Height - 1) / 2) * 2 + 1;
            while ((Math.Abs(end.X-beg.X) + Math.Abs(end.Y-beg.Y))<((map.Width+map.Height)/2) && pointUseful(end))
            {             
                end.X = rnd.Next((map.Width - 1) / 2) * 2 + 1;
                end.Y = rnd.Next((map.Height - 1) / 2) * 2 + 1;
            }
            map.BlockCells[end.X, end.Y] = Block.Target ;
        }

        Random rnd = new Random(DateTime.Now.Second);
        private void MakeMap()
        {
            List<Point> wallList = new List<Point>();
            

            Point nowBlock = new Point();
            Point tmpBlock = new Point();
            bool[,] isInway = new bool[map.Width, map.Height];
            byte[,] faceTo = new byte[map.Width, map.Height];

            nowBlock.X = rnd.Next((map.Width - 1) / 2) * 2 + 1;
            nowBlock.Y = rnd.Next((map.Height - 1) / 2) * 2 + 1;

            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    isInway[x, y] = false;

            isInway[nowBlock.X, nowBlock.Y] = true;

            SearchWall(nowBlock, ref wallList,ref faceTo);

            while (wallList.Count > 0)
            {
                rnd = new Random(rnd.Next(100));
                nowBlock = wallList[rnd.Next(wallList.Count)];
                if (!isInway[nowBlock.X + fix[faceTo[nowBlock.X, nowBlock.Y]].X, nowBlock.Y + fix[faceTo[nowBlock.X, nowBlock.Y]].Y])
                {
                    map.BlockCells[nowBlock.X, nowBlock.Y] = Block.Empty;
                    wallList.Remove(nowBlock);
                    tmpBlock.X = nowBlock.X + fix[faceTo[nowBlock.X, nowBlock.Y]].X;
                    tmpBlock.Y = nowBlock.Y + fix[faceTo[nowBlock.X, nowBlock.Y]].Y;
                    isInway[tmpBlock.X, tmpBlock.Y] = true;
                    SearchWall(tmpBlock, ref wallList, ref faceTo);
                }
                else wallList.Remove(nowBlock);
            }   
        }
        private void SearchWall(Point EmptyCell,ref List<Point> Walls,ref byte[,] Face)
        {
            Point addPoint = new Point();
            for(int n = 0; n < 4; n++)
            {
                if (EmptyCell.X + fix[n].X >= 0 && EmptyCell.X + fix[n].X < map.Width && EmptyCell.Y + fix[n].Y >= 0 && EmptyCell.Y + fix[n].Y < map.Height)
                {
                    if(map.BlockCells[EmptyCell.X + fix[n].X, EmptyCell.Y + fix[n].Y] == Block.Wall)
                    {
                        addPoint.X = EmptyCell.X + fix[n].X;
                        addPoint.Y = EmptyCell.Y + fix[n].Y;
                        Face[addPoint.X, addPoint.Y] = (byte)n;
                        Walls.Add(addPoint);
                    }
                }
            }
        }
        private bool pointUseful(Point p)
        {
            if(map.BlockCells[p.X, p.Y]==Block.Empty && 
               map.BlockCells[p.X, p.Y]!=Block.Wall && 
               map.BlockCells[p.X, p.Y]!=Block.Border)
                return true;
            return false;
        }
    }


    public static class MapGen
    {
        public static Map NewMap(int w, int h)
        {
            MapGenerator map = new MapGenerator(w, h);
            return map.MyMap;
        }

        public static void PaintMap(Map map)
        {
            StringBuilder buffer = new StringBuilder();
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    switch (map.BlockCells[x, y])
                    {
                        case Block.Border:
                            buffer.Append("■");
                            break;
                        case Block.Empty:
                            buffer.Append("  ");
                            break;
                        case Block.Wall:
                            buffer.Append("□");
                            break;
                        case Block.Target:
                            buffer.Append("●");
                            break;
                        case Block.Begin:
                            buffer.Append("●");
                            break;
                    }
                }
                buffer.Append(Environment.NewLine);
            }
            Console.Write(buffer.ToString());
        }
    }
}
