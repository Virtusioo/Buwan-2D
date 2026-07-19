using System;
using System.Collections.Generic;
using System.Text;
using Lua;

namespace Buwan.Modules.Objects
{
    [LuaObject]
    internal partial class Vector2
    {
        [LuaMember("X")]
        public float X;

        [LuaMember("Y")]
        public float Y;

        [LuaMember("new")]
        public static Vector2 New(float x, float y)
        {
            return new Vector2
            {
                X = x,
                Y = y
            };
        }
    }
}
