using Buwan.Models;
using Buwan.Modules.Objects;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules
{
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
