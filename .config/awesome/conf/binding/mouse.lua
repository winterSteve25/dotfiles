local gears = require("gears")
local awful = require("awful")
local modkey = require("conf.constants").modkey

return {
    clientbuttons = gears.table.join(awful.button({}, 1, function(c)
        c:emit_signal("request::activate", "mouse_click", {raise = true})
    	end),
		awful.button({modkey}, 1, function(c)
        	c:emit_signal("request::activate", "mouse_click", {raise = true})
        	awful.mouse.client.move(c)
    	end),
		awful.button({modkey}, 3, function(c)
        	c:emit_signal("request::activate", "mouse_click", {raise = true})
        	awful.mouse.client.resize(c)
    	end)
	),
	globalbuttons = gears.table.join(awful.button({}, 3, function()
			require("conf.ui.menus").ctx_menu:toggle()
		end),
		awful.button({}, 4, awful.tag.viewnext),
		awful.button({}, 5, awful.tag.viewprev)
	),
}
