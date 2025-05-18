import unittest
import os
from PIL import Image
import ela

class TestELA(unittest.TestCase):
    def setUp(self):
        # Создаем тестовое изображение
        self.test_img_path = "test_input.jpg"
        img = Image.new('RGB', (100, 100), color='blue')
        img.save(self.test_img_path, quality=90)

    def tearDown(self):
        # Удаляем временные файлы
        if os.path.exists(self.test_img_path):
            os.remove(self.test_img_path)
        if os.path.exists("test_output.jpg"):
            os.remove("test_output.jpg")

    def test_ela_processing(self):
        # Применяем ELA
        ela.get_ela(self.test_img_path, "test_output.jpg")

        # Проверяем, что выходной файл создан
        self.assertTrue(os.path.exists("test_output.jpg"))

        # Проверяем, что это валидное изображение
        with Image.open("test_output.jpg") as img:
            self.assertEqual(img.size, (100, 100))

if __name__ == '__main__':
    unittest.main()
