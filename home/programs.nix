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
		bun

		# programs
		google-chrome
		webcord
		grim
		slurp
		kitty

		# ides
		jetbrains.webstorm
		jetbrains.idea-ultimate
	];

	home.file.".ideavimrc".source = ./ideavim;
}
