using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Aws_App.AWSS3Helper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Aws_App.Pages.Files
{
    public class DeleteModel : PageModel
    {
        private ServiceConfiguration _serviceConfiguration;
        public DeleteModel(IOptions<ServiceConfiguration> serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration.Value;
        }

        public async void OnGetAsync(string fileName)
        {

            if (fileName == null)
            {
                ModelState.AddModelError(string.Empty, "Please, Choose a file to delete");
                return;
            }

            var accesskey = _serviceConfiguration.AccessKey;
            var secretkey = _serviceConfiguration.SecretKey;
            var bucketName = _serviceConfiguration.BucketName;
            RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;
            var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            ListVersionsResponse listVersions = await s3Client.ListVersionsAsync(bucketName);
            List<string> keys = listVersions.Versions.Select(c => c.Key).ToList();
            var key = keys.FirstOrDefault(k => k == fileName);

            if (key == null)
            {
                return;
            }

            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,

            };

            try
            {
                var response = s3Client.DeleteObjectAsync(request).Result;
                return;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected Error, Try again");
                return;
            }

        }
    }
}
