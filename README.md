# Create dotfile repository
To create a dotfile repository, edit files.txt to include all the config files you would like to include.
They are all relative to the home directory

After that, run `python setup.py`
This will move all the specified files to the directory where the setup script is located.

To use these dotfiles, run `python install.py`
This will create a symlink with all of the specified files/directories in their original location targeting the ones that have been moved into this repository
