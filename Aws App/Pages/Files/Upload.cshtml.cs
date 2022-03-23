using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Aws_App.AWSS3Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Aws_App.Pages.Files
{
    public class UploadModel : PageModel
    {
        private ServiceConfiguration _serviceConfiguration;
        public UploadModel(IOptions<ServiceConfiguration> serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration.Value;
        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync(IFormFile? file)
        {


            if (file == null)
            {
                ModelState.AddModelError(string.Empty, "Please, choose file");
                return Page();
            }


            var accesskey = _serviceConfiguration.AccessKey;
            var secretkey = _serviceConfiguration.SecretKey;
            var bucketName = _serviceConfiguration.BucketName;
            RegionEndpoint bucketRegion = RegionEndpoint.APSouth1;

            var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);

            var fileTransferUtility = new TransferUtility(s3Client);
            await using var newMemoryStream = new MemoryStream();
            await file.CopyToAsync(newMemoryStream);
            var documentName = file.FileName;

            try
            {
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    Key = documentName,
                    CannedACL = S3CannedACL.PublicRead,
                    InputStream = newMemoryStream,
                };
                fileTransferUtility.UploadAsync(fileTransferUtilityRequest).GetAwaiter().GetResult();

                return RedirectToPage("/Files/Index");

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected Error, Try again");
                return Page();
            }
        }
    }

}

