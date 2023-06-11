local beautiful = require("beautiful")

return {
	hover_bg = function (background)
		background:connect_signal("mouse::enter", function ()
			background.bg = beautiful.bg_focus
		end)
		background:connect_signal("mouse::leave", function ()
			background.bg = beautiful.bg_normal
		end)
	end
}
