{
  config,
  pkgs,
  inputs,
  ...
}: {
  imports = [
    ../../home/hyprland/binds.nix
    ../../home/hyprland/input.nix
    ../../home/hyprland/enable.nix
    ../../home/hyprland/startups.nix
    ../../home/hyprland/window_rules.nix
  ];

  wayland.windowManager.hyprland.settings = {
    monitor = [
      "DP-1,1920x1080@144,0x0,1"
      "eDP-1,1920x1080@60,1920x0,1"
    ];
    bind = [
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
    ];
    windowrulev2 = [
      "workspace 11,class:(WebCord)"
    ];
  };

  wayland.windowManager.hyprland.plugins = [
    inputs.split-monitor-workspaces.packages.${pkgs.system}.split-monitor-workspaces
  ];
}
