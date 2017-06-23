using OpenTK.Graphics.ES20;
using System;

namespace Where.Renderer.Lower
{
    internal class GLTexture : IDisposable
    {
        public GLTexture()
        {
            textureID = GL.GenTexture();
        }

        public void Bind(int texUnit)
        {
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texUnit));
            GL.BindTexture(TextureTarget.Texture2D, textureID);
        }

        public void BindTo0AndLoadImage(string name)
        {
            var sst = new SSTReader(new System.IO.BinaryReader(System.IO.File.OpenRead("../../../Assets/Textures/" + name + ".sst")));
            Bind(0);

            var texSize = sst.Size;
            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, (int)texSize.X, (int)texSize.Y, 0, PixelFormat.Rgba, PixelType.UnsignedByte, sst.Data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        private readonly int textureID;

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Engine.Engine.TaskToMainThread(() => GL.DeleteTexture(textureID));

                disposedValue = true;
            }
        }

        ~GLTexture()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}