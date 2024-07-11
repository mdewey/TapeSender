using System.Web;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

public class TapeToAwsConfig
{
  public string? AwsAccessKey { get; set; }
  public string? AwsSecretKey { get; set; }
  public string? AwsBucket { get; set; }

}


public class TapeToAws
{

  public string? AwsAccessKey { get; set; }
  public string? AwsSecretKey { get; set; }
  public string? AwsBucket { get; set; }

  public TapeToAws(TapeToAwsConfig config)
  {
    AwsAccessKey = config.AwsAccessKey;
    AwsSecretKey = config.AwsSecretKey;
    AwsBucket = config.AwsBucket;
  }

  public async Task<TapeUploadObject> UploadToAws(TapeUploadObject tape)
  {
    if (tape == null || tape.FilePath == null || tape.ImageUrl == null)
    {
      throw new ArgumentNullException(nameof(tape));
    }

    Console.WriteLine($"staring to upload {tape.FilePath}");
    var accessKeyID = this.AwsAccessKey;
    var secretAccessKey = this.AwsSecretKey;
    var bucketName = this.AwsBucket;
    var region = Amazon.RegionEndpoint.USEast1;
    var creds = new BasicAWSCredentials(accessKeyID, secretAccessKey);
    using (var client = new AmazonS3Client(creds, region))
    {
      Console.WriteLine("reading file");
      Console.WriteLine(tape);
      // upload screenshot
      using (var ms = new MemoryStream())
      {

        using (FileStream file = new FileStream(tape.ImageUrl, FileMode.Open, FileAccess.Read))
        {
          file.CopyTo(ms);

          var uploadRequest = new TransferUtilityUploadRequest
          {
            InputStream = ms,
            Key = tape.FileName?.Replace(".mp4", ".jpg"),
            BucketName = bucketName // bucket name of S3
          };

          var fileTransferUtility = new TransferUtility(client);
          Console.WriteLine("uploading sceenshot");
          await fileTransferUtility.UploadAsync(uploadRequest);
          tape.ImageUrl = $"https://{bucketName}.s3.amazonaws.com/{HttpUtility.UrlEncode(uploadRequest.Key)}";
          tape.AwsImageKey = uploadRequest.Key;
          Console.WriteLine($"uploaded sceenshot to {tape.ImageUrl}, aws key {tape.AwsImageKey}");
        }
      }
      // upload video

      using (FileStream file = new FileStream(tape.FilePath, FileMode.Open, FileAccess.Read))
      {
        var uploadRequest = new TransferUtilityUploadRequest
        {
          InputStream = file,
          Key = tape.FileName,
          BucketName = bucketName // bucket name of S3
        };

        var fileTransferUtility = new TransferUtility(client);
        Console.WriteLine("uploading file");
        await fileTransferUtility.UploadAsync(uploadRequest);
        tape.Url = $"https://{bucketName}.s3.amazonaws.com/{HttpUtility.UrlEncode(uploadRequest.Key)}";
        tape.AwsKey = uploadRequest.Key;
        Console.WriteLine("success!", tape.Url);
      }

    }

    return tape;
  }


}