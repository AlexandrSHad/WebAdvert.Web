using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WebAdvert.Web.Services
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _configuration;

        public S3FileUploader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> UploadFileAsync(string fileName, Stream fileStream)
        {
            if (fileName == null)
            {
                throw new ArgumentException($"Parameter {nameof(fileName)} should not be null.");
            }

            var bucketName = _configuration.GetValue<string>("ImagesBucketName");

            using (var client = new AmazonS3Client())
            {
                if (fileStream.Length > 0 && fileStream.CanSeek)
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                }

                var request = new PutObjectRequest
                {
                    AutoCloseStream = true,
                    BucketName = bucketName,
                    InputStream = fileStream,
                    Key = fileName
                };

                var response = await client.PutObjectAsync(request);

                return response.HttpStatusCode == HttpStatusCode.OK;
            }
        }
    }
}
