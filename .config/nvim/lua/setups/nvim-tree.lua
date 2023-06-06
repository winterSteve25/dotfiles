-- Start nvim tree when Neovim is opened
vim.api.nvim_create_autocmd({ "VimEnter" }, {
    callback = function()
        require("nvim-tree.api").tree.open()
    end
})
