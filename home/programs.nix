{ config, pkgs, ...}:

{
	home.packages = with pkgs; [
		# clipboard
		wl-clipboard
		copyq

		# sys libs
		glfw

		# utilities
		fzf
		unzip
		swww
		cmake
		gnumake
		clang
		ninja
		nodejs
		ripgrep
		bun
		btop
		libnotify
		rustup
		python3
		gimp
		brightnessctl
		pavucontrol
		networkmanagerapplet

		# programs
		google-chrome
		webcord
		grim
		slurp
		kitty

		racket

		# ides
		jetbrains.idea-ultimate
	];

	home.file.".ideavimrc".source = ./ideavim;
}
