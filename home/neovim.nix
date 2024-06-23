{ config, pkgs, ... }:

let
	toLua = str: "lua << EOF\n${str}\nEOF\n";
in
{
	programs.neovim = {
		enable = true;
		viAlias = true;
		vimAlias = true;
		extraConfig = ''
			set tabstop=4
			set shiftwidth=4
			set clipboard^=unnamed,unnamedplus
		'';
		plugins = with pkgs.vimPlugins; [
			{
				plugin = comment-nvim;
				config = toLua "require(\"Comment\").setup()";
			}
			{
				plugin = nvim-tree-lua;
				config = toLua "require(\"nvim-tree\").setup()";
			}
			{
				plugin = nvim-web-devicons;
				config = toLua "require(\"nvim-web-devicons\").setup()";
			}

			(nvim-treesitter.withPlugins (p: [
				p.tree-sitter-nix
				p.tree-sitter-json
			]))
		];
	};
}
