import os
from pathlib import Path
import shutil

with open("files.txt") as f:

    home_dir = os.path.expanduser("~")
    files = f.readlines()

    for file in files:
        file = file.replace("\n", "")

        if not file:
            continue

        symlink_abs_path = os.path.join(home_dir, file)
        actual_path = os.getcwd() + "/" + file
        symlink_path_obj = Path(symlink_abs_path)

        if symlink_path_obj.exists():
            if str(symlink_path_obj.resolve()) == actual_path:
                continue
            else:
                shutil.rmtree(symlink_abs_path)

        os.symlink(actual_path, symlink_abs_path)
