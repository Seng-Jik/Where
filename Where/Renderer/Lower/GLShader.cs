using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES20;
using OpenTK;

namespace Where.Renderer.Lower
{
    public class GLShader:IDisposable
    {
        public GLShader(string vert,string frag)
        {
            var vShader = GL.CreateShader(ShaderType.VertexShader);
            var fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(vShader, File.ReadAllText("../../../Assets/Shaders/" + vert + ".vs"));
            GL.ShaderSource(fShader, File.ReadAllText("../../../Assets/Shaders/" + frag + ".fs"));
            GL.CompileShader(vShader);
            GL.CompileShader(fShader);
            var vLog = GL.GetShaderInfoLog(vShader);
            var fLog = GL.GetShaderInfoLog(fShader);
            if (vLog != "") throw new Exception(vLog);
            if (fLog != "") throw new Exception(fLog);
            programHandle = GL.CreateProgram();
            GL.AttachShader(programHandle, vShader);
            GL.AttachShader(programHandle, fShader);
            GL.LinkProgram(programHandle);
            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);
        }

        public void Use()
        {
            GL.UseProgram(programHandle);
        }

        public int GetUniformLocation(string uniform)
        {
            return GL.GetUniformLocation(programHandle,uniform);
        }

        public int GetAttributionLocation(string attrib)
        {
            return GL.GetAttribLocation(programHandle,attrib);
        }

        public void SetUniform(int loc,ref Matrix4 mat4)
        {
            GL.UniformMatrix4(loc, false, ref mat4);
        }

        public void SetUniform(int loc, float v)
        {
            GL.Uniform1(loc, v);
        }

        public void SetUniform(int loc, Vector3 v)
        {
            GL.Uniform3(loc, v);
        }

        public void SetUniform(string name,int i)
        {
            GL.Uniform1(GetUniformLocation(name), i);
        }

        public void EnableAttribute(int loc)
        {
            GL.EnableVertexAttribArray(loc);
        }

        int programHandle;

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Engine.Engine.TaskToMainThread(() => GL.DeleteProgram(programHandle));

                disposedValue = true;
            }
        }

         ~GLShader() {
           // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
           Dispose(false);
         }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
             GC.SuppressFinalize(this);
        }
        #endregion
    }
}
