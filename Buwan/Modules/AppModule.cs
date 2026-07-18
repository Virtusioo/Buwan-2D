using Buwan.Models;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Modules
{
    internal class AppModule : BuwanModule
    {
        public bool IsRunning = false;

        public AppModule()
            : base("App")
        {}

        public override void OnCreate(LuaTable module)
        {
            module["Exit"] = new LuaFunction((context, ct) =>
            {
                IsRunning = false;

                return new(0);
            });
        }
    }
}
