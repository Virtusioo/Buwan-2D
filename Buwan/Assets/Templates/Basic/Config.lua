

-- Get the default app configuration 
---@returns The default app configuration table
function App.GetConfig()
	return {
		-- App metadata --
		Name = "Basic", -- The name of the app 
		Version = "1.0.0", -- The version of the app: MAJOR.MINOR.PATCH
		Identifier = "com.buwan.basic", -- The identifier of the app (e.g. com.companyname.appname)

		-- Screen defaults --
		ScreenWidth = 800, -- The initial window width 
		ScreenHeight = 600 -- The initial window height
	}
end 