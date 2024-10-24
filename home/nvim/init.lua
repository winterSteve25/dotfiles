
-- lazy.nvim installation
local lazypath = vim.fn.stdpath("data") .. "/lazy/lazy.nvim"
if not vim.loop.fs_stat(lazypath) then
  vim.fn.system({
    "git",
    "clone",
    "--filter=blob:none",
    "https://github.com/folke/lazy.nvim.git",
    "--branch=stable", -- latest stable release
    lazypath,
  })
end
vim.opt.rtp:prepend(lazypath)

-- settings
require("settings")

-- install plugins
require("lazy").setup("plugins")

-- setup
require("setups/lsp")
require("setups/auto-cmp")

-- setup keymap
require("keymaps")
