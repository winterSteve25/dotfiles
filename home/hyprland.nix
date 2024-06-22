{ config, pkgs, inputs, ... }:

{
    wayland.windowManager.hyprland.enable = true;
    wayland.windowManager.hyprland.settings = {
	"$mod" = "SUPER";
	monitor = [
	    "DP-1,1920x1080@144,0x0,1"
	    "eDP-1,1920x1080,1920x0,1"
	];
	bind = [
	    "$mod, T, exec, kitty"
	    "$mod, Q, killactive,"
	    "$mod, V, togglefloating,"
	    "$mod, S, fullscreen, 0"
	    "$mod_SHIFT, Q, exit,"
	];
	exec-once = [
	    "copyq --start-server"
	    "discord"
	];
	input = {
	    sensitivity = 0;
	    follow_mouse = 1;
	    accel_profile = "flat";
	};
    };

    wayland.windowManager.hyprland.plugins = [
	inputs.split-monitor-workspaces.packages.${pkgs.system}.split-monitor-workspaces
    ];
}
