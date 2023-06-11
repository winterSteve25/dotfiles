-- Runs when mouse enters a window
client.connect_signal("mouse::enter", function(c)
	-- Makes it focus follow mouse 
    c:emit_signal("request::activate", "mouse_enter", {raise = false})
end)
