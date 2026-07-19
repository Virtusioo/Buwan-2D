
local MyRectangle = Rectangle.new(10, 10, 100, 100)
local OtherRectangle = Rectangle.new(200, 20, 100, 100)
local RectangleSpeed = 60

-- Runs after every module has initialized (e.g., Graphics, Colors).
-- You can remove this function 
function App:OnReady()
	-- Initialize stuff here
end

-- Runs on every app update
---@param deltaTime number How long the last frame took in seconds
function App:OnUpdate(deltaTime)
	-- Update stuff here
	MyRectangle.X = MyRectangle.X + RectangleSpeed * deltaTime
end

-- Runs on every app draw
function App:OnDraw()
	-- Draw stuff here
	Graphics:SetColor(Colors.Black)
	Graphics:ClearScreen() -- Clear the screen with the current color (Black)

	Graphics:SetColor(Colors.Red)
	Graphics:DrawRectangle(MyRectangle)

	if MyRectangle:CollidesWithRectangle(OtherRectangle) then
		Graphics:SetColor(Colors.Green)
	end

	Graphics:DrawRectangle(OtherRectangle)
end