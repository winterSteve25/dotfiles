{
  config,
  pkgs,
  ...
}: {
  programs.kitty.enable = true;
  programs.kitty.theme = "Atom";
  programs.kitty.font = {
    name = "FiraCode Nerd Font";
    size = 18;
  };
}
