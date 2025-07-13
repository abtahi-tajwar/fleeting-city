#!/usr/bin/env python3

import sys
import os
from PIL import Image

def trim_image(filepath, output_dir):
    with Image.open(filepath) as img:
        bbox = img.getbbox()
        if not bbox:
            print(f"❌ Skipped (empty): {filepath}")
            return

        trimmed = img.crop(bbox)
        filename = os.path.basename(filepath)
        trimmed.save(os.path.join(output_dir, filename))
        print(f"✅ Trimmed: {filename}")

def main():
    if len(sys.argv) < 2:
        print("Usage: trim-sprite <file.png | folder>")
        sys.exit(1)

    input_path = sys.argv[1]
    output_dir = os.path.join(os.path.dirname(input_path), "trimmed")
    os.makedirs(output_dir, exist_ok=True)

    if os.path.isdir(input_path):
        for f in os.listdir(input_path):
            if f.lower().endswith(".png"):
                trim_image(os.path.join(input_path, f), output_dir)
    elif os.path.isfile(input_path) and input_path.lower().endswith(".png"):
        trim_image(input_path, output_dir)
    else:
        print("❌ Input must be a .png file or folder.")

if __name__ == "__main__":
    main()
