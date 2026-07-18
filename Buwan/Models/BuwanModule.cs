using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Models
{
    internal abstract class BuwanModule(string name)
    {
        public readonly string Name = name;
        public abstract void OnCreate(LuaTable module);
    }
}
