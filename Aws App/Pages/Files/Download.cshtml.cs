using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Aws_App.AWSS3Helper;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace Aws_App.Pages.Files
{
    public class DownloadModel : PageModel
    {
        private ServiceConfiguration _serviceConfiguration;
        public DownloadModel(IOptions<ServiceConfiguration> serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration.Value;
        }

        public  async Task<IActionResult> OnGetAsync(string fileName)
        {
            var accesskey = _serviceConfiguration.AccessKey;
            var secretkey = _serviceConfiguration.SecretKey;
            var bucketName = _serviceConfiguration.BucketName;
            RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;

            var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            MemoryStream ms = null;
            using (var response = await s3Client.GetObjectAsync(getObjectRequest))
            {
                using (ms = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(ms);
                }
            }

            return File(ms.ToArray(), "application/octet-stream", fileName);

        }

        public void OnPost(string fileName)
        {
        }
    }
}





            /*using (TransferUtility transferUtility = new Amazon.S3.Transfer.TransferUtility(s3Client))
            {
                TransferUtilityDownloadRequest downloadRequest = new TransferUtilityDownloadRequest
                {
                    FilePath = filePath,
                    BucketName = bucketName,
                    Key = fileName,
                };
                transferUtility.Download(downloadRequest);

            }*//*
            var fileTransferUtility = new TransferUtility(s3Client);

            var objectResponse = await fileTransferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = fileName,
            });*//*
            MemoryStream ms = null;

            using (var response = await s3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            }
)) ;
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                throw new FileNotFoundException(string.Format("The document '{0}' is not found", fileName));

            
            return File(objectResponse.ResponseStream, objectResponse.Headers.ContentType);
            */
