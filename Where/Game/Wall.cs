using System;
using System.Collections.Generic;
using System.Text;
using MapGen;
using OpenTK;
namespace Where.Game
{
    public class Wall : GameObjectWithCollitor
    {
        public Wall(Point pos) : base(pos)
        {

        }

        protected override void OnColliWithPlayer()
        {
            var map = ((GameContext)Game.GameContext.CurrentGame.Target).Map;
            Vector2 vec = new Vector2();
            Vector2 fix = new Vector2();
            var pos = ((GameContext)(GameContext.CurrentGame.Target)).Player.Position;
            var lastpos = ((GameContext)(GameContext.CurrentGame.Target)).Player.LastPosition;
            vec.X = (pos.X - lastpos.X) * 2.5f;
            vec.Y = (pos.Y - lastpos.Y) * 2.5f;
            if ((map.BlockCells[(int)(lastpos.X + 0.5f + vec.X), (int)(lastpos.Y + 0.5f + vec.Y)] == Block.Wall ||
                map.BlockCells[(int)(lastpos.X + 0.5f + vec.X), (int)(lastpos.Y + 0.5f + vec.Y)] == Block.Border) &&
                map.BlockCells[(int)(lastpos.X + 0.5f + vec.X), (int)(lastpos.Y + 0.5f)] == Block.Empty &&
                map.BlockCells[(int)(lastpos.X + 0.5f), (int)(lastpos.Y + 0.5f + vec.Y)] == Block.Empty) 
                if (vec.X > vec.Y) vec.Y = 0;
                else vec.X = 0;
            if (map.BlockCells[(int)(lastpos.X + 0.5f + vec.X), (int)(lastpos.Y + 0.5f)] == Block.Wall ||
                     map.BlockCells[(int)(lastpos.X + 0.5f + vec.X), (int)(lastpos.Y + 0.5f)] == Block.Border) 
                    vec.X = 0;
            if (map.BlockCells[(int)(lastpos.X + 0.5f), (int)(lastpos.Y + 0.5f + vec.Y)] == Block.Wall ||
                    map.BlockCells[(int)(lastpos.X + 0.5f), (int)(lastpos.Y + 0.5f + vec.Y)] == Block.Border)
                    vec.Y = 0;
            var player = ((GameContext)(GameContext.CurrentGame.Target)).Player;
            player.Position = new Vector2(lastpos.X + vec.X / 2.5f, lastpos.Y + vec.Y / 2.5f);
        }
    }
}
