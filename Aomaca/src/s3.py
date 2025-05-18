import os
import S3Storage


host = os.getenv('MINIO_HOST')
access_key = os.getenv('MINIO_ROOT_USER')
secret_key = os.getenv('MINIO_ROOT_PASSWORD')


def __get_storage() -> S3Storage.S3Storage:
    return S3Storage.S3Storage(
        # TODO: исправить захардкоженный порт
        endpoint_url="http://"+host+":9000",
        aws_access_key_id=access_key,
        aws_secret_access_key=secret_key,
    )


def upload_image(bucket_name: str, object_name: str, file_path: str) -> None:
    storage = __get_storage()
    storage.upload_file(file_path, object_name, bucket_name)


def get_image(bucket_name: str, object_name: str, file_path: str) -> None:
    storage = __get_storage()
    storage.download_file(object_name, file_path, bucket_name)
