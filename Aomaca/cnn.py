import sys
import numpy as np
from PIL import Image
from keras.models import load_model

model = load_model("trained_model.keras")


def PrepareImage(path: str) -> np.ndarray:
    file = Image.open(path).convert("RGB")
    imageSize = (128, 128)
    return np.array(file.resize(imageSize)).flatten() / 255.0


def PredictResult(path: str) -> float:
    image = PrepareImage(path)
    image = image.reshape(-1, 128, 128, 3)
    y_pred = model.predict(image)
    return round(1 - y_pred[0][0], 3)


def predict_result(path: str) -> float:
    pass
