

local capabilities = vim.lsp.protocol.make_client_capabilities()
capabilities = require('cmp_nvim_lsp').default_capabilities(capabilities)

require("lspconfig").gdscript.setup{
    capabilities = capabilities,
    on_attach = require("on_attach_func"),
}

vim.cmd(":LspStart")
