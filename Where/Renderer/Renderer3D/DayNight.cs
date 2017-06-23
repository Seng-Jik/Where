using OpenTK;
using System;

namespace Where.Renderer.Renderer3D
{
    internal class DayNight
    {
        public double Clock { get; private set; } = 3.0;
        public Vector3 SkyColorA { get; private set; }
        public Vector3 SkyColorB { get; private set; }
        public float CloudDensity { get; private set; }
        public float PlayerLight { get; private set; }
        public Vector3 SunLightPos { get; private set; }

        public struct TimeDescribe
        {
            public enum DescribeEnum
            {
                Night = 0,
                Morning = 1,
                Noon = 2,
                AfterNoon = 3,
                Evening = 4
            }

            public DescribeEnum Describe;
            public double Delta;
        }

        public TimeDescribe Describe
        {
            get
            {
                TimeDescribe t;
                t.Delta = 0;

                t.Describe = TimeDescribe.DescribeEnum.Night;

                if (Clock >= 20 && Clock <= 4)
                    t.Describe = TimeDescribe.DescribeEnum.Night;

                if (Clock >= 7 && Clock <= 9)
                    t.Describe = TimeDescribe.DescribeEnum.Morning;

                if (Clock >= 11 && Clock <= 14)
                    t.Describe = TimeDescribe.DescribeEnum.Noon;

                if (Clock >= 15 && Clock <= 16)
                    t.Describe = TimeDescribe.DescribeEnum.AfterNoon;

                if (Clock >= 18 && Clock <= 19)
                    t.Describe = TimeDescribe.DescribeEnum.Evening;

                //插值行为
                double deltaBegin = 0, deltaEnd = 0;
                if (Clock >= 4 && Clock <= 7)
                {
                    deltaBegin = 4;
                    deltaEnd = 7;
                    t.Describe = TimeDescribe.DescribeEnum.Night;
                }

                if (Clock >= 9 && Clock <= 11)
                {
                    deltaBegin = 9;
                    deltaEnd = 11;
                    t.Describe = TimeDescribe.DescribeEnum.Morning;
                }

                if (Clock >= 14 && Clock <= 15)
                {
                    deltaBegin = 14;
                    deltaEnd = 15;
                    t.Describe = TimeDescribe.DescribeEnum.Noon;
                }

                if (Clock >= 16 && Clock <= 18)
                {
                    deltaBegin = 16;
                    deltaEnd = 18;
                    t.Describe = TimeDescribe.DescribeEnum.AfterNoon;
                }

                if (Clock >= 19 && Clock <= 20)
                {
                    deltaBegin = 19;
                    deltaEnd = 20;
                    t.Describe = TimeDescribe.DescribeEnum.Evening;
                }

                if (deltaEnd - deltaBegin > 0)
                    t.Delta = (Clock - deltaBegin) / (deltaEnd - deltaBegin);
                else
                    t.Delta = 0;

                t.Delta = Math.Sin(Math.PI / 2 * t.Delta);

                return t;
            }
        }

        public void Update()
        {
            Clock += 1.0 / 60.0;     //每帧1秒
            if (Clock >= 24) Clock -= 24;

            UpdateSkyColor();
            CloudDensity = UpdateFloatValue(cloudDensitys);
            PlayerLight = UpdateFloatValue(playerLights);
            UpdateSunLightPos();
        }

        private void UpdateSunLightPos()
        {
            double sunDelta = Clock / 24 * 360;
            sunDelta += 90;
            sunDelta = sunDelta * Math.PI / 180;

            SunLightPos = new Vector3(3000 * (float)Math.Cos(sunDelta), 3000 * (float)Math.Sin(sunDelta), -30 * (float)Math.Sin(sunDelta));
        }

        private T CalcValueByDescribe<T>(T[] vset, TimeDescribe td, Func<T, T, float, T> mixer)
        {
            var begin = vset[(int)td.Describe];
            var end = vset[(int)td.Describe + 1];
            return mixer(begin, end, (float)td.Delta);
        }

        private readonly Vector3[] skyColorsA = //亮色
        {
            new Vector3(0.05f,0.05f,0.25f),  //夜:[20~24]U[0~4]
            new Vector3(0.2f, 0.4f, 0.6f),                                              //上午:[7~9]
            new Vector3(1.0f, 1.0f, 1.0f),                                              //中午:[11~14]
            new Vector3(0.4f, 0.6f, 0.9f),                                              //下午:[15,16]
            new Vector3(0.952941176470588f,0.388235294117647f,0.305882352941176f),      //黄昏:[18~19]
            new Vector3(0.05f,0.05f,0.25f)                                               //夜:[20~24]U[0~4]
        };

        private readonly Vector3[] skyColorsB =    //暗色
        {
            new Vector3(0.00784313725490196f,0.0784313725490196f,0.266666666666667f),  //夜:[20~24]U[0~4]
            new Vector3(0.2f, 0.4f, 0.6f),                                              //上午:[7~9]
            new Vector3(0.6f, 0.9f, 1.0f),                                              //中午:[11~14]
            new Vector3(0.4f, 0.6f, 0.9f),                                              //下午:[15,16]
            new Vector3(0.952941176470588f,0.388235294117647f,0.305882352941176f),       //黄昏:[18~19]
            new Vector3(0.00784313725490196f,0.0784313725490196f,0.266666666666667f)  //夜:[20~24]U[0~4]
        };

        private void UpdateSkyColor()
        {
            var des = Describe;

            SkyColorA = CalcValueByDescribe(skyColorsA, des, (a, b, d) => (b - a) * d + a);
            SkyColorB = CalcValueByDescribe(skyColorsB, des, (a, b, d) => (b - a) * d + a);
        }

        private readonly float[] cloudDensitys = { 0.1f, 0.7f, 1.0f, 0.7f, 0.8f, 0.1f };
        private readonly float[] playerLights = { 1.2f, 0.25f, 0.0f, 0.0f, 0.0f, 1.2f };

        private float UpdateFloatValue(float[] vset) => CalcValueByDescribe(vset, Describe, (a, b, d) => (b - a) * d + a);
    }
}