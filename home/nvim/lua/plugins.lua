return {
    -- LSP Configuration & Plugins
    {
        'neovim/nvim-lspconfig',
        dependencies = {
            'williamboman/mason.nvim',
            'williamboman/mason-lspconfig.nvim',
            'folke/neodev.nvim',
            {
				'j-hui/fidget.nvim',
				tag = "legacy",
				opts = {}
			},
        },
    },

	-- Autocompletion
    {
        'hrsh7th/nvim-cmp',
        dependencies = {
            'hrsh7th/cmp-nvim-lsp',
            'hrsh7th/cmp-nvim-lsp-signature-help',
            'hrsh7th/cmp-buffer',
            'hrsh7th/cmp-path',
            'L3MON4D3/LuaSnip',
            'saadparwaiz1/cmp_luasnip',
        },
    },

    -- Inlayed Hints
    {
        "lvimuser/lsp-inlayhints.nvim",
        opts = {},
    },

	-- Folding
	{
		"kevinhwang91/nvim-ufo",
		dependencies = {
			"kevinhwang91/promise-async",
		}
	},

	-- Startup menu
	{
		'goolord/alpha-nvim',
		event = "VimEnter",
		dependencies = {
			'kyazdani42/nvim-web-devicons',

			-- Session management
			{
				"Shatur/neovim-session-manager",
				dependencies = { "plenary.nvim" },
				config = function ()
					local config = require("session_manager.config")
					require("session_manager").setup({
						autoload_mode = config.AutoloadMode.Disabled,
					})
				end
			},
		},
		config = function()
			local alpha = require'alpha'
			local dashboard = require'alpha.themes.dashboard'
			dashboard.section.header.val = {
				[[                               __                ]],
				[[  ___     ___    ___   __  __ /\_\    ___ ___    ]],
				[[ / _ `\  / __`\ / __`\/\ \/\ \\/\ \  / __` __`\  ]],
				[[/\ \/\ \/\  __//\ \_\ \ \ \_/ |\ \ \/\ \/\ \/\ \ ]],
				[[\ \_\ \_\ \____\ \____/\ \___/  \ \_\ \_\ \_\ \_\]],
				[[ \/_/\/_/\/____/\/___/  \/__/    \/_/\/_/\/_/\/_/]],
			}

			dashboard.section.buttons.val = {
				dashboard.button("e", "  New file" , ":ene <BAR> startinsert <CR>"),
				dashboard.button("d", "  Open Current Directory" , ":NvimTreeOpen<CR>"),
				dashboard.button("l", "󰔠  Load Session", ":SessionManager load_session<CR>"),
				dashboard.button("q", "  Quit NVIM" , ":qa<CR>"),
			}

			dashboard.config.opts.noautocmd = true
			vim.cmd[[autocmd User AlphaReady echo 'ready']]
			alpha.setup(dashboard.config)
		end,
	},

	-- Neoformatter
	{
		"sbdchd/neoformat",
	},

	-- Gitignore generator
	{
		"wintermute-cell/gitignore.nvim",
		 dependencies = {
			"nvim-telescope/telescope.nvim"
		 },
	},

    -- Auto save
    {
        "Pocco81/auto-save.nvim",
	    config = function()
		    require("auto-save").setup {
		        enabled = true,
            }
	    end
    },

    -- Surround with brackets and stuff
    {
        "kylechui/nvim-surround",
        opts = {},
    },

	-- Leap for searching
    {
        "ggandor/leap.nvim",
        dependencies = {
            "tpope/vim-repeat"
        },
        config = function()
            require('leap').add_default_mappings()
        end
    },

    -- Color Picker
    {
        "ziontee113/color-picker.nvim",
        opts = {},
    },

    -- Auto pairs
    {
        "windwp/nvim-autopairs",
        opts = {},
    },

	-- Notifications
	{
		"rcarriga/nvim-notify",
		lazy = false,
		config = function ()
			require("notify").setup {
				background_colour = "#000000",
			}
			vim.notify = require("notify")
		end
	},

    -- TELESCOPE
    {
        "nvim-telescope/telescope.nvim",
        dependencies = {
            "nvim-lua/plenary.nvim",
            {
                'nvim-telescope/telescope-fzf-native.nvim',
                build = 'make',
                cond = function()
                  return vim.fn.executable 'make' == 1
                end,
				}
        },
        config = function()
            require("telescope").setup({})
            require("telescope").load_extension("fzf")
        end
    },

	-- Telescope for code actions
    {
        'nvim-telescope/telescope-ui-select.nvim',
        config = function()
            -- This is your opts table
            require("telescope").setup {
                extensions = {
                    ["ui-select"] = {
                      require("telescope.themes").get_dropdown {
                        -- even more opts
                      }

                      -- pseudo code / specification for writing custom displays, like the one
                      -- for "codeactions"
                      -- specific_opts = {
                      --   [kind] = {
                      --     make_indexed = function(items) -> indexed_items, width,
                      --     make_displayer = function(widths) -> displayer
                      --     make_display = function(displayer) -> function(e)
                      --     make_ordinal = function(e) -> string
                      --   },
                      --   -- for example to disable the custom builtin "codeactions" display
                      --      do the following
                      --   codeactions = false,
                      -- }
                    }
                }
            }
            -- To get ui-select loaded and working with telescope, you need to call
            -- load_extension, somewhere after setup function:
            require("telescope").load_extension("ui-select")
        end
    },

    -- Indentation display
    {
        "lukas-reineke/indent-blankline.nvim",
		config = function ()
			require("indent_blankline").setup {
				show_current_context = true,
            	show_trailing_blankline_indent = true,
			}
		end,
    },

    -- TODOs
    {
        "folke/todo-comments.nvim",
        dependencies = { "nvim-lua/plenary.nvim" },
        lazy = false,
        opts = {}
    },

    -- Comment
    { 'numToStr/Comment.nvim', opts = {} },

    -- Toggle Term
    {
		'akinsho/toggleterm.nvim',
		opts = {
			direction = "float"
		}
	},

    -- Wilder
    {
        "gelguy/wilder.nvim",
        dependencies = {
            "romgrk/fzy-lua-native",
        },
        config = function()
     		local gradient = {
				"#f4dbd6", "#f0c6c6", "#f5bde6", "#c6a0f6", "#ed8796",
				"#ee99a0", "#f5a97f", "#eed49f", "#a6da95", "#8bd5ca",
				"#91d7e3", "#7dc4e4", "#8aadf4", "#b7bdf8"
			}

			local wilder = require('wilder')
			for i, fg in ipairs(gradient) do
				gradient[i] = wilder.make_hl('WilderGradient' .. i, 'Pmenu', {{a = 1}, {a = 1}, {foreground = fg}})
			end
            wilder.setup({modes = {':', '/', '?'}})
            wilder.set_option('use_python_remote_plugin', 0)

            wilder.set_option('pipeline', {
              wilder.branch(
                wilder.cmdline_pipeline({
                  fuzzy = 1,
                  fuzzy_filter = wilder.lua_fzy_filter(),
                }),
                wilder.vim_search_pipeline()
              )
            })

            wilder.set_option('renderer', wilder.renderer_mux({
              [':'] = wilder.popupmenu_renderer(
                    wilder.popupmenu_border_theme({
                        border = "rounded",
						highlights = {
							gradient = gradient
						},
                        highlighter = wilder.highlighter_with_gradient({
							wilder.lua_fzy_highlighter()
						}),
                        left = {
                          ' ',
                          wilder.popupmenu_devicons()
                        },
                        right = {
                          ' ',
                          wilder.popupmenu_scrollbar()
                        },
                    })
                ),
              	['/'] = wilder.wildmenu_renderer({
					highlights = {
						gradient = gradient
					},
					highlighter = wilder.highlighter_with_gradient({
						wilder.lua_fzy_highlighter()
					}),
            	}),
            }))
        end
    },


    -- File Tree
    {
        "nvim-tree/nvim-tree.lua",
        dependencies = {
            "kyazdani42/nvim-web-devicons",
        },
        config = function()
            require("nvim-tree").setup {
                renderer = {
                    group_empty = true,
                },
                filters = {
                    dotfiles = true,
                },
            }
        end,
    },

    -- Which Key
    { 'folke/which-key.nvim', opts = {} },

    -- Bottom status bar
    {
        'nvim-lualine/lualine.nvim',
        config = function()
            local lsp = {
                function()
                    local msg = 'No Active Lsp'
                    local buf_ft = vim.api.nvim_buf_get_option(0, 'filetype')
                    local clients = vim.lsp.get_active_clients()
                    if next(clients) == nil then
                      return msg
                    end
                    for _, client in ipairs(clients) do
                      local filetypes = client.config.filetypes
                      if filetypes and vim.fn.index(filetypes, buf_ft) ~= -1 then
                        return client.name
                      end
                    end
                    return msg
                end,
                icon = ' LSP:',
            }

            local config = {
                options = {
                    theme = "catppuccin"
                },
                sections = {
                    lualine_x = {
                        "fileformat",
                        "filetype",
                        lsp
                    },
                }
            }

            require("lualine").setup(config)
        end
    },

    -- Dev Icons
    "kyazdani42/nvim-web-devicons",

    -------------------------------------------
    -------------------THEMES------------------
    -------------------------------------------

	{
		"catppuccin/nvim",
		name = "catppuccin",
		config = function ()
			vim.cmd.colorscheme("catppuccin")
		end
	},

	{
		"dasupradyumna/midnight.nvim",
		-- config = function ()
		-- 	vim.cmd.colorscheme("midnight")
		-- end
	},

	{
		"ellisonleao/gruvbox.nvim",
		-- config = function ()
		-- 	require("gruvbox").setup({
		-- 		contrast = "hard"
		-- 	})
		-- 	vim.cmd.colorscheme("gruvbox")
		-- end
	},

	-------------------------------------------
    -------------LANGUAGE SPECIFICS------------
    -------------------------------------------

    -- Flutter
    {
        "akinsho/flutter-tools.nvim",
        opts = {
            lsp = {
                on_attach = require("on_attach_func")
            }
        },
        dependencies = {
            "nvim-lua/plenary.nvim",
            "stevearc/dressing.nvim",
        }
    },

    -- Yuck
    {
        "elkowar/yuck.vim"
    },
}
