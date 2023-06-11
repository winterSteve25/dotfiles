import os

with open("files.txt") as f:

    home_dir = os.path.expanduser("~")
    files = f.readlines()

    for file in files:
        file = file.replace("\n", "")

        if not file:
            continue

        symlink_abs_path = os.path.join(home_dir, file)
        actual_path = os.getcwd() + "/" + file

        os.symlink(actual_path, symlink_abs_path)
