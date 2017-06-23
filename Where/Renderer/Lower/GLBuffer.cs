using OpenTK.Graphics.ES20;
using System;

namespace Where.Renderer.Lower
{
    public class GLBuffer : IDisposable
    {
        public GLBuffer(BufferTarget bufferTarget)
        {
            target = bufferTarget;
            bufferHandle = GL.GenBuffer();
        }

        public void BufferData<T>(int size, T[] buffer, BufferUsageHint usage)
            where T : struct
        {
            Bind();
            GL.BufferData<T>(target, size, buffer, usage);
        }

        public void Bind()
        {
            GL.BindBuffer(target, bufferHandle);
        }

        private int bufferHandle;
        private BufferTarget target;

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Engine.Engine.TaskToMainThread(() => GL.DeleteBuffer(bufferHandle));

                disposedValue = true;
            }
        }

        ~GLBuffer()
        {
            //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}