{ config, pkgs, ...}:

{
  home.packages = with pkgs; [
    # fonts
    (pkgs.nerdfonts.override { fonts = [ "FantasqueSansMono" ]; })

    # clipboard
    wl-clipboard
    copyq

    # programs
    google-chrome
    webcord
    grim
    slurp
    kitty
  ];
}
