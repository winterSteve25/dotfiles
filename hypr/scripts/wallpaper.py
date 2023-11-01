import subprocess
import random

wallpapers = [
    "~/Wallpapers/1336595.png",
    "~/Wallpapers/1327357.png",
    "~/Wallpapers/1327363.png",
    "~/Wallpapers/1327365.png",
    "~/Wallpapers/1335808.png",
    "~/Wallpapers/1335809.png",
]

subprocess.run(
    "swww img " + wallpapers[random.randint(0, len(wallpapers) - 1)], 
    shell=True,
)
