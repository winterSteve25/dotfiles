{ config, pkgs, ... }:

{
  imports = [
      ./hyprland.nix
      ./programs.nix
      ./neovim.nix
      ./fuzzel.nix
      ./kitty.nix
  ];

  nixpkgs.config.allowUnfree = true;

  home.username = "cadenz";
  home.homeDirectory = "/home/cadenz";    
  home.packages = with pkgs; [
    (pkgs.nerdfonts.override { fonts = [ "FantasqueSansMono" ]; })
  ];

  home.sessionVariables = {
    EDITOR = "vim";
  };

  # This value determines the Home Manager release that your configuration is
  # compatible with. This helps avoid breakage when a new Home Manager release
  # introduces backwards incompatible changes.
  #
  # You should not change this value, even if you update Home Manager. If you do
  # want to update the value, then make sure to first check the Home Manager
  # release notes.
  home.stateVersion = "24.05"; # Please read the comment before changing.
  programs.home-manager.enable = true;
}
