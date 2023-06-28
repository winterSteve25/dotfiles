if status is-interactive
	# remove greeting
	set fish_greeting ""

	alias ls 'ls --color=auto'
	alias grep 'grep --color=auto'

	alias yay-install 'yay -S --needed'
	alias yay-remove 'yay -Rs'
	alias yay-update 'yay -Sy'
	alias yay-update-all 'yay -Syu'
	alias yay-list 'yay -Q'
	alias yay-info 'yay -Qi'

	alias clean-system 'yay -Scc && rm -rf ~/.cache/* && cargo cache -a && yay -Rns $(yay -Qtdq)'

	alias ls 'exa --icons --group-directories-first'
	alias cat 'bat'
	alias tree 'tree -C'
	alias nuget 'mono /usr/local/bin/nuget.exe'
	alias vim 'nvim'

	# environment vars
	set -gx PATH "$HOME/.local/bin" $PATH
	set -gx PATH "$HOME/Applications/flutter/bin" $PATH
	set -gx CHROME_EXECUTABLE "/usr/bin/google-chrome-stable"

	# set theme
	source $HOME/.config/fish/themes/everforest.fish

	# start starship
	starship init fish | source
end
