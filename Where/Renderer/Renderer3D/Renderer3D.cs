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
            Projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 1.7F, Engine.Engine.Window.Height / ((float)Engine.Engine.Window.Width), 0.1F, 1000.0F);

            objectDrawLocs.Vertex = objectDraw.GetAttributionLocation("Vertex");
            objectDrawLocs.Normal = objectDraw.GetUniformLocation("Normal");
            objectDrawLocs.Camera = objectDraw.GetUniformLocation("Camera");
            objectDrawLocs.TexCoord = objectDraw.GetAttributionLocation("TexCoordInput");
            objectDrawLocs.EyePos = objectDraw.GetUniformLocation("EyePos");
            objectDrawLocs.Time = objectDraw.GetUniformLocation("Time");
            objectDrawLocs.TBNMatrix = objectDraw.GetUniformLocation("TBNMatrix");

            objectDraw.EnableAttribute(objectDrawLocs.Vertex);
            objectDraw.EnableAttribute(objectDrawLocs.TexCoord);
            objectDraw.Use();
            objectDraw.SetUniform("Surface", 0);
            objectDraw.SetUniform("Height", 1);
            objectDraw.SetUniform("NormalMap", 2);
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
            sky.OnDraw();

            objectDraw.Use();
            objectDraw.SetUniform(objectDrawLocs.Time,time);

            

            earthMateria.Bind();
            earth.OnDraw(objectDraw,objectDrawLocs);

            //此处绘制墙体
            wallMateria.Bind();
            wall.OnDraw(objectDraw, objectDrawLocs);

            GL.Disable(EnableCap.DepthTest);

            GL.Viewport(0, 0, Engine.Engine.Window.Width / 4, Engine.Engine.Window.Height / 4);
            renderer2d.OnDraw();
        }

        public void SetCamera(float angle,float pov, Vector2 pos)
        {
            renderer2d.SetCamera(angle,pov, pos);

            var eyePos = new Vector3(-21.0F * pos.X, -20.0F, 21.0F * pos.Y);
            Camera = Matrix4.CreateTranslation(eyePos) * Matrix4.CreateRotationY((float)((angle + 180) * Math.PI / 180));

            Matrix4 camera = Camera * Projection;

            sky.SetPos(pos,this);

           

            objectDraw.Use();
            objectDraw.SetUniform(objectDrawLocs.Camera, ref camera);
            objectDraw.SetUniform(objectDrawLocs.EyePos, eyePos);
        }

        public void SetWallBuffer(List<Point> wallPoints, Point targetPoint)
        {
            renderer2d.SetWallBuffer(wallPoints, targetPoint);
            wall = new Wall(wallPoints);
        }

        public static Matrix3 GetTBNMatrix(Vector3 N,Vector3 T)
        {
            Vector3 B = OpenTK.Vector3.Cross(N, T);
            return new Matrix3(T, B, N);
        }

        public Matrix4 Projection { get; private set; }
        public Matrix4 Camera { get; private set; }

        Renderer2D.Renderer2D renderer2d = new Renderer2D.Renderer2D();
        Earth earth = new Earth();
        GLShader objectDraw = new GLShader("VertexShader", "3D_ObjectDraw");

        public struct ObjectDrawShaderLocs
        {
            public int
                Vertex,
                Normal,
                Camera,
                TexCoord,
                EyePos,
                TBNMatrix,
                Time;
        }
        ObjectDrawShaderLocs objectDrawLocs;

        Wall wall;
        float time = 0;

        GLTexture cloudNoise = new GLTexture();
        readonly Materia wallMateria, earthMateria;

        SkyBox sky = new SkyBox();


    }
}
