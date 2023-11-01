from enum import StrEnum
from pathlib import Path
import os
import shutil
import json

class Preset(StrEnum):
    CONFIG = "CONFIG"

    def resolve(self, home_dir: str) -> Path:
        match self:
            case CONFIG:
                return Path(home_dir).joinpath(".config")

class Settings: 
    def __init__(self, preset: Preset = Preset.CONFIG, path: str = ""):
        self.preset = preset
        self.path = path

    def resolve(self, home_dir: str, file_name: str) -> Path:
        if self.path:
            return Path(self.path).joinpath(file_name)

        return self.preset.resolve(home_dir).joinpath(file_name)

def create_if_not_present(path: str):
    if not os.path.exists(path):
        with open(path, "x") as f:
            json.dump({}, f)

def delete_symlink_or_file(path: Path):
    if path.is_symlink():
        os.unlink(path)
    elif os.path.isdir(path):
        shutil.rmtree(path)
    else:
        os.remove(path)

def link_file(file: str, path: Path, lock: dict[str, str]):
    os.symlink(f"{os.getcwd()}/{file}", path)
    lock[file] = str(path)

def should_file_be_deleted(path: Path) -> bool:
    print(f"Warning: file at {path} already exists")
    delete = False

    while True:
        yesOrNo = input("Do you want to remove the file at that location? (y/n)")
        if yesOrNo == "y" or yesOrNo == "Y":
            delete = True
            break
        elif yesOrNo == "n" or yesOrNo == "N": 
            break

    print()
    return delete

home_dir = os.path.expanduser("~")
settings = {}
config_file_path = f"{os.getcwd()}/sysconfig.json"
lock_file_path = f"{os.getcwd()}/sysconfig.lock.json"
lock = {}
new_lock = {}

create_if_not_present(config_file_path)
create_if_not_present(lock_file_path)

# load config
with open(config_file_path, "r") as f:
    config = json.load(f)
    for file, setting in config.items():
        if "path" in setting:
            settings[file] = Settings(path=setting["path"])
        elif "preset" in setting:
            settings[file] = Settings(preset=Preset[setting["preset"]])

# load lock
with open(lock_file_path, "r") as f:
    lock = json.load(f)

# create symlinks
for file, setting in settings.items():
    symlink_path = setting.resolve(home_dir, file)
    
    if not file in lock:
        if os.path.exists(symlink_path):
            if should_file_be_deleted(symlink_path):
                delete_symlink_or_file(symlink_path)
                link_file(file, symlink_path, new_lock)
            else:
                continue
        else:
            link_file(file, symlink_path, new_lock)
    else:
        if symlink_path == lock[file]:
            if not os.path.exists(symlink_path):
                link_file(file, symlink_path, new_lock)
            else:
                new_lock[file] = str(symlink_path)
            continue
        
        delete_symlink_or_file(Path(lock[file]))
        link_file(file, symlink_path, new_lock)

# resolve removals
for file, path in lock.items():
    if not file in new_lock:
        delete_symlink_or_file(Path(path))

# save lock file
with open(lock_file_path, "w") as f:
    json.dump(new_lock, f)
