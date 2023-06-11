local wibox = require("wibox")
local gears = require("gears")
local awful = require("awful")
local beautiful = require("beautiful")
local uiutils = require("utils.ui")

local dpi = require("beautiful.xresources").apply_dpi

local function initialize_screen(screen)
	-- Wallpaper
	require("conf.appearance").set_wallpaper(screen)

	-- Create tags for screen. Layout indicate the initial window layout mode
	awful.tag({"1", "2", "3", "4", "5", "6", "7", "8", "9"}, screen, awful.layout.layouts[1])
end

local function create_taglist(screen)
	-- Create a taglist widget
	return awful.widget.taglist {
		screen = screen,
		filter = awful.widget.taglist.filter.all,
		buttons = gears.table.join(awful.button({}, 1, function(t) t:view_only() end)),
		layout = wibox.layout.fixed.vertical,
	}
end

local function create_bar(screen)
	return awful.wibar({
		position = "left",
		screen = screen,
		width = dpi(55),
		border_width = dpi(8),
		border_color = "#00000000",
		shape = function (cr, w, h)
			gears.shape.rounded_rect(cr, w, h, 20)
		end
	})
end

local function create_tasklist(screen)
		return awful.widget.tasklist {
		screen = screen,
		filter = awful.widget.tasklist.filter.currenttags,
		buttons = gears.table.join(awful.button({}, 1, function (c)
			if c == client.focus then
				c.minimized = true
			else
				c:emit_signal(
					"request::activate",
					"tasklist",
					{ raise = true }
				)
			end
		end)),
		layout = {
			layout = wibox.layout.flex.vertical
		},
		widget_template = {
			{
				{
					{
						{
							id = 'icon_role',
							widget = wibox.widget.imagebox,
						},
						margins = 4,
						widget  = wibox.container.margin,
					},
					layout = wibox.layout.fixed.vertical,
				},
				left = 8,
				right = 8,
				widget = wibox.container.margin
			},
			id = 'background_role',
			widget = wibox.container.background,
		},
	}
end

local function create_clock(screen)
	local layout = wibox.layout.fixed.vertical()
	layout:add(wibox.container.place(wibox.widget.textclock("%H:%M"), 'center', 'center'))
	layout:add(wibox.container.place(wibox.widget.textclock("%b %d"), 'center', 'center'))
	local bg = wibox.container.background(layout, beautiful.bg_normal)
	uiutils.hover_bg(bg)

	local calendar_popup = awful.widget.calendar_popup.month({
		screen = screen,
		margin = dpi(8)
	})

	calendar_popup:attach(bg, 'bl')

	return bg
end

local function create_powerbtn()
	local btn = wibox.widget.imagebox(beautiful.get().powerbtn_icon)
	btn.resize = true
	btn.forced_width = dpi(24)
	btn.forced_height = dpi(24)

	local bg = wibox.container.background(wibox.container.place(btn, 'center', 'center'), beautiful.bg_normal)
	bg.forced_height = dpi(36)

	btn:connect_signal("button::press", function (_, _, _, mousebtn)
		if mousebtn ~= 1 then
			return
		end

		awful.spawn.with_shell(beautiful.get().powermenu_cmd)
	end)
	uiutils.hover_bg(bg)

	return bg
end

local function start_bar(screen)
	local layout = wibox.layout.fixed.vertical()

	layout:add(wibox.container.place(wibox.container.constraint(wibox.widget.imagebox(beautiful.awesome_icon), 'exact', dpi(32)), 'center', 'center'))
	layout:add(wibox.widget { orientation = 'horizontal', forced_height = dpi(16), span_ratio = 0.8, widget = wibox.widget.separator })
  	layout:add(create_tasklist(screen))

	return layout
end

local function end_bar(screen)
	local layout = wibox.layout.fixed.vertical()
	layout.spacing = dpi(8)

	layout:add(create_clock(screen))
	layout:add(create_powerbtn())

	return layout
end

return {
	connect_screen = function (screen)
		initialize_screen(screen)

		-- if screen.index ~= 1 then
		-- 	return
		-- end

		-- Add widgets to the wibox
		create_bar(screen):setup {
			layout = wibox.layout.align.vertical,
			wibox.container.margin(start_bar(screen), 0, 0, dpi(16), dpi(16)),
			wibox.container.place(create_taglist(screen), 'center', 'center'),
			wibox.container.margin(end_bar(screen), 0, 0, dpi(16), dpi(16)),
		}
	end
}
