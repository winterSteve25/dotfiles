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

		# emacs
		ripgrep
		fd
		coreutils
		clang
		libvterm
		emacs

		# programs
		google-chrome
		webcord
		grim
		slurp
		kitty
	];
}
