using OpenTK.Graphics.ES20;
using System.Collections.Generic;

namespace Where.Renderer.Renderer3D.AfterEffect
{
    public class AfterEffectSystem
    {
        public void OnDraw()
        {
            foreach (var i in effects)
            {
                GL.BindTexture(TextureTarget.Texture2D, texHandle);
                GL.CopyTexImage2D(TextureTarget2d.Texture2D, i.DownSample, TextureCopyComponentCount.Rgba, 0, 0, Engine.Engine.Window.Width, Engine.Engine.Window.Height, 0);
                i.SetDrawState();

                //TODO:Render i to screen.
                //GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

                i.ResetDrawState();
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        public void AddEffect(IAfterEffect effect)
        {
            effects.Add(effect);
        }

        private int texHandle = GL.GenTexture();
        private List<IAfterEffect> effects = new List<IAfterEffect>();
    }
}