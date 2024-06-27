{ config, pkgs, ...}:

{
	home.packages = with pkgs; [
		# clipboard
		wl-clipboard
		copyq

		# utilities
		fzf
		unzip
		swww
		cmake
		gnumake
		nodejs
		ripgrep
		clang

		# programs
		google-chrome
		webcord
		grim
		slurp
		kitty
	];
}
