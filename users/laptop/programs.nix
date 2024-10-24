{
  config,
  pkgs,
  ...
}: {
  imports = [
    ../../home/programs.nix
  ];

  home.packages = with pkgs; [
    brightnessctl
  ];
}
