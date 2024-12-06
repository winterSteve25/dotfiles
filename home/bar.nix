{
  config,
  pkgs,
  ...
}: {
  programs.waybar = {
    enable = true;
    systemd.enable = true;
    style = ''
      ${builtins.readFile "${pkgs.waybar}/etc/xdg/waybar/style.css"}

      window#waybar {
        background: @theme_bg_color;
        border: none;
      }

      label {
          color: @theme_text_color;
          font-size: 18px;
          font-family: "Iosevka Nerd Font Mono", monospace;
      }
      
      .modules-right {
          margin-right: 8px;
      }
      
      .modules-left {
          margin-left: 8px;
      }
      
      #workspaces button.active {
          color: @theme_selected_fg_color;
      }
    '';
    settings = [{
      height = 40;
      layer = "top";
      position = "top";
      modules-center = [ "hyprland/window" ];
      modules-left = [ "tray" "hyprland/workspaces" ];
      modules-right = [ "pulseaudio" "battery" "network" "clock"];

      pulseaudio = {
        format = "{volume}% {icon} {format_source}";
        format-bluetooth = "{volume}% {icon} {format_source}";
        format-bluetooth-muted = " {icon} {format_source}";
        format-icons = {
          default = [ "" "" "" ];
        };
        format-muted = " {format_source}";
        format-source = "{volume}% ";
        format-source-muted = "";
        on-click = "pavucontrol";
      };
      clock = {
        tooltip-format = "{:%Y-%m-%d | %H:%M}";
      };
      network = {
        interval = 1;
        format-alt = "{ifname}: {ipaddr}/{cidr}";
        format-disconnected = "Not Connected";
        format-wifi = "{essid}  ";
      };
    }];
  };
}
