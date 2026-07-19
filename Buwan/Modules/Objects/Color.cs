using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules.Objects
{
    [LuaObject]
    internal partial class Color
    {
        [LuaMember("R")]
        public float R;

        [LuaMember("G")]
        public float G;

        [LuaMember("B")]
        public float B;

        public Color() { }

        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        [LuaMember("new")]
        public static Color New(float r, float g, float b)
        {
            return new(r, g, b);
        }

        [LuaMember("fromRGB")]
        public static Color FromRGB(float r, float g, float b)
        {
            return new Color
            {
                R = r / 255,
                G = g / 255,
                B = b / 255
            };
        }
    }
}
