local gears = require("gears")
local awful = require("awful")
local beautiful = require("beautiful")

local constants = require("conf.constants")
local modkey = constants.modkey

local globalkeys = gears.table.join(
	-- Show Rofi
	awful.key({modkey}, "space", function ()
		awful.spawn.with_shell(beautiful.get().rofi_cmd)
	end, {description="Show Rofi", group="Utilities"}),

	-- Open Terminal
	awful.key({modkey}, "t", function ()
		awful.spawn.with_shell("kitty")
	end, {description="Open Kitty", group="Utilities"}),

	-- Restart Awesome
	awful.key({modkey, "Shift"}, "r", awesome.restart, {description="Restart Awesome", group="Awesome"}),

	-- Quit Awesome
	awful.key({modkey, "Shift"}, "q", awesome.quit, {description="Quit Awesome", group="Awesome"}),

	-- Switch monitors
	awful.key({modkey}, "[", function ()
		awful.screen.focus_relative(-1)
	end, {description="Switch to the last monitor", group="Monitor"}),

	awful.key({modkey}, "]", function ()
		awful.screen.focus_relative(1)
	end, {description="Switch to the next monitor", group="Monitor"}),

	-- Volume for notifications
	awful.key({}, "XF86AudioLowerVolume", function ()

	end),

	awful.key({}, "XF86AudioRaiseVolume", function ()
	end),

	awful.key({}, "XF86AudioMute", function ()
	end)
)

-- Keybinds to switch windows
local directions = constants.directions
for key, value in pairs(directions) do
	globalkeys = gears.table.join(globalkeys,
		-- Switch windows
		awful.key({modkey}, key, function ()
			awful.client.focus.bydirection(value)
			if client.focus then
				client.focus:raise()
			end
		end, {description="Switch to the " .. value .. " window", group="Windows"})
	)
end

-- Bind all key numbers to tags.
-- Be careful: we use keycodes to make it work on any keyboard layout.
-- This should map on the top row of your keyboard, usually 1 to 9.
for i = 1, 9 do
    globalkeys = gears.table.join(globalkeys,
		awful.key({modkey}, "#" .. i + 9, function()
			local screen = awful.screen.focused()
			local tag = screen.tags[i]
			if tag then
				tag:view_only()
			end
  	    end, {description = "View tag #" .. i, group = "Tag"}),

		awful.key({modkey, "Control"}, "#" .. i + 9, function()
			local screen = awful.screen.focused()
			local tag = screen.tags[i]
			if tag then awful.tag.viewtoggle(tag) end
		end, {description = "Toggle tag #" .. i, group = "Tag"}),

		awful.key({modkey, "Shift"}, "#" .. i + 9, function()
			if client.focus then
				local tag = client.focus.screen.tags[i]
				if tag then client.focus:move_to_tag(tag) end
			end
		end, {description = "Move focused client to tag #" .. i, group = "Tag"})
	)
end

local clientkeys = gears.table.join(
	-- Quit window
	awful.key({modkey}, "q", function (client)
		client:kill()
	end, {description="Quit focused window"}),

	-- Maximize window
	awful.key({modkey}, "m", function (client)
		client.maximized = not client.maximized
	end, {description="Toggle maximize focused window"}),

	-- Float window
	awful.key({modkey}, "v", function (client)
		client.floating = not client.floating
	end, {description="Toggle float on focused window"})
)

return {
	globalkeys = globalkeys,
	clientkeys = clientkeys
}
