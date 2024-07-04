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
		nodejs
		ripgrep
		clang
		bun
		btop
		libnotify
		rustup

		# programs
		google-chrome
		webcord
		grim
		slurp
		kitty

		# ides
		jetbrains.webstorm
		jetbrains.idea-ultimate
		jetbrains.rust-rover
	];

	home.file.".ideavimrc".source = ./ideavim;
}
