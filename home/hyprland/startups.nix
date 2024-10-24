{
  config,
  pkgs,
  ...
}: let
  wallpapers = ../wallpapers;
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
in {
  wayland.windowManager.hyprland.settings.exec-once = [
    "ags"
    "copyq --start-server"
    "webcord"
    "swww-daemon"
    "swww img ${wallpapers}/a_small_town_with_many_houses.png"
  ];
}
