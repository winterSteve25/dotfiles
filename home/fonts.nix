{ config, pkgs, ...}:

let
	nerd_fonts = [
		"FantasqueSansMono"
	];
in
{
  home.packages = with pkgs; [
    (pkgs.nerdfonts.override { fonts = nerd_fonts; })
  ];
}
