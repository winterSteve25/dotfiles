local beautiful = require("beautiful")
local awful = require("awful")
local mouse_bindings = require("conf.binding.mouse");
local keyboard_bindings = require("conf.binding.keys");

return {
    -- For everything
    {
        rule = {},
        properties = {
            border_width = beautiful.border_width,
            border_color = beautiful.border_normal,
            focus = awful.client.focus.filter,
            raise = true,
            keys = keyboard_bindings.clientkeys,
            buttons = mouse_bindings.clientbuttons,
            screen = awful.screen.preferred,
            placement = awful.placement.no_overlap + awful.placement.no_offscreen,
        }
	},

	-- For Chrome and electron apps, for some reason they starts maximized
	{
		rule_any = {
			class = {
				"google-chrome",
				"Google-chrome",
				"discord"
			},
			role = {
				"browser",
				"browser-window"
			}
		},
		callback = function(client)
			client.maximized, client.maximized_vertical, client.maximized_horizontal = false, false, false
		end
	},

	-- Chrome on screen 2 tab 2
	{
		rule_any = {
			class = {
				"google-chrome",
				"Google-chrome",
			},
			role = "browser",
		},
		properties = {
			screen = 2,
			tag = "2",
		},
	},

	-- Discord on screen 2 tab 1
	{
		rule = {
			class = "discord",
			role = "browser-window",
		},
		properties = {
			screen = 2,
			tag = "1",
		},
	},

    -- Make pop-ups floating windows
	{
        rule_any = {
            role = {
                "pop-up", -- e.g. Google Chrome's (detached) Developer Tools.
            }
        },
        properties = {
			floating = true
		}
    },
}
