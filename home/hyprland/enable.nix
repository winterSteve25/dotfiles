{
  config,
  pkgs,
  inputs,
  ...
}: {
  wayland.windowManager.hyprland = {
    enable = true;
    xwayland.enable = true;
    package = inputs.hyprland.packages.${pkgs.system}.hyprland.override {
      legacyRenderer = true;
      mesa = pkgs.mesa;
    };
  };
}
