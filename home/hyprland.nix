{ config, pkgs, inputs, ... }:

{
    wayland.windowManager.hyprland.enable = true;
    wayland.windowManager.hyprland.package = inputs.hyprland.packages.${pkgs.system}.hyprland;
    wayland.windowManager.hyprland.settings = {
	monitor = [
	    "DP-1,1920x1080@144,0x0,1"
	    "eDP-1,1920x1080,1920x0,1"
	];
	bind = [
	    "SUPER, T, exec, kitty"
	    "SUPER, RETURN, exec, fuzzel"
	    "SUPER, Q, killactive,"
	    "SUPER, V, togglefloating,"
	    "SUPER, S, fullscreen, 0"
	    "SUPER_SHIFT, Q, exit,"
	    ", PRINT, exec, grim -g \"$(slurp -d)\" - | wl-copy -t image/png"
	    ", XF86AudioRaiseVolume, exec, wpctl set-volume -l 1.4 @DEFAULT_AUDIO_SINK@ 5%+"
	    ", XF86AudioLowerVolume, exec, wpctl set-volume -l 1.4 @DEFAULT_AUDIO_SINK@ 5%-"
	    "SUPER, h, movefocus, l"
	    "SUPER, l, movefocus, r"
	    "SUPER, k, movefocus, u"
	    "SUPER, j, movefocus, d"
	    "SUPER, 1, split-workspace, 1"
	    "SUPER, 2, split-workspace, 2"
	    "SUPER, 3, split-workspace, 3"
	    "SUPER, 4, split-workspace, 4"
	    "SUPER, 5, split-workspace, 5"
	    "SUPER, 6, split-workspace, 6"
	    "SUPER, 7, split-workspace, 7"
	    "SUPER, 8, split-workspace, 8"
	    "SUPER, 9, split-workspace, 9"
	    "SUPER, 0, split-workspace, 10"
	    "SUPER_SHIFT, 1, split-movetoworkspace, 1"
	    "SUPER_SHIFT, 2, split-movetoworkspace, 2"
	    "SUPER_SHIFT, 3, split-movetoworkspace, 3"
	    "SUPER_SHIFT, 4, split-movetoworkspace, 4"
	    "SUPER_SHIFT, 5, split-movetoworkspace, 5"
	    "SUPER_SHIFT, 6, split-movetoworkspace, 6"
	    "SUPER_SHIFT, 7, split-movetoworkspace, 7"
	    "SUPER_SHIFT, 8, split-movetoworkspace, 8"
	    "SUPER_SHIFT, 9, split-movetoworkspace, 9"
	    "SUPER_SHIFT, 0, split-movetoworkspace, 10"
	    "SUPER_SHIFT, h, split-changemonitor, next"
	    "SUPER_SHIFT, l, split-changemonitor, prev"
	    "SUPER_CTRL, h, resizeactive, -40 0"
	    "SUPER_CTRL, l, resizeactive, 40 0"
	    "SUPER_CTRL, k, resizeactive, 0 -40"
	    "SUPER_CTRL, j, resizeactive, 0 40"
	];
	bindm = [
	    "SUPER, mouse:272, movewindow"
	    "SUPER, mouse:273, resizewindow"
	];
	exec-once = [
	    "copyq --start-server"
	    "webcord"
	];
	windowrulev2 = [
	    "opacity 0.0 override,class:^(xwaylandvideobridge)$"
	    "noanim,class:^(xwaylandvideobridge)$"
	    "noinitialfocus,class:^(xwaylandvideobridge)$"
	    "maxsize 1 1,class:^(xwaylandvideobridge)$"
	    "noblur,class:^(xwaylandvideobridge)$"
	    "workspace 11,class:(WebCord)"
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
