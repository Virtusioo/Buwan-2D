using Buwan.Models;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules
{
    [LuaObject]
    internal partial class LuaColor
    {
        [LuaMember("R")]
        public float R;

        [LuaMember("G")]
        public float G;

        [LuaMember("B")]
        public float B;

        public LuaColor() { }

        public LuaColor(float r, float g, float b)
        {
            R = r;  
            G = g;
            B = b;
        }

        [LuaMember("fromRGB")]
        public static LuaColor FromRGB(float r, float g, float b)
        {
            return new LuaColor
            {
                R = r / 255,
                G = g / 255,
                B = b / 255
            };
        }
    }

    internal class ColorsModule : ILuaModule
    {
        public void OnCreate(LuaTable module)
        {
            module["Black"] = new LuaColor(0, 0, 0);
            module["White"] = new LuaColor(1, 1, 1);
            module["Red"] = new LuaColor(1, 0, 0);
            module["Blue"] = new LuaColor(0, 1, 0);
            module["Green"] = new LuaColor(0, 0, 1);
        }
    }
}
