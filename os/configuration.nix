{ config, pkgs, ... }:

{
	imports = [
		./hardware-configuration.nix
		./services.nix
		./network.nix
		./programs.nix
	];

	options = {};
  	config = {
		# Bootloader.
		boot.loader.systemd-boot.enable = true;
		boot.loader.systemd-boot.configurationLimit = 5;
		boot.loader.efi.canTouchEfiVariables = true;
		boot.kernelPackages = pkgs.linuxPackages_latest;

		# boot.initrd.kernelModules = [ "amdgpu" ];
		hardware.opengl = {
			enable = true;
			driSupport = true;
			driSupport32Bit = true;
	#		extraPackages = with pkgs; [ mesa mesa.drivers ];
	#		extraPackages32 = with pkgs; [ driversi686Linux.mesa ];
		};

		programs.nix-ld.enable = true;
		programs.nix-ld.libraries = with pkgs; [
			libpulseaudio
			libGL
			glfw
			openal
			stdenv.cc.cc.lib
			flite
		];

		# Set your time zone.
		time.timeZone = "America/Vancouver";
		i18n.defaultLocale = "en_CA.UTF-8";

		# Define a user account. Don't forget to set a password with ‘passwd’.
		users.users.cadenz = {
			isNormalUser = true;
			description = "cadenz";
			extraGroups = [ "networkmanager" "wheel" ];
			packages = with pkgs; [];
		};

	  	nixpkgs.config.allowUnfree = true;
	  	nix.settings.experimental-features = [ "nix-command" "flakes" ];
		nix.gc = {
			automatic = true;
			dates = "weekly";
			options = "--delete-older-than 7d";
		};

		programs.steam = {
		  	enable = true;
		  	remotePlay.openFirewall = true; # Open ports in the firewall for Steam Remote Play
		  	dedicatedServer.openFirewall = true; # Open ports in the firewall for Source Dedicated Server
		 	localNetworkGameTransfers.openFirewall = true; # Open ports in the firewall for Steam Local Network Game Transfers
		};

		environment.systemPackages = with pkgs; [
			vim
			git
		];

		# Env Vars
		environment.sessionVariables = {
			WLR_NO_HARDWARE_CURSORS = "1";
			NIXOS_OZONE_WL = "1";
		};

	  	sound.enable = true;
	  	security.rtkit.enable = true;

		hardware.bluetooth.enable = true;
		hardware.bluetooth.powerOnBoot = true;
		services.blueman.enable = true;

		services.upower.enable = true;
	  	services.pipewire = {
			enable = true;
			alsa.enable = true;
			alsa.support32Bit = true;
			pulse.enable = true;
			jack.enable = true;
	  	};

		xdg.portal.enable = true;
		xdg.portal.extraPortals = [
			pkgs.xdg-desktop-portal-gtk
			pkgs.xdg-desktop-portal-hyprland
		];
	  
		programs.hyprland = {
			enable = true;
			xwayland.enable = true;
		};

		# This value determines the NixOS release from which the default
		# settings for stateful data, like file locations and database versions
		# on your system were taken. It‘s perfectly fine and recommended to leave
		# this value at the release version of the first install of this system.
		# Before changing this value read the documentation for this option
		# (e.g. man configuration.nix or on https://nixos.org/nixos/options.html).
	  	system.stateVersion = "24.05"; # Did you read the comment?
  	};
}
