from PIL import Image, ImageChops, ImageEnhance
import uuid
import os


def get_temp_dir() -> str:
    return 'Temp'


def get_difference(path: str, quality: int, format: str) -> Image:
    resavedPath = f"{get_temp_dir()}\\{uuid.uuid4()}_resaved_image.jpg"
    originalImage = Image.open(path).convert(format)
    originalImage.save(resavedPath, "JPEG", quality=quality)
    resavedImage = Image.open(resavedPath)
    image = ImageChops.difference(originalImage, resavedImage)
    os.remove(resavedPath)
    return image


def get_enhance(image: Image, format: str) -> Image:
    if format == 'L':
        extrema = image.getextrema()
    elif format == 'RGB':
        extrema = image.getextrema()[0]
    else:
        raise Exception(f'Invalid argument: {format}')
    maxDifference = max(extrema)
    if maxDifference == 0:
        maxDifference = 1
    scale = 255.0 / maxDifference
    return ImageEnhance.Brightness(image).enhance(scale)


def get_ela(image_for_analysis_path: str, image_ela_path: str) -> None:
    image = get_difference(image_for_analysis_path, 90, 'RGB')
    image = get_enhance(image, 'RGB')
    image.save(image_ela_path)
