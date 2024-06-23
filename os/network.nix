{ config, pkgs, ... }:

{
	networking.hostName = "nixos"; 

  	## change dns server
  	networking.nameservers = [ "1.1.1.1" ];
  	networking.networkmanager.dns = "none";
  	networking.networkmanager.enable = true;

  	# Open ports in the firewall.
  	# networking.firewall.allowedTCPPorts = [ ... ];
  	# networking.firewall.allowedUDPPorts = [ ... ];
  	# Or disable the firewall altogether.
  	# networking.firewall.enable = false;
}
