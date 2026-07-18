using Buwan.Models;
using Lua;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buwan.Common
{
    internal class BuwanLibraryBinder(LuaState state)
    {
        private readonly List<BuwanModule> _modules = [];
        private readonly Dictionary<string, BuwanModule> _storedModules = [];
        public LuaState Context { get; private set; } = state;


        public T AddModule<T>() where T: BuwanModule, new()
        {
            T module = new();

            _modules.Add(module);
            _storedModules[typeof(T).Name] = module;

            return module;
        }
        
        public T GetModule<T>() where T: BuwanModule, new()
        {
            string moduleName = typeof(T).Name;

            return (T)_storedModules.GetValueOrDefault(moduleName)!;
        }

        /// <summary>
        /// Add all modules to lua environment and clear all modules for the next bind
        /// </summary>
        public void BindModules()
        {
            foreach (var module in _modules)
            {
                LuaTable tableModule = new();

                module.OnCreate(tableModule);
                Context.Environment[module.Name] = tableModule;
            }

            _modules.Clear();
        }
    }
}
