{
  config,
  pkgs,
  ...
}: {
  imports = [
    ../../home/home.nix
    ./hyprland.nix
    ../../home/programs.nix
    ../../home/fonts.nix
    ../../home/neovim.nix
    ../../home/fuzzel.nix
    ../../home/kitty.nix
    ../../home/fish.nix
    ../../home/starship.nix
    ../../home/ags.nix
    ../../home/java.nix
    ../../home/theming/theme.nix
  ];
}
