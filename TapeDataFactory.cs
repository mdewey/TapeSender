using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

public class TapeDataFactory
{

  public TapeUploadObject GetThumbnailAndLength(TapeUploadObject tape)
  {
    var inputFile = new MediaFile { Filename = tape.FilePath };
    var outputFile = new MediaFile { Filename = @$"C:\Users\markt\Desktop\movie_project\test\{tape.FileName}.jpg" };

    using (var engine = new Engine())
    {
      engine.GetMetadata(inputFile);
      // Saves the frame located on the 15th second of the video.
      var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(60), };
      engine.GetThumbnail(inputFile, outputFile, options);
      tape.ImageUrl = outputFile.Filename;
      tape.Length = inputFile.Metadata.Duration.ToString();
    }

    return tape;
  }

  public List<TapeUploadObject> CreateList(string sourceDir)
  {
    var fileEntries = Directory.GetFiles(sourceDir, "*.mp3", SearchOption.TopDirectoryOnly);
    var list = fileEntries.Select(file => new TapeUploadObject
    {
      FilePath = file
    }).ToList();
    return list;
  }

  public static List<TapeUploadObject> CreateTapeUploadObjectsDummy(string filePath = "")
  {
    var TapeUploadObjects = new List<TapeUploadObject>();

     return TapeUploadObjects;
  }
}