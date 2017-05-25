using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Where.Renderer.Renderer3D
{
    class DayNight
    {
        double clock = 7;
        public Vector3 SkyColorA { get; private set; }
        public Vector3 SkyColorB { get; private set; }
        public Vector3 LightPos { get; private set; }
        public void Update()
        {
            clock += 1.0 / 60.0 / 60.0;     //每帧1秒
            if (clock >= 24) clock -= 24;
        }

        readonly Vector3[] skyColorsA =
        {
            new Vector3(0.00784313725490196f,0.0784313725490196f,0.266666666666667f),  //夜:[20~24]U[0~4]
            new Vector3(0.2f, 0.4f, 0.6f),                                                 //上午,下午:[7~9]U[15,16]
            new Vector3(0.4f, 0.7f, 1.0f),                                                  //中午:[11~14]
            new Vector3(0.952941176470588f,0.388235294117647f,0.305882352941176f)       //黄昏:[18~19]
        };
    }
}
