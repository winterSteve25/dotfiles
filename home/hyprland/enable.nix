{
  config,
  pkgs,
  inputs,
  ...
} : {
  wayland.windowManager.hyprland.enable = true;
  wayland.windowManager.hyprland.package = inputs.hyprland.packages.${pkgs.system}.hyprland;
}
