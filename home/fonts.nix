{ config, pkgs, ...}:

{
	# do `fc-cache -f` once finished
  	home.packages = with pkgs; [
		fira-code-nerdfont
		iosevka
  	];
}
