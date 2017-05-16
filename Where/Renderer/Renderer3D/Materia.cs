using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Where.Renderer.Lower;

namespace Where.Renderer.Renderer3D
{
    class Materia
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

        GLTexture tex, height, normal;
    }
}
