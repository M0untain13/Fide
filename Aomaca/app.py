from flask import Flask, request
from flask_cors import CORS
from s3 import upload_image, get_image
from ela import get_ela
from cnn import predict_result
import uuid
import os
from waitress import serve
    

app = Flask(__name__)
CORS(app)
os.makedirs('Files', exist_ok=True)


@app.route('/healthcheck', methods=['GET'])
def healthcheck():
    return "ok"


@app.route('/analysis', methods=['POST'])
def analysis():
    data = request.get_json()

    image_for_analysis_path = f'Files/{uuid.uuid4()}.jpg'
    image_ela_path = f'Files/{uuid.uuid4()}.jpg'

    image_for_analysis_bucket = data.get('BucketName')
    image_ela_bucket = data.get('BucketName')

    object_name_for_analysis = data.get('ObjectName')
    object_name_ela = f'ela_{object_name_for_analysis}'

    get_image(image_for_analysis_bucket, object_name_for_analysis, image_for_analysis_path)
    get_ela(image_for_analysis_path, image_ela_path)
    upload_image(image_ela_bucket, object_name_ela, image_ela_path)

    response = {
        "PredictedResult": float(predict_result(image_for_analysis_path)), 
        "BucketName": image_ela_bucket,
        "ObjectName": object_name_ela
    }
    os.remove(image_for_analysis_path)
    os.remove(image_ela_path)

    return response


if __name__ == '__main__':
    serve(app, host="0.0.0.0", port=80)
