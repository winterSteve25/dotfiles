{
  config,
  pkgs,
  ...
}: {
  # do `fc-cache -f` once finished
  home.packages = with pkgs; [
    nerd-fonts.fira-code
    nerd-fonts.iosevka
  ];
}
