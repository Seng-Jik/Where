using Where.Renderer.Lower;

namespace Where.Renderer.Renderer3D
{
    internal class Materia
    {
        public Materia(string name)
        {
            tex = new GLTexture();
            height = new GLTexture();
            normal = new GLTexture();

            tex.BindTo0AndLoadImage(name);
            height.BindTo0AndLoadImage(name + "_height");
            normal.BindTo0AndLoadImage(name + "_normal");
        }

        public void Bind()
        {
            tex.Bind(0);
            height.Bind(1);
            normal.Bind(2);
        }

        private GLTexture tex, height, normal;
    }
}