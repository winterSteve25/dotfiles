local home = os.getenv('HOME')
local jdtls = require('jdtls')

local root_markers = {'gradlew', 'mvnw', '.git'}
local root_dir = require('jdtls.setup').find_root(root_markers)

local workspace_folder = home .. "/.local/share/eclipse/" .. vim.fn.fnamemodify(root_dir, ":p:h:t")

require("jdtls.setup").add_commands()

local config = {
  flags = {
    debounce_text_changes = 80,
  },
  on_attach = function()
    require("on_attach_func")
  end,
  root_dir = root_dir,
  settings = {
    java = {
      format = {
        settings = {
          url = "/.local/share/eclipse/eclipse-java-google-style.xml",
          profile = "GoogleStyle",
        },
      },
      signatureHelp = { enabled = true },
      sources = {
        organizeImports = {
          starThreshold = 9999;
          staticStarThreshold = 9999;
        },
      },
      codeGeneration = {
        toString = {
          template = "${object.className}{${member.name()}=${member.value}, ${otherMembers}}"
        },
        hashCodeEquals = {
          useJava7Objects = true,
        },
        useBlocks = true,
      },
      configuration = {
        runtimes = {
          -- {
          --   name = "JavaSE-17",
          --   path = "/usr/lib/jvm/java-17-openjdk"
          -- },
          -- {
          --   name = "JavaSE-19",
          --   path = "/usr/lib/jvm/java-19-openjdk",
          -- },
          {
            name = "JavaSE-1.8",
            path = "/usr/lib/jvm/java-8-openjdk"
          },
        }
      }
    }
  },
  cmd = {
    "/usr/lib/jvm/java-19-openjdk/bin/java",
    '-Declipse.application=org.eclipse.jdt.ls.core.id1',
    '-Dosgi.bundles.defaultStartLevel=4',
    '-Declipse.product=org.eclipse.jdt.ls.core.product',
    '-Dlog.protocol=true',
    '-Dlog.level=ALL',
    '-Xmx1g',
    '--add-modules=ALL-SYSTEM',
    '--add-opens', 'java.base/java.util=ALL-UNNAMED',
    '--add-opens', 'java.base/java.lang=ALL-UNNAMED',
    -- '-javaagent:' .. home .. '/.local/share/nvim/mason/packges/jdtls/lombok.jar',
    '-jar', home .. '/.local/share/nvim/mason/packages/jdtls/plugins/org.eclipse.equinox.launcher_1.6.400.v20210924-0641.jar',
    '-configuration', home .. '/.local/share/nvim/mason/packges/jdtls/config_linux',
    '-data', workspace_folder,
  },
  capabilities = vim.lsp.protocol.make_client_capabilities(),
}

jdtls.start_or_attach(config)
