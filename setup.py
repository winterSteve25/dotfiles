import shutil
import os

with open("files.txt") as f:

    home_dir = os.path.expanduser("~")
    files = f.readlines()

    for file in files:
        file = file.replace("\n", "")

        if not file:
            continue

        abs_path = os.path.join(home_dir, file)
        shutil.move(abs_path, os.getcwd() + "/" + file)
