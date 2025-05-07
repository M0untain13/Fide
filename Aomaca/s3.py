import os
import boto3

host = os.getenv('MINIO_HOST')
access_key = os.getenv('MINIO_ROOT_USER')
secret_key = os.getenv('MINIO_ROOT_PASSWORD')

# Использование ресурса вместо клиента
s3_resource = boto3.resource(
    host,
    aws_access_key_id=access_key,
    aws_secret_access_key=secret_key,
)

def upload_image(bucket_name: str, object_name: str, file_path: str) -> None:
    bucket = s3_resource.Bucket(bucket_name)
    bucket.upload_file(file_path, object_name)

def get_image(bucket_name: str, object_name: str, file_path: str) -> None:
    bucket = s3_resource.Bucket(bucket_name)
    bucket.download_file(object_name, file_path)
