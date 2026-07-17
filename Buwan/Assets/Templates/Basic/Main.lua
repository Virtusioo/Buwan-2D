
-- Runs after every module has initialized (e.g., Graphics, Colors)
---@note You can remove this function 
function App.OnReady()
	-- Initialize stuff here
end

-- Runs on every app update
---@param deltaTime number How long the last frame took in seconds
function App.OnUpdate(deltaTime)
	-- Update stuff here
end

-- Runs on every app draw
function App.OnDraw()
	-- Draw stuff here
	Graphics.SetColor(Colors.Red)
	Graphics.DrawRectangle(10, 10, 100, 100)
end