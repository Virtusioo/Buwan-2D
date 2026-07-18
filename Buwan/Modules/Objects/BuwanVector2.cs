using System;
using System.Collections.Generic;
using System.Text;
using Lua;

namespace Buwan.Modules.Objects
{
    [LuaObject]
    internal partial class BuwanVector2
    {
        [LuaMember("X")]
        public float X;

        [LuaMember("Y")]
        public float Y;

        [LuaMember("new")]
        public static BuwanVector2 New(float x, float y)
        {
            return new BuwanVector2
            {
                X = x,
                Y = y
            };
        }
    }
}
