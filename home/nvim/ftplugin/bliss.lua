vim.lsp.start {
	name = "Bliss LS",
	cmd = {"/home/cadenz/Dev/Bliss/target/debug/bliss-ls"},
	capabilities = vim.lsp.protocol.make_client_capabilities()
}
