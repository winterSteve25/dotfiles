{ config, pkgs, ... }:
{
	programs.starship.enable = true;
	programs.starship.enableFishIntegration = true;
	programs.starship.settings = {
		format = "[   ](green)\$git_branch\$directory\$nodejs\$rust\$golang\$php\n[  └──> ](green)";
	};
}
