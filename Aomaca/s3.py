from minio import Minio
from minio.error import S3Error
import os

host = os.getenv('MINIO_HOST')
access_key = os.getenv('MINIO_ROOT_USER')
secret_key = os.getenv('MINIO_ROOT_PASSWORD')

client = Minio(
    host,
    access_key=access_key,
    secret_key=secret_key
)

def upload_image(bucket_name: str, object_name: str, file_path: str) -> None:
    try:
        client.fput_object(bucket_name, object_name, file_path)
    except S3Error as err:
        print(err)

def get_image(bucket_name: str, object_name: str, file_path: str) -> None:
    try:
        client.fget_object(bucket_name, object_name, file_path)
    except S3Error as err:
        print(err)
