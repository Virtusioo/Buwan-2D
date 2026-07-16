
function Buwan.OnReady()
	X = 0
	Y = 0
end

function Buwan.OnUpdate(deltaTime)
	X = X + 1 
	Y = Y + 1
end

function Buwan.OnDraw()
	ClearScreen()
	SetColor(1, 1, 1) -- Color white
	FillRectangle(X, Y, 100, 100)
end