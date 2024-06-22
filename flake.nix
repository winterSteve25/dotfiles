{
    description = "Configuration";

    inputs = {
    	nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.05";
    };

    outputs = { self, nixpkgs, ... }: 
    let
	system = "x86_64-linux";
    in {
	nixosConfigurations = {
	    nixos = nixpkgs.lib.nixosSystem {
   	    	inherit system;
		modules = [ ./configuration.nix ];
	    };
	};
    };

}
