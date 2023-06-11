local set_key = vim.keymap.set

set_key("n", "<M-CR>", vim.lsp.buf.code_action, { desc = "Show Code Actions" })
set_key("n", "<C-t>", ":ToggleTerm<CR>", { desc = "Toggle Terminal" })
set_key("n", "<leader>tt", ":NvimTreeToggle<CR>", { desc = "Toggle NvimTree" })

local builtin = require('telescope.builtin')
set_key('n', '<leader>ff', builtin.find_files, { desc = "Find Files"})
set_key('n', '<leader>fg', builtin.live_grep, { desc = "Find Word" })
set_key('n', '<leader>fh', builtin.help_tags, { desc = "Find Help Tag"})
set_key('n', '<leader>ft', ":TodoTelescope<CR>", { desc = "Find TODOs"})

local toggle_term = require("toggleterm").toggle
set_key("t", "<C-t>", toggle_term, { desc = "Toggle Terminal" })
set_key("t", "<esc>", toggle_term, { desc = "Toggle Terminal" })

set_key("n", "<leader>c", ":PickColor<CR>", { desc = "Pick color" })
set_key("n", "<leader>fF", ":Neoformat<CR>", { desc = "Formats current buffer" })

local gitignore = require("gitignore")
set_key("n", "<leader>gi", gitignore.generate, { desc = "Generates a gitignore file" })
