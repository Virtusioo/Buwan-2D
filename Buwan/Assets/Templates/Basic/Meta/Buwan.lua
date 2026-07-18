
---@meta 

App = {}

---@class Color
---@field R number
---@field G number
---@field B number 
Color = {}

-- Create a new color object 
---@param r number The red value ranging from 0.0 to 1.0 
---@param g number The green value ranging from 0.0 to 1.0 
---@param b number The blue value ranging from 0.0 to 1.0
---@return Color 
function Color.new(r, g, b) end 

-- Create a new color object 
---@param r number The red value ranging from 0 to 255 
---@param g number The green value ranging from 0 to 255 
---@param b number The blue value ranging from 0 to 255
---@return Color 
function Color.fromRGB(r, g, b) end 

---@return Color 
local function color() end

---@type table<string, Color>
Colors = {
    White = color(),
    Black = color(),
    Red = color(),
    Green = color(),
    Blue = color()
}
 
---@class Rectangle
---@field X number
---@field Y number
---@field Width number 
---@field Height number 
Rectangle = {}

-- Create a new rectangle object 
---@param x number The horizontal position of the rectangle 
---@param y number The vertical position of the rectangle  
---@param width number The width of the rectangle 
---@param height number The height of the rectangle 
---@return Rectangle
function Rectangle.new(x, y, width, height) end

Graphics = {}

-- Clear the screen 
function Graphics.ClearScreen() end

-- Begin properties state for drawing
--
-- These properties include rotation, color, alpha, etc
function Graphics.BeginState() end  

-- End properties state for drawing 
--
-- Must match with another Graphics.BeginState() 
function Graphics.EndState() end

-- Set the current alpha 
---@note If you want everything to be transparent, set alpha to 0.0
---@param alpha number The alpha value ranging from 0.0 to 1.0
function Graphics.SetAlpha(alpha) end 

-- Set the current color  
---@param color Color The color to use 
function Graphics.SetColor(color) end

-- Draw a rectangle 
---@param rectangle Rectangle The rectangle to use 
function Graphics.DrawRectangle(rectangle) end