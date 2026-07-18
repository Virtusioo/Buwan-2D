using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules.Objects
{
    [LuaObject]
    internal partial class BuwanColor
    {
        [LuaMember("R")]
        public float R;

        [LuaMember("G")]
        public float G;

        [LuaMember("B")]
        public float B;

        public BuwanColor() { }

        public BuwanColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        [LuaMember("new")]
        public static BuwanColor New(float r, float g, float b)
        {
            return new(r, g, b);
        }

        [LuaMember("fromRGB")]
        public static BuwanColor FromRGB(float r, float g, float b)
        {
            return new BuwanColor
            {
                R = r / 255,
                G = g / 255,
                B = b / 255
            };
        }
    }
}
