--------------------------------
------------IMPORTS-------------
--------------------------------
pcall(require, "luarocks.loader")

-- Standard awesome library
local gears = require("gears")
local awful = require("awful")

-- Start up stuff
require("awful.autofocus")
require("conf.errors").handle()

local beautiful = require("beautiful") -- Theme handling library
local menubar = require("menubar") -- Application Launcher

--------------------------------
------------SIGNALS-------------
--------------------------------
require("conf.signals.manage_window")
require("conf.signals.update_wallpaper")
require("conf.signals.titlebar")
require("conf.signals.focus_mouse")
require("conf.signals.border")

--------------------------------
-------------SETUP--------------
--------------------------------
-- Themes define colours, icons, font and wallpapers.
beautiful.init(gears.filesystem.get_configuration_dir() .. "themes/flower/theme.lua")

local terminal = require("conf.constants").terminal
local mouse_bindings = require("conf.binding.mouse");
local keyboard_bindings = require("conf.binding.keys");
local bar = require("conf.ui.bar")

-- Table of layouts to cover with awful.layout.inc, order matters.
awful.layout.layouts = require("conf.layouts")

-- Menubar configuration
menubar.utils.terminal = terminal -- Set the terminal for applications that require it

-- Setup bar
awful.screen.connect_for_each_screen(bar.connect_screen)

-- setup global keys
root.buttons(mouse_bindings.globalbuttons)
root.keys(keyboard_bindings.globalkeys)

-- setup window rules
awful.rules.rules = require("conf.window_rules")

-- auto start applications
local function run_if_not_running(program, arguments)
	awful.spawn.easy_async(
		"pgrep " .. program,
		function(_, _, _, exit_code)
			if exit_code ~= 0 then
				awful.spawn.with_shell(program .. " " .. arguments)
			end
		end
	)
end

awful.spawn.once("xinput set-prop 'Glorious Model O Wireless' 'libinput Accel Speed' -0.6")
awful.spawn.once("picom -b")
awful.spawn.once("xrandr --output DisplayPort-0 --mode 1920x1080 --rate 144")
awful.spawn.once("discord")
run_if_not_running("clipit", "&")
