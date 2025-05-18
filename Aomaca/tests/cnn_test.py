import unittest
from unittest.mock import patch, MagicMock
import numpy as np
from PIL import Image
import sys
sys.path.append("../src")
import cnn

class TestCNN(unittest.TestCase):
    @patch('keras.models.load_model')  # Мокируем загрузку модели
    def test_predict_result(self, mock_load_model):
        # Создаем фейковую модель, которая всегда возвращает 0.9
        mock_model = MagicMock()
        mock_model.predict.return_value = np.array([[0.1]])  # 1 - 0.1 = 0.9
        mock_load_model.return_value = mock_model

        # Создаем тестовое изображение
        test_img_path = "test_image.jpg"
        img = Image.new('RGB', (128, 128), color='red')
        img.save(test_img_path)

        # Проверяем предсказание
        result = cnn.predict_result(test_img_path)
        self.assertAlmostEqual(result, 0.9, places=2)  # Ожидаем ~0.9

        # Удаляем тестовое изображение
        import os
        os.remove(test_img_path)

if __name__ == '__main__':
    unittest.main()
