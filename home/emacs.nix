{ config, pkgs, ... }:

{
	home.sessionPath = [
		"~/.emacs.d/bin/"
	];

	home.file.".doom.d/".source = ./doom;
	services.emacs.enable = true;
}
