using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Model
{
    internal interface ILuaModule
    {
        void OnCreate(LuaTable module);
    }
}
