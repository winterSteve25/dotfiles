{ config, pkgs, inputs, ... }:

let
	wallpapers = ./wallpapers;
	changebg = pkgs.writeShellScript "bgchanger" ''
			count=$(find ${wallpapers} | wc -l)
			echo "Count: ''${count}"
			rand=$RANDOM
			echo "Rand: ''${rand}"
			i=$((1 + (rand % count)))
			echo "i: ''${i}"
			path=$(find ${wallpapers} | sed -n "''${i}p")
			echo "path: ''${path}"
			${pkgs.swww} img $path --transition-type wipe
	'';
in
{
    wayland.windowManager.hyprland.enable = true;
    wayland.windowManager.hyprland.package = inputs.hyprland.packages.${pkgs.system}.hyprland;
    wayland.windowManager.hyprland.settings = {
		monitor = [
			# "DP-1,1920x1080@144,0x0,1"
			# "eDP-1,1920x1080@60,1920x0,1"
			"eDP-1,1536x1024@60,0x0,1"
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
			"SUPER, 1, workspace, 1"
			"SUPER, 2, workspace, 2"
			"SUPER, 3, workspace, 3"
			"SUPER, 4, workspace, 4"
			"SUPER, 5, workspace, 5"
			"SUPER, 6, workspace, 6"
			"SUPER, 7, workspace, 7"
			"SUPER, 8, workspace, 8"
			"SUPER, 9, workspace, 9"
			"SUPER, 0, workspace, 10"
			"SUPER_SHIFT, 1, movetoworkspace, 1"
			"SUPER_SHIFT, 2, movetoworkspace, 2"
			"SUPER_SHIFT, 3, movetoworkspace, 3"
			"SUPER_SHIFT, 4, movetoworkspace, 4"
			"SUPER_SHIFT, 5, movetoworkspace, 5"
			"SUPER_SHIFT, 6, movetoworkspace, 6"
			"SUPER_SHIFT, 7, movetoworkspace, 7"
			"SUPER_SHIFT, 8, movetoworkspace, 8"
			"SUPER_SHIFT, 9, movetoworkspace, 9"
			"SUPER_SHIFT, 0, movetoworkspace, 10"
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
			"ags"
			"copyq --start-server"
			"swww-daemon"
			"swww img ${wallpapers}/a_cartoon_of_a_person_with_headphones.png"
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
}
