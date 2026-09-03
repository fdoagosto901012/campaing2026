using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace Cajas.Utils
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "your-bucket-name";

        public S3Service(string accessKey, string secretKey, Amazon.RegionEndpoint region)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(awsCredentials, region);
        }

        public async Task UploadFileAsync(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                FilePath = filePath,
                ContentType = "application/octet-stream"
            };

            await _s3Client.PutObjectAsync(request);
        }


    }
}
