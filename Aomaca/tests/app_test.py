import unittest
from unittest.mock import patch
import os, sys
from ..src import app  # Импортируем Flask-приложение

class TestApp(unittest.TestCase):
    def setUp(self):
        app.app.config['TESTING'] = True
        self.client = app.app.test_client()
        os.makedirs('Files', exist_ok=True)

    def tearDown(self):
        # Очищаем временные файлы после тестов
        for file in os.listdir('Files'):
            os.remove(os.path.join('Files', file))

    def test_healthcheck(self):
        response = self.client.get('/healthcheck')
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.data.decode(), "ok")

    @patch('app.upload_image')  # Мокируем загрузку в S3
    @patch('app.get_image')     # Мокируем скачивание из S3
    @patch('app.predict_result', return_value=0.95)  # Мокируем CNN
    def test_analysis_endpoint(self, mock_predict, mock_get_image, mock_upload_image):
        # Подготавливаем тестовые данные
        test_data = {
            "BucketName": "test-bucket",
            "ObjectName": "test-image.jpg"
        }

        # Вызываем API
        response = self.client.post(
            '/analysis',
            json=test_data,
            content_type='application/json'
        )

        # Проверяем, что ответ корректен
        self.assertEqual(response.status_code, 200)
        self.assertIn("PredictedResult", response.json)
        self.assertEqual(response.json["PredictedResult"], 0.95)

        # Проверяем, что моки вызвались
        mock_get_image.assert_called_once()
        mock_upload_image.assert_called_once()
        mock_predict.assert_called_once()

if __name__ == '__main__':
    unittest.main()
