
public class TapeUploadObject
{
  public string? Title { 
    get {
      var title = Path.GetFileNameWithoutExtension(FilePath);
      return title?.Split("-")[1];
    } 
  }

  public string? Id { 
    get {
      var title = Path.GetFileNameWithoutExtension(FilePath);
      return title?.Split("-")[0];
    } 
  }

  public string? FilePath { get; set; }

  public string? AwsKey { get; set; } = "";
  public string? AwsImageKey { get; set; }

  public string? FileName { get { return Path.GetFileName(FilePath); } }
  public string? ImageFile { get { return FilePath?.Replace("mp3", "jpg"); } }

  public string? ImageFileName { get { return Path.GetFileName(ImageFile); } }

  public string? Url { get; set; }
  public string? ImageUrl { get; set; } = "https://some/url";

  public string? Length { get; set; } = "00:45:00";

  public override string ToString()
  {
    return $"{Id} | {Title} | {Url} | {FilePath} | {ImageFile} | {Length}";
  }
}