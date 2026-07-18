using Buwan.Models;
using Buwan.Modules.Objects;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules
{
    internal class ColorsModule : BuwanModule
    {
        public ColorsModule()
            : base("Colors")
        {}

        public override void OnCreate(LuaTable module)
        {
            module["Black"] = new BuwanColor(0, 0, 0);
            module["White"] = new BuwanColor(1, 1, 1);
            module["Red"] = new BuwanColor(1, 0, 0);
            module["Green"] = new BuwanColor(0, 1, 0);
            module["Blue"] = new BuwanColor(0, 0, 1);
        }
    }
}
