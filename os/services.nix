{
  config,
  pkgs,
  ...
}: {
  services.openssh.enable = true;
  services.xserver = {
    xkb.layout = "us";
    xkb.variant = "";
    videoDrivers = ["modesetting"];
  };
}
