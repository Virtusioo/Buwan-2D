using Lua;
using System;
using System.Collections.Generic;
using System.Text;
using SDL3;
using System.Drawing;

namespace Buwan.Modules.Objects
{
    [LuaObject]
    internal partial class Rectangle
    {
        public SDL.FRect Rect;

        [LuaMember("X")]
        public float X
        {
            get => Rect.X;
            set => Rect.X = value;
        }

        [LuaMember("Y")]
        public float Y
        {
            get => Rect.Y;
            set => Rect.Y = value;
        }

        [LuaMember("Width")]
        public float Width
        {
            get => Rect.W;
            set => Rect.W = value;
        }

        [LuaMember("Height")]
        public float Height
        {
            get => Rect.H;
            set => Rect.H = value;
        }

        [LuaMember("new")]
        public static Rectangle New(float x, float y, float width, float height)
        {
            return new Rectangle
            {
                Rect = new SDL.FRect
                {
                    X = x,
                    Y = y,
                    W = width,
                    H = height
                }
            };
        }

        [LuaMember("CollidesWithRectangle")]
        public bool CollidesWithRectangle(Rectangle rectangle)
        {
            return (X + Width >= rectangle.X &&
                   X <= rectangle.X + rectangle.Width) ||
                   (Y >= rectangle.Y &&
                   Y + Height <= rectangle.Y + rectangle.Height);
        }

        [LuaMember("CollidesWithPoint")]
        public bool CollidesWithPoint(Vector2 point)
        {
            return (X >= point.X && X <= point.X) ||
                   (Y >= point.Y && Y <= point.Y);
        }
    }
}
