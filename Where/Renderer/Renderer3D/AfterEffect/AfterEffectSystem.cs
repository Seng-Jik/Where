using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES20;

namespace Where.Renderer.Renderer3D.AfterEffect
{
    class AfterEffectSystem
    {
        public AfterEffectSystem()
        {
            texHandle = GL.GenTexture();
        }

        public void OnDraw()
        {
            foreach(var i in effects)
            {
                GL.CopyTexImage2D(TextureTarget2d.Texture2D, i.DownSample, TextureCopyComponentCount.Rgba, 0, 0, Engine.Engine.Window.Width, Engine.Engine.Window.Height, 0);
            }
        }

        public void AddEffect(IAfterEffect effect)
        {
            effects.Add(effect);
        }

        int texHandle;
        List<IAfterEffect> effects;
    }
}
