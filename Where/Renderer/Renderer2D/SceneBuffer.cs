using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Where.Renderer.Lower;

namespace Where.Renderer.Renderer2D
{
    public class SceneBuffer
    {
        public GLBuffer VertexBuffer { get; set; }
        public GLBuffer IndexBuffer { get; set; }
        public int VerticleSize { get; set; }
    }
}
