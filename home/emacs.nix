{ config, pkgs, ... }:

{
	# not working sometimes as a workaround a line is added in fish.nix
	home.sessionPath = [
		"~/.emacs.d/bin/"
	];

	home.file.".doom.d/".source = ./doom;
	services.emacs.enable = true;
}
