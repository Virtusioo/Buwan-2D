using Buwan.Modules.Objects;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules
{
    [LuaObject]
    internal partial class ColorsModule 
    {
        [LuaMember("Black")]
        public static readonly Color Black = new(0, 0, 0);

        [LuaMember("White")]
        public static readonly Color White = new(1, 1, 1);

        [LuaMember("Red")]
        public static readonly Color Red = new(1, 0, 0);

        [LuaMember("Green")]
        public static readonly Color Green = new(0, 1, 0);

        [LuaMember("Blue")]
        public static readonly Color Blue = new(0, 0, 1);
    }
}
