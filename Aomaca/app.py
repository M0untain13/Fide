from flask import Flask

app = Flask(__name__)

@app.route('/healthcheck', methods=['GET'])
def healthcheck():
    return "ok"

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80)