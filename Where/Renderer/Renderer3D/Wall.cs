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
        private void AddBox(List<Vector3> verticles,List<Vector2> texCoords,List<ushort> indiclesF, List<ushort> indiclesB, List<ushort> indiclesL, List<ushort> indiclesR, Point pos)
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

            //左面
            indiclesL.Add((ushort)(begin + 0));
            indiclesL.Add((ushort)(begin + 4));
            indiclesL.Add((ushort)(begin + 1));

            indiclesL.Add((ushort)(begin + 1));
            indiclesL.Add((ushort)(begin + 4));
            indiclesL.Add((ushort)(begin + 5));

            //右面
            indiclesR.Add((ushort)(begin + 6));
            indiclesR.Add((ushort)(begin + 7));
            indiclesR.Add((ushort)(begin + 3));

            indiclesR.Add((ushort)(begin + 2));
            indiclesR.Add((ushort)(begin + 6));
            indiclesR.Add((ushort)(begin + 3));

            //前面
            indiclesF.Add((ushort)(begin + 3));
            indiclesF.Add((ushort)(begin + 4));
            indiclesF.Add((ushort)(begin + 0));

            indiclesF.Add((ushort)(begin + 7));
            indiclesF.Add((ushort)(begin + 4));
            indiclesF.Add((ushort)(begin + 3));

            //后面
            indiclesB.Add((ushort)(begin + 1));
            indiclesB.Add((ushort)(begin + 5));
            indiclesB.Add((ushort)(begin + 2));

            indiclesB.Add((ushort)(begin + 2));
            indiclesB.Add((ushort)(begin + 5));
            indiclesB.Add((ushort)(begin + 6));


        }
        public Wall(List<Point> wallPoints)
        {
            verticles = new GLBuffer(BufferTarget.ArrayBuffer);
            texCoords = new GLBuffer(BufferTarget.ArrayBuffer);
            indicesLeft = new GLBuffer(BufferTarget.ElementArrayBuffer);
            indicesRight = new GLBuffer(BufferTarget.ElementArrayBuffer);
            indicesFront = new GLBuffer(BufferTarget.ElementArrayBuffer);
            indicesBack = new GLBuffer(BufferTarget.ElementArrayBuffer);

            List<Vector3> vecs = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<ushort> indsL = new List<ushort>();
            List<ushort> indsR = new List<ushort>();
            List<ushort> indsF = new List<ushort>();
            List<ushort> indsB = new List<ushort>();

            int s = 0;
            foreach (var i in wallPoints)
            {
                AddBox(vecs, texs, indsF,indsB,indsL,indsR,i);
                s++;
                //if (s >= 1) break;
            }

            verticles.BufferData(3 * vecs.Count * sizeof(float), vecs.ToArray(), BufferUsageHint.StaticDraw);
            texCoords.BufferData(2 * texs.Count * sizeof(float), texs.ToArray(), BufferUsageHint.StaticDraw);
            indicesFront.BufferData(indsF.Count * sizeof(ushort), indsF.ToArray(), BufferUsageHint.StaticDraw);
            indicesBack.BufferData(indsB.Count * sizeof(ushort), indsB.ToArray(), BufferUsageHint.StaticDraw);
            indicesLeft.BufferData(indsL.Count * sizeof(ushort), indsL.ToArray(), BufferUsageHint.StaticDraw);
            indicesRight.BufferData(indsR.Count * sizeof(ushort), indsR.ToArray(), BufferUsageHint.StaticDraw);

            vecSize = vecs.Count * 3 / 4;
        }

        public void OnDraw(GLShader shader,Renderer3D.ObjectDrawShaderLocs locs)
        {
            verticles.Bind();
            GL.VertexAttribPointer(locs.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
            texCoords.Bind();
            GL.VertexAttribPointer(locs.TexCoord, 2, VertexAttribPointerType.Float, false, 0, 0);

            Matrix3 tbn;
            shader.SetUniform(locs.Normal, new Vector3(0, 0, -1));
            tbn = Renderer3D.GetTBNMatrix(new Vector3(0, 0, -1), new Vector3(1, 1, 0));
            shader.SetUniform(locs.TBNMatrix, ref tbn);
            indicesFront.Bind();
            GL.DrawElements(BeginMode.Triangles, vecSize, DrawElementsType.UnsignedShort, 0);

            shader.SetUniform(locs.Normal, new Vector3(0, 0, 1));
            tbn = Renderer3D.GetTBNMatrix(new Vector3(0, 0, 1), new Vector3(1, 1, 0));
            shader.SetUniform(locs.TBNMatrix, ref tbn);
            indicesBack.Bind();
            GL.DrawElements(BeginMode.Triangles, vecSize, DrawElementsType.UnsignedShort, 0);

            shader.SetUniform(locs.Normal, new Vector3(1, 0, 0));
            tbn = Renderer3D.GetTBNMatrix(new Vector3(1, 0, 0), new Vector3(0, 1, 1));
            shader.SetUniform(locs.TBNMatrix, ref tbn);
            indicesLeft.Bind();
            GL.DrawElements(BeginMode.Triangles, vecSize, DrawElementsType.UnsignedShort, 0);

            shader.SetUniform(locs.Normal, new Vector3(-1, 0, 0));
            tbn = Renderer3D.GetTBNMatrix(new Vector3(-1, 0, 0), new Vector3(0, 1, 1));
            shader.SetUniform(locs.TBNMatrix, ref tbn);
            indicesRight.Bind();
            GL.DrawElements(BeginMode.Triangles, vecSize, DrawElementsType.UnsignedShort, 0);

        }

        readonly GLBuffer verticles, texCoords;
        readonly GLBuffer indicesFront, indicesBack, indicesLeft, indicesRight;
        int vecSize;
    }
}
