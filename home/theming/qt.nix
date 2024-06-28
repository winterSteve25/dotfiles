{ config, pkgs, ... }:

{
	qt.enable = true;
	qt.platformTheme.name = "gtk";

	qt.style.package = pkgs.adwaita-qt;
	qt.style.name = "adwaita-dark";
}
