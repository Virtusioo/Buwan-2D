using Buwan.Models;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Common
{
    internal class LuaLibraryBuilder
    {
        private readonly Dictionary<string, ILuaModule> _modules = [];

        public void PutModule(string name, ILuaModule module)
        {
            _modules[name] = module;
        }

        public void BuildToTable(LuaTable library)
        {
            foreach (var pair in _modules)
            {
                string name = pair.Key;
                ILuaModule module = pair.Value;
                LuaTable tableModule = new();

                module.OnCreate(tableModule);
                library[name] = tableModule;
            }
        }
    }
}
