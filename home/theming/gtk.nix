{ config, pkgs, ... }:

{
	gtk.enable = true;

	# gtk.theme.package = pkgs.tokyonight-gtk-theme;
	# gtk.theme.name = "Tokyonight-Dark-B";

	# gtk.theme.package = pkgs.adw-gtk3;
	# gtk.theme.name = "adw-gtk3";

	gtk.theme.package = (pkgs.graphite-gtk-theme.override {	
		themeVariants = [ "pink" ];
		colorVariants = [ "dark" ];
		tweaks = [ "darker" ];
	});
	gtk.theme.name = "Graphite-pink-Dark";

	gtk.iconTheme.package = pkgs.tokyonight-gtk-theme;
	gtk.iconTheme.name = "Tokyonight-Dark";
}
