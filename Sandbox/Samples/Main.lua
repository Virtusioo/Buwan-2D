
local x, y = 0, 0
local Speed = 5

function App.OnUpdate(deltaTime)
	x = x + Speed
	y = y + Speed
end

function App.OnDraw()
	Graphics.ClearScreen()
	Graphics.SetColor(1, 1, 1, 1) -- Color white
	Graphics.FillRectangle(x, y, 100, 100)
end