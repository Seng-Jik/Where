using System;

namespace Where.Renderer.Lower
{
    sealed class SSTReader
    {
        public enum SSTCompressMode
        {
            RGBA,DXT1,DXT3,DXT5
        }

        public SSTReader(System.IO.BinaryReader sst)
        {
            byte compressMode = sst.ReadByte();

            switch (compressMode)
            {
                case 0:
                    cmode = SSTCompressMode.RGBA;
                    break;
                case 10:
                    cmode = SSTCompressMode.DXT1;
                    break;
                case 11:
                    cmode = SSTCompressMode.DXT3;
                    break;
                case 12:
                    cmode = SSTCompressMode.DXT5;
                    break;
                default:
                    throw new Exception("Bad sst texture.");
            }

            UInt16 rectSize = sst.ReadUInt16();

            width = sst.ReadUInt16();
            height = sst.ReadUInt16();

            rects = new OpenTK.Box2[rectSize];

            for (UInt16 i = 0;i < rectSize; ++i)
            {
                UInt16 x = sst.ReadUInt16();
                UInt16 y = sst.ReadUInt16();
                UInt16 w = sst.ReadUInt16();
                UInt16 h = sst.ReadUInt16();

                rects[i].Left = x / width;
                rects[i].Right = (x + w)/width;
                rects[i].Top = y/height;
                rects[i].Bottom = (y + h)/height;
            }

            var dataSize = sst.ReadInt32();
            data = sst.ReadBytes(dataSize);
        }

        public byte[] Data
        {
            get => data;
        }

        public SSTCompressMode CompressMode
        {
            get => cmode;
        }

        public OpenTK.Box2[] Rects
        {
            get => rects;
        }

        public OpenTK.Vector2 Size
        {
            get => new OpenTK.Vector2(width, height);
        }

        private byte[] data;
        private float width, height;
        OpenTK.Box2[] rects;
        SSTCompressMode cmode;
    }
}
