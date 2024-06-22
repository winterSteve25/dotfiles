{ config, pkgs, ... }:

{
    services.openssh.enable = true;
    services.xserver = {
 	layout = "us";
    	xkbVariant = "";
    };
}
