using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;

namespace Cajas.Utils
{
    public class AwsS3 : iAwsS3
    {
        const string _BUCKETNAME = "credenciales";
        const string accessKey = "AKIAIKRX5JBCT26XE43A";
        const string secretKey = "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";
        string urlString;
        private AmazonS3Client _s3client;

        public string getImage(string BUCKETNAME = _BUCKETNAME, string objectKey = "")
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
            {
                BucketName = BUCKETNAME,
                Key = objectKey,
                Expires = DateTime.Now.AddMinutes(5)
            };
            try
            {
                urlString = _s3client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {

                    // Error : Check the provided AWS Credentials. "To sign up for service, go to http://aws.amazon.com/s3"
                }
                else
                {
                    // amazonS3Exception.Message;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }
            return urlString;
        }

        public AwsS3()
        {
            try
            {
                BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
                _s3client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void OnWriteObjectProgressEvent(object sender, WriteObjectProgressArgs e)
        {
            // Process progress update events.
        }
    }
}
