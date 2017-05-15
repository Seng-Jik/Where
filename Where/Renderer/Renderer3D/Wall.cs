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
    class Wall
    {
        private void AddBox(List<Vector3> verticles,List<Vector2> texCoords,List<ushort> indicles,Point pos)
        {
            ushort begin = (ushort)(verticles.Count);

            const float BOX_SIZE = 10.5f;
            const float BOX_HEIGHT_MUL = 8.0f;

            //底部顶点
            verticles.Add(new Vector3(2*BOX_SIZE*pos.X - BOX_SIZE, 0, -2*BOX_SIZE*pos.Y - BOX_SIZE)); texCoords.Add(new Vector2(0, 0));
            verticles.Add(new Vector3(2*BOX_SIZE*pos.X + BOX_SIZE, 0, -2*BOX_SIZE*pos.Y - BOX_SIZE)); texCoords.Add(new Vector2(1, 0));
            verticles.Add(new Vector3(2*BOX_SIZE*pos.X + BOX_SIZE, 0, -2*BOX_SIZE* pos.Y+ BOX_SIZE)); texCoords.Add(new Vector2(0, 0));
            verticles.Add(new Vector3(2*BOX_SIZE*pos.X - BOX_SIZE, 0, -2*BOX_SIZE* pos.Y + BOX_SIZE)); texCoords.Add(new Vector2(1, 0));

            //顶部顶点
            verticles.Add(new Vector3(2 * BOX_SIZE * pos.X - BOX_SIZE, BOX_HEIGHT_MUL * BOX_SIZE, -2 * BOX_SIZE * pos.Y - BOX_SIZE)); texCoords.Add(new Vector2(0, 1));
            verticles.Add(new Vector3(2 * BOX_SIZE * pos.X + BOX_SIZE, BOX_HEIGHT_MUL * BOX_SIZE, -2 * BOX_SIZE * pos.Y - BOX_SIZE)); texCoords.Add(new Vector2(1, 1));
            verticles.Add(new Vector3(2 * BOX_SIZE * pos.X + BOX_SIZE, BOX_HEIGHT_MUL * BOX_SIZE, -2 * BOX_SIZE * pos.Y + BOX_SIZE)); texCoords.Add(new Vector2(0, 1));
            verticles.Add(new Vector3(2 * BOX_SIZE * pos.X - BOX_SIZE, BOX_HEIGHT_MUL * BOX_SIZE, -2 * BOX_SIZE * pos.Y + BOX_SIZE)); texCoords.Add(new Vector2(1, 1));

            //顶点索引


            //底面
            /*indicles.Add((ushort)(begin + 0));
            indicles.Add((ushort)(begin + 2));
            indicles.Add((ushort)(begin + 1));
            
            indicles.Add((ushort)(begin + 0));
            indicles.Add((ushort)(begin + 2));
            indicles.Add((ushort)(begin + 3));

            //顶面
            indicles.Add((ushort)(begin + 4));
            indicles.Add((ushort)(begin + 6));
            indicles.Add((ushort)(begin + 5));

            indicles.Add((ushort)(begin + 4));
            indicles.Add((ushort)(begin + 6));
            indicles.Add((ushort)(begin + 7));*/

            //左面
            indicles.Add((ushort)(begin + 0));
            indicles.Add((ushort)(begin + 4));
            indicles.Add((ushort)(begin + 1));

            indicles.Add((ushort)(begin + 1));
            indicles.Add((ushort)(begin + 4));
            indicles.Add((ushort)(begin + 5));

            //右面
            indicles.Add((ushort)(begin + 3));
            indicles.Add((ushort)(begin + 6));
            indicles.Add((ushort)(begin + 7));

            indicles.Add((ushort)(begin + 3));
            indicles.Add((ushort)(begin + 6));
            indicles.Add((ushort)(begin + 2));

            //前面
            indicles.Add((ushort)(begin + 0));
            indicles.Add((ushort)(begin + 4));
            indicles.Add((ushort)(begin + 3));

            indicles.Add((ushort)(begin + 3));
            indicles.Add((ushort)(begin + 4));
            indicles.Add((ushort)(begin + 7));

            //后面
            indicles.Add((ushort)(begin + 1));
            indicles.Add((ushort)(begin + 5));
            indicles.Add((ushort)(begin + 2));

            indicles.Add((ushort)(begin + 2));
            indicles.Add((ushort)(begin + 5));
            indicles.Add((ushort)(begin + 6));


        }
        public Wall(List<Point> wallPoints)
        {
            verticles = new GLBuffer(BufferTarget.ArrayBuffer);
            texCoords = new GLBuffer(BufferTarget.ArrayBuffer);
            indices = new GLBuffer(BufferTarget.ElementArrayBuffer);

            List<Vector3> vecs = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<ushort> inds = new List<ushort>();

            int s = 0;
            foreach (var i in wallPoints)
            {
                AddBox(vecs, texs, inds, i);
                s++;
                //if (s >= 1) break;
            }

            verticles.BufferData(3 * vecs.Count * sizeof(float), vecs.ToArray(), BufferUsageHint.StaticDraw);
            texCoords.BufferData(2 * texs.Count * sizeof(float), texs.ToArray(), BufferUsageHint.StaticDraw);
            indices.BufferData(inds.Count * sizeof(ushort), inds.ToArray(), BufferUsageHint.StaticDraw);

            vecSize = vecs.Count * 3;
        }

        public void OnDraw(Renderer3D.ObjectDrawShaderLocs locs)
        {
            verticles.Bind();
            GL.VertexAttribPointer(locs.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
            texCoords.Bind();
            GL.VertexAttribPointer(locs.TexCoord, 2, VertexAttribPointerType.Float, false, 0, 0);
            indices.Bind();
            GL.DrawElements(BeginMode.Triangles, vecSize, DrawElementsType.UnsignedShort, 0);
            //GL.DrawElements(BeginMode.Lines, vecSize, DrawElementsType.UnsignedShort, 0);

        }

        readonly GLBuffer verticles, texCoords, indices;
        int vecSize;
    }
}
