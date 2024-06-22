{config, pkgs, ...}:

{
  home.packages = with pkgs; [
    wl-clipboard
    copyq
    google-chrome
    webcord-vencord
    grim
    slurp
    kitty
  ];
}
