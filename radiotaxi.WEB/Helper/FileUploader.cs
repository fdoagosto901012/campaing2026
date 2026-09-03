using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon;
using radiotaxi.Model;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace radiotaxi.WEB.Helper
{

    public class FileUploader
    {
        private readonly HttpClient _httpClient;

        public FileUploader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> UploadFileAsync(string filePath, string uploadUrl)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("El archivo no existe.", filePath);
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var content = new StreamContent(fileStream);

                // Si es necesario, puedes agregar encabezados adicionales
                // content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await _httpClient.PostAsync(uploadUrl, content);
                response.EnsureSuccessStatusCode();

                return true; // La subida fue exitosa
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir el archivo: {ex.Message}");
                return false; // La subida falló
            }
        }
        /*
        public static File LocalUploadFileAsync(int userid, string partnerReference, HttpPostedFileBase file)
        {
            string path = "";
            string FileName = "";
            string extension = "";
            amazonPicture AmazonPicture = new amazonPicture();
            extension = Path.GetExtension(file.FileName).ToLower();
            FileName = partnerReference + "_" + userid + "_" + extension;
            path = Path.Combine("C:/amazon/", Path.GetFileName(FileName));
            file.SaveAs(path);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.CheckCertificateRevocationList = true;
            string accessKey = ConfigurationManager.AppSettings["AccessKey"];
            string secretKey = ConfigurationManager.AppSettings["SecretAccessKey"];
            string bucketName = null;
            extension = Path.GetExtension(path);
            var key = partnerReference + "_" + userid;
            bucketName = "partner-photos2-web";
            var fstream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            try
            {
                using (var client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.USEast1))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = key,
                        InputStream = fstream
                    };
                    var response = client.PutObject(request);

                    if (response == null)
                    {
                        return null;
                    }
                    //MessageBox.Show("Ocurrió un error al subir la imagen", "Error");
                }
                fstream.Close();
                AmazonPicture.bucket = bucketName;
                AmazonPicture.key = key;
                AmazonPicture.pictureTypeId = 2;
                AmazonPicture.DtCreated = DateTime.Now;
                return AmazonPicture;
            }
            catch (AmazonS3Exception s3Exception)
            {
                System.Console.WriteLine(s3Exception.StackTrace);
                return null;
            }
            catch (AmazonServiceException serException)
            {
                System.Console.WriteLine(serException.StackTrace);
                return null;
            }
            catch (Exception e)
            {
                //MessageBox.Show("Ocurrió un error al subir la imagen", "Error");
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                fstream.Close();
            }
        }

        */
    }
}