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