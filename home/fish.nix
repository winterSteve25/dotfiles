{ config, pkgs, ... }:

{
	programs.fish.enable = true;
	programs.fish.interactiveShellInit = ''
		set -g fish_greeting
		set -gx PATH "$HOME/.emacs.d/bin" $PATH

		# Colors
		set -l foreground d3c6aa
		set -l selection 493840
		set -l comment 859289
		set -l red e67e80
		set -l orange e69875
		set -l yellow dbbc7f
		set -l green a7c080
		set -l purple d699b6
		set -l cyan 83c092
		set -l pink 7fbbb3

		# Syntax Highlighting Colors
		set -g fish_color_normal $foreground
		set -g fish_color_command $cyan
		set -g fish_color_keyword $pink
		set -g fish_color_quote $yellow
		set -g fish_color_redirection $foreground
		set -g fish_color_end $orange
		set -g fish_color_error $red
		set -g fish_color_param $purple
		set -g fish_color_comment $comment
		set -g fish_color_selection --background=$selection
		set -g fish_color_search_match --background=$selection
		set -g fish_color_operator $green
		set -g fish_color_escape $pink
		set -g fish_color_autosuggestion $comment

		# Completion Pager Colors
		set -g fish_pager_color_progress $comment
		set -g fish_pager_color_prefix $cyan
		set -g fish_pager_color_completion $foreground
		set -g fish_pager_color_description $comment
		set -g fish_pager_color_selected_background --background=$selection
	'';

	programs.bash.enable = true;
	programs.bash.initExtra = ''
		if [[ $(${pkgs.procps}/bin/ps --no-header --pid=$PPID --format=comm) != "fish" && -z ''${BASH_EXECUTION_STRING} ]] then
			shopt -q login_shell && LOGIN_OPTION='--login' || LOGIN_OPTION=""
			exec ${pkgs.fish}/bin/fish $LOGIN_OPTION
		fi
	'';
}
