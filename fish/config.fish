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
	alias set-pacman-mirrors 'sudo reflector --country 'America,' --latest 5 --age 2 --fastest 5 --protocol https --sort rate --save /etc/pacman.d/mirrorlist'

	alias cat 'bat'
	alias nuget 'mono /usr/local/bin/nuget.exe'
	alias vim 'nvim'

	# environment vars
	set -gx PATH "$HOME/.local/bin" $PATH
	set -gx PATH "$HOME/Applications/flutter/bin" $PATH

	set -gx CHROME_EXECUTABLE "/usr/bin/google-chrome-stable"
	set -gx VISUAL "nvim"
	
	source $HOME/.config/fish/themes/everforest.fish

	starship init fish | source
	zoxide init fish --cmd cd | source
end
