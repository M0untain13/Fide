import os
import logging
from typing import Optional, BinaryIO, Union
from boto3 import Session
from botocore.client import BaseClient
from botocore.exceptions import ClientError

logger = logging.getLogger(__name__)


class S3Storage:
    def __init__(
        self,
        endpoint_url: str = None,
        aws_access_key_id: str = None,
        aws_secret_access_key: str = None,
        region_name: str = None,
        bucket_name: str = None
    ):
        """
        Инициализация клиента для работы с S3-совместимым хранилищем.

        :param endpoint_url: URL хранилища (для MinIO - http://localhost:9000)
        :param aws_access_key_id: Ключ доступа
        :param aws_secret_access_key: Секретный ключ
        :param region_name: Регион (для Amazon S3)
        :param bucket_name: Имя корзины (bucket) по умолчанию
        """
        self.bucket_name = bucket_name
        self.session = Session(
            aws_access_key_id=aws_access_key_id,
            aws_secret_access_key=aws_secret_access_key,
            region_name=region_name
        )
        
        # Определяем клиента в зависимости от наличия endpoint_url
        if endpoint_url:
            # Для MinIO или других S3-совместимых хранилищ
            self.client = self.session.client(
                's3',
                endpoint_url=endpoint_url
            )
        else:
            # Для Amazon S3
            self.client = self.session.client('s3')

    def upload_file(
        self,
        file_path: str,
        object_name: str = None,
        bucket_name: str = None,
        extra_args: dict = None
    ) -> bool:
        """
        Загрузка файла в S3 хранилище.

        :param file_path: Путь к файлу на локальной системе
        :param object_name: Имя объекта в S3 (если None, будет использовано имя файла)
        :param bucket_name: Имя корзины (если None, будет использована корзина по умолчанию)
        :param extra_args: Дополнительные аргументы (например, {'ContentType': 'text/html'})
        :return: True если файл был загружен, иначе False
        """
        bucket = bucket_name or self.bucket_name
        if bucket is None:
            raise ValueError("Bucket name must be specified")
        
        # Если имя объекта не указано, используем имя файла
        if object_name is None:
            object_name = os.path.basename(file_path)

        try:
            self.client.upload_file(
                file_path,
                bucket,
                object_name,
                ExtraArgs=extra_args or {}
            )
        except ClientError as e:
            logger.error(f"Error uploading file {file_path} to {bucket}/{object_name}: {e}")
            return False
        return True

    def download_file(
        self,
        object_name: str,
        file_path: str = None,
        bucket_name: str = None
    ) -> bool:
        """
        Скачивание файла из S3 хранилища.

        :param object_name: Имя объекта в S3
        :param file_path: Путь для сохранения файла (если None, будет использовано имя объекта)
        :param bucket_name: Имя корзины (если None, будет использована корзина по умолчанию)
        :return: True если файл был скачан, иначе False
        """
        bucket = bucket_name or self.bucket_name
        if bucket is None:
            raise ValueError("Bucket name must be specified")
        
        # Если путь к файлу не указан, используем имя объекта
        if file_path is None:
            file_path = os.path.basename(object_name)

        try:
            self.client.download_file(
                bucket,
                object_name,
                file_path
            )
        except ClientError as e:
            logger.error(f"Error downloading file {bucket}/{object_name} to {file_path}: {e}")
            return False
        return True