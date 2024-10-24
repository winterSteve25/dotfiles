{config, ...}: {
  wayland.windowManager.hyprland.settings = {
    bind = [
      "SUPER, T, exec, kitty"
      "SUPER, RETURN, exec, fuzzel"
      "SUPER, Q, killactive,"
      "SUPER, V, togglefloating,"
      "SUPER, S, fullscreen, 0"
      "SUPER_SHIFT, Q, exit,"
      ", PRINT, exec, grim -g \"$(slurp -d)\" - | wl-copy -t image/png"
      "SUPER, h, movefocus, l"
      "SUPER, l, movefocus, r"
      "SUPER, k, movefocus, u"
      "SUPER, j, movefocus, d"
      "SUPER_CTRL, h, resizeactive, -40 0"
      "SUPER_CTRL, l, resizeactive, 40 0"
      "SUPER_CTRL, k, resizeactive, 0 -40"
      "SUPER_CTRL, j, resizeactive, 0 40"
    ];
    bindel = [
      ", XF86AudioRaiseVolume, exec, wpctl set-volume -l 1.4 @DEFAULT_AUDIO_SINK@ 5%+"
      ", XF86AudioLowerVolume, exec, wpctl set-volume -l 1.4 @DEFAULT_AUDIO_SINK@ 5%-"
    ];
    bindm = [
      "SUPER, mouse:272, movewindow"
      "SUPER, mouse:273, resizewindow"
    ];
  };
}
