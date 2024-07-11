
public class TapeUploadObject
{
  public string? Title { get; set; }

  public string? FilePath { get; set; }

  public string? AwsKey { get; set; } = "";
  public string? AwsImageKey { get; set; }

  public string? FileName { get { return Path.GetFileName(FilePath); } }
  public string? Url { get; set; }

  public string? ImageUrl { get; set; }
  public string? Length { get; set; }


  public List<String>? Tags { get; set; }

  public override string ToString()
  {
    return $"{Title} - {Url} - {FilePath}, tags {Tags?.Count} - Length: {Length}";
  }
}