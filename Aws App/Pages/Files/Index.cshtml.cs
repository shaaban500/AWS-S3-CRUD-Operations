using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Aws_App.AWSS3Helper;

namespace Aws_App.Pages.Files
{
    public class IndexModel : PageModel
    {
        private ServiceConfiguration _serviceConfiguration;
        public IndexModel(IOptions<ServiceConfiguration> serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration.Value;
        }

        public List<string> Keys;

        public async Task OnGetAsync()
        {

            var accesskey = _serviceConfiguration.AccessKey;
            var secretkey = _serviceConfiguration.SecretKey;
            var bucketName = _serviceConfiguration.BucketName;
            RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;

            var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            ListVersionsResponse listVersions = await s3Client.ListVersionsAsync(bucketName);

            Keys = listVersions.Versions.Select(c => c.Key).ToList();


        }

        public void OnPost()
        {

        }


    }
}
