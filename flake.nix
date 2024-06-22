{
    description = "Configuration";

    inputs = {
	hyprland.url = "github:hyprwm/Hyprland";
    	nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.05";
	home-manager = {
	    url = "github:nix-community/home-manager/release-24.05";
            inputs.nixpkgs.follows = "nixpkgs";
        };
	split-monitor-workspaces = {
	    url = "github:Duckonaut/split-monitor-workspaces";
	    inputs.hyprland.follows = "hyprland";
	};
    };

    outputs = { self, nixpkgs, home-manager, ... }@inputs: 
    let
	system = "x86_64-linux";
	pkgs = nixpkgs.legacyPackages.${system};
    in {
	nixosConfigurations = {
	    nixos = nixpkgs.lib.nixosSystem {
		inherit system;
		specialArgs = { inherit inputs; };
		modules = [ 
		    ./os/configuration.nix 
 		];
	    };
	};
	homeConfigurations = {
	    cadenz = home-manager.lib.homeManagerConfiguration {
		inherit pkgs;
		extraSpecialArgs = { inherit inputs; };
		modules = [ 
		    ./home/home.nix
	        ];
	    };
	};
    };

}
