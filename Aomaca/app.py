from flask import Flask, request
from s3 import upload_image, get_image
from ela import get_ela
from cnn import predict_result
import uuid
import os

app = Flask(__name__)
os.makedirs('Files', exist_ok=True)


@app.route('/healthcheck', methods=['GET'])
def healthcheck():
    return "ok"


@app.route('/analysis', methods=['POST'])
def analysis():
    data = request.get_json()

    image_for_analysis_path = f'Files/{uuid.uuid4()}'
    image_ela_path = f'Files/{uuid.uuid4()}'

    image_for_analysis_bucket = data['bucketName']
    image_ela_bucket = "aomaca"

    object_name_for_analysis = data['objectName']
    object_name_ela = f'ela_{object_name_for_analysis}'

    get_image(image_for_analysis_bucket, object_name_for_analysis, image_for_analysis_path)
    get_ela(image_for_analysis_path, image_ela_path)
    upload_image(image_ela_bucket, object_name_ela, image_ela_path)

    response = [predict_result(image_for_analysis_path), image_ela_bucket, object_name_ela]
    os.remove(image_for_analysis_path)
    os.remove(image_ela_path)

    return response


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80)