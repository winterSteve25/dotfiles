
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

local ctx_menu = awful.menu({
    items = {
		{
			"Launcher",
			beautiful.get().rofi_cmd,
			beautiful.get().launcher_icon
		},
		{
			"Terminal",
			constants.terminal,
			beautiful.get().terminal_icon
		},
        { "Awesome", awesome_menu, beautiful.awesome_icon },
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
