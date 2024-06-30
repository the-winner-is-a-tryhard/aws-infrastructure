from io import BytesIO
import boto3
import pandas as pd
import os
import requests

def lambda_handler(event, context):
    s3_client = boto3.client('s3')
    s3_bucket_name = os.environ.get('DATALAKE_S3_BUCKET_NAME')
    s3_object_uri = 'sleeper/player_data.parquet'
    sleeper_api_url = "https://api.sleeper.app/v1/players/nfl"
    response = requests.get(sleeper_api_url)
    response.raise_for_status()
    nfl_player_data = pd.read_json(response.text).transpose()
    parquet_buffer = BytesIO()
    nfl_player_data.to_parquet(parquet_buffer, index=False)
    s3_client.put_object(Bucket=s3_bucket_name, Key=s3_object_uri, Body=parquet_buffer.getvalue())
     