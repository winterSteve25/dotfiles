{ config, pkgs, ... }:

{
    programs.kitty.enable = true;
    programs.kitty.theme = "Atom";
    programs.kitty.font = {
    	name = "DejaVu Sans";
		size = 18;
    };
}
