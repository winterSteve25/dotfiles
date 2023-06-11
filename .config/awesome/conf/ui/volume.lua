local awful = require("awful")
local gears = require("gears")
local wibox = require("wibox")
local beautiful = require("beautiful")

local volumenotification = awful.popup {
	widget = {
		{
			image = beautiful.get().volume_icon,
			resize = true,
			widget = wibox.widget.imagebox
		},
		{
			max_value = 1,
			value = 0.5,
			widget = wibox.widget.progressbar,
		},
		layout = wibox.layout.fixed.horizontal
	},
	placement = awful.placement.centered,
	shape = gears.shape.rounded_rect
}

return {
	widget = volumenotification,
}
