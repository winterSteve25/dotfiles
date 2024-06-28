{ config, pkgs, ... }: 

{
	imports = [ ./qt.nix ./gtk.nix ]; 

	dconf.settings = {
    	"org/gnome/desktop/interface" = {
    		color-scheme = "prefer-dark";
    	};
  	};
}
