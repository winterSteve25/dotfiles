{ config, pkgs, ...}:

{
  	home.packages = with pkgs; [
		fira-code-nerdfont
  	];
}
