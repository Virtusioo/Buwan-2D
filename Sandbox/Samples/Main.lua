
local x, y = 0, 0

function App.OnUpdate(deltaTime)
	x = x + 1 
	y = y + 1
end

function App.OnDraw()
	Graphics.ClearScreen()
	Graphics.SetColor(1, 1, 1) -- Color white
	Graphics.FillRectangle(x, y, 100, 100)
end