
local awful = require("awful")
local beautiful = require("beautiful")
local hotkeys_popup = require("awful.hotkeys_popup")
local constants = require("conf.constants")

local awesome_menu = {
    {
        "Show Hotkeys",
        function()
			hotkeys_popup.show_help(nil, awful.screen.focused())
		end,
    },
    {
		"Restart Awesome",
		awesome.restart,
	},
    {
		"Quit Awesome",
		awesome.quit,
	},
}

local utilities_menu = {
	{
		"Launcher",
		"rofi -show drun",
	},
	{
		"Terminal",
		constants.terminal
	},
}

local ctx_menu = awful.menu({
    items = {
        { "Awesome", awesome_menu, beautiful.awesome_icon },
        { "Utilities", utilities_menu }
    }
})

local launcher_widget = awful.widget.launcher({
    image = beautiful.awesome_icon,
    menu = ctx_menu
})

return {
	launcher = launcher_widget,
	ctx_menu = ctx_menu
}
