using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.IO;
using System.Globalization;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3.Model;
using System.Net;
using System.Reflection;
using radiotaxi.Model;
using Microsoft.Ajax.Utilities;
using Amazon.Runtime;

namespace radiotaxi.WEB.Helper
{
    public class AmazonHelper
    {
        const string bucketName = "credenciales";
        const string bucketDocuments = "operatorfilearchiver";
        private const string _salt = "NaeihFsd3x";

        private const string bucketNameFile = "*** provide bucket name ***";
        private const string keyName = "*** provide a name for the uploaded object ***";
        private const string filePath = "*** provide the full path name of the file to upload ***";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

        public AmazonHelper()
        {

            string accessKey = ConfigurationManager.AppSettings["AccessKey"];
            string secretKey = ConfigurationManager.AppSettings["SecretAccessKey"];
        }

        public static string GenerateNameFile(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static string saveImageOnServer()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string getImage(string bucket, string nombre)
        {
            string accessKey = "AKIAIKRX5JBCT26XE43A";
            string secretKey = "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";
            try
            {
                IAmazonS3 client;
                using (client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, RegionEndpoint.USEast1))
                {
                    var expiryUrlRequest = new GetPreSignedUrlRequest() { BucketName = "credenciales", Key = bucket + "/" + nombre, Expires = System.DateTime.Now.AddMinutes(8) }; return client.GetPreSignedURL(expiryUrlRequest);
                }
            }
            catch (Exception e)
            {
                ////MessageBox.Show("Ocurrió un error al recuperar la imagen", "Error");
                Console.WriteLine(e);
                return "";
            }
        }

        public static string getImageUser(string bucket, string key)
        {
            string accessKey = "AKIAIKRX5JBCT26XE43A";
            string secretKey = "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";
            try
            {
                IAmazonS3 client;
                using (client = AWSClientFactory.CreateAmazonS3Client(accessKey, secretKey, RegionEndpoint.USEast1))
                {
                    var expiryUrlRequest = new GetPreSignedUrlRequest() { BucketName = bucket, Key = key, Expires = System.DateTime.Now.AddMinutes(8) }; return client.GetPreSignedURL(expiryUrlRequest);
                }
            }
            catch (Exception e)
            {
                ////MessageBox.Show("Ocurrió un error al recuperar la imagen", "Error");
                Console.WriteLine(e);
                return "";
            }
        }

        public static Image GetImageFromUrl(string url)
        {
            HttpClient client = new HttpClient();
            try
            {
                var stream = client.GetStreamAsync(url).Result;
                return Image.FromStream(stream);
            }
            catch (Exception)
            {
                ////MessageBox.Show("Error al obtener la imagen", "Error");
                return null;
            }
        }

        public static string Hash(string valueToHash)
        {
            var computedHash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(valueToHash + _salt));
            return Convert.ToBase64String(computedHash);
        }

        public static byte[] GetImageStream(string url)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(url);
                    return imageBytes;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static byte[] ToByteArray(Image image)
        {
            Byte[] data;

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

                data = memoryStream.ToArray();
                return data;
            }
        }

        public static string MigraDocFilenameFromByteArray(byte[] image)
        {
            return "base64:" +
                   Convert.ToBase64String(image);
        }

        public static byte[] LoadImage(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }

        public static Image toImage(byte[] array)
        {
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(array))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static Bitmap SetAlpha(Bitmap bmp, byte alpha)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");

            var data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            var line = data.Scan0;
            var eof = line + data.Height * data.Stride;
            while (line != eof)
            {
                var pixelAlpha = line + 3;
                var eol = pixelAlpha + data.Width * 4;
                while (pixelAlpha != eol)
                {
                    System.Runtime.InteropServices.Marshal.WriteByte(
                        pixelAlpha, alpha);
                    pixelAlpha += 4;
                }
                line += data.Stride;
            }
            bmp.UnlockBits(data);
            return bmp;
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static amazonPicture savePictureWebSite(int userid, string partnerReference, HttpPostedFileBase file)
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
    }
}