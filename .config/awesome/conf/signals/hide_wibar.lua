local function toggle_wibar(client)
	client.screen.wibar.visible = not client.maximized
end

client.connect_signal("property::maximized", toggle_wibar)
client.connect_signal("property::fullscreen", toggle_wibar)
client.connect_signal("manage", toggle_wibar)
client.connect_signal("focus", toggle_wibar)
