using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapGen;
using OpenTK;
using OpenTK.Graphics.ES20;
using Where.Renderer.Lower;

namespace Where.Renderer.Renderer3D
{
    public class Renderer3D : IRenderer
    {
        public Renderer3D()
        {
            objectDrawLocs.Vertex = objectDraw.GetAttributionLocation("Vertex");
            objectDrawLocs.Camera = objectDraw.GetUniformLocation("Camera");
            objectDrawLocs.TexCoord = objectDraw.GetAttributionLocation("TexCoordInput");
            objectDrawLocs.EyePos = objectDraw.GetUniformLocation("EyePos");
            objectDrawLocs.Time = objectDraw.GetUniformLocation("Time");
            objectDraw.EnableAttribute(objectDrawLocs.Vertex);
            objectDraw.EnableAttribute(objectDrawLocs.TexCoord);
            objectDraw.Use();
            objectDraw.SetUniform("Surface", 0);
            objectDraw.SetUniform("Height", 1);
            objectDraw.SetUniform("Normal", 2);
            objectDraw.SetUniform("Cloud", 3);
            GL.UseProgram(0);

            cloudNoise.BindTo0AndLoadImage("Cloud");

            wallMateria = new Materia("wall");
            earthMateria = new Materia("grass");
        }
        public void OnDraw()
        {
            time += 1;
            GL.Viewport(0, 0, Engine.Engine.Window.Width, Engine.Engine.Window.Height);

            GL.Enable(EnableCap.DepthTest);

            cloudNoise.Bind(3);

            objectDraw.Use();
            objectDraw.SetUniform(objectDrawLocs.Time,time);
            earthMateria.Bind();
            earth.OnDraw(objectDrawLocs);

            wallMateria.Bind();
            wall.OnDraw(objectDrawLocs);

            GL.Disable(EnableCap.DepthTest);

            GL.Viewport(0, 0, Engine.Engine.Window.Width / 4, Engine.Engine.Window.Height / 4);
            renderer2d.OnDraw();
        }

        public void SetCamera(float angle, Vector2 pos)
        {
            renderer2d.SetCamera(angle, pos);

            var camera = Matrix4.Identity;

            camera *= Matrix4.CreateTranslation(new Vector3(-21.0F * pos.X, -20.0F, 21.0F * pos.Y));
            camera *= Matrix4.CreateRotationY((float)((angle + 180) * Math.PI / 180));

            camera *= Matrix4.CreatePerspectiveFieldOfView((float)Math.PI/1.7F, Engine.Engine.Window.Height / ((float)Engine.Engine.Window.Width), 0.1F, 1000.0F);



            objectDraw.Use();
            objectDraw.SetUniform(objectDrawLocs.Camera, ref camera);
            //objectDraw.SetUniform(objectDrawLocs.EyePos,);
        }

        public void SetWallBuffer(List<Point> wallPoints, Point targetPoint)
        {
            renderer2d.SetWallBuffer(wallPoints, targetPoint);
            wall = new Wall(wallPoints);
        }

        Renderer2D.Renderer2D renderer2d = new Renderer2D.Renderer2D();
        Earth earth = new Earth();
        GLShader objectDraw = new GLShader("VertexShader", "3D_ObjectDraw");

        public struct ObjectDrawShaderLocs
        {
            public int
                Vertex,
                Camera,
                TexCoord,
                EyePos,
                Time;
        }
        ObjectDrawShaderLocs objectDrawLocs;

        Wall wall;
        float time = 0;

        GLTexture cloudNoise = new GLTexture();
        readonly Materia wallMateria, earthMateria;
    }
}
