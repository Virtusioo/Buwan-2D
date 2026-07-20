using System;
using System.Collections.Generic;
using System.Text;
using Lua;
using SDL3;

namespace Buwan.Modules.Objects
{
    [LuaObject]
    internal partial class Vector2
    {
        public SDL.FPoint Point;

        [LuaMember("X")]
        public float X
        {
            get => Point.X;
            set => Point.X = value;
        }

        [LuaMember("Y")]
        public float Y
        {
            get => Point.Y;
            set => Point.Y = value;
        }

        [LuaMember("new")]
        public static Vector2 New(float x, float y)
        {
            return new Vector2
            {
                X = x,
                Y = y
            };
        }

        public static implicit operator SDL.FPoint(Vector2 value) => value.Point;
    }
}
