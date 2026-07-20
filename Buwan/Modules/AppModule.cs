using Lua;
using System;
using System.Collections.Generic;
using System.Text;
using Buwan.Runtime;
using System.Security.Cryptography.X509Certificates;

namespace Buwan.Modules
{
    [LuaObject]
    internal partial class AppModule(Application app)
    {
        public Application App { get; private set; } = app;

        [LuaMember("GetConfig")]
        public LuaValue GetConfigFunc;

        [LuaMember("OnReady")]
        public LuaValue OnReadyFunc;

        [LuaMember("OnDraw")]
        public LuaValue OnDrawFunc;

        [LuaMember("OnUpdate")]
        public LuaValue OnUpdateFunc;

        [LuaMember("Exit")]
        public void Exit()
        {
            App.WindowShouldClose = true;
        }

        [LuaMember("GetFPS")]
        public float GetFPS()
        {
            return 1 / App.DeltaTime;
        }
    }
}
