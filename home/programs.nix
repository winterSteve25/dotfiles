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

		# emacs
		ripgrep
		fd
		coreutils
		clang
		emacs

		# programs
		google-chrome
		webcord
		grim
		slurp
		kitty
	];
}
