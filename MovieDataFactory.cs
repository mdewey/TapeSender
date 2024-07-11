using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

public class MovieDataFactory
{


  Dictionary<string, string> LINE_MARKERS = new Dictionary<string, string>
  {
    ["movieName"] = "##",
    ["tags"] = "> tags:",
    ["timeStamps"] = "--",
    ["imageUrl"] = "> imageUrl:",
    ["length"] = "> length:",
    ["filePath"] = "> filePath:",
  };

  public MovieUploadObject GetThumbnailAndLength(MovieUploadObject movie)
  {
    var inputFile = new MediaFile { Filename = movie.FilePath };
    var outputFile = new MediaFile { Filename = @$"C:\Users\markt\Desktop\movie_project\test\{movie.FileName}.jpg" };

    using (var engine = new Engine())
    {
      engine.GetMetadata(inputFile);
      // Saves the frame located on the 15th second of the video.
      var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(60), };
      engine.GetThumbnail(inputFile, outputFile, options);
      movie.ImageUrl = outputFile.Filename;
      movie.Length = inputFile.Metadata.Duration.ToString();
    }

    return movie;
  }

  public List<MovieUploadObject> CreateListFromReadme(string FilePath)
  {
    var list = new List<MovieUploadObject>();
    MovieUploadObject? bucket = null;
    foreach (string line in System.IO.File.ReadLines(FilePath))
    {
      Console.WriteLine(line);
      if (line.Contains(LINE_MARKERS["movieName"]))
      {
        if (bucket != null)
        {
          list.Add(bucket);
        }
        bucket = new MovieUploadObject { Title = line.Replace(LINE_MARKERS["movieName"], "").Trim() };
      }
      else if (line.Contains(LINE_MARKERS["length"]))
      {
        if (bucket != null)
        {
          // bucket.Length = line.Replace(LINE_MARKERS["length"], "").Trim();
        }
      }
      else if (line.Contains(LINE_MARKERS["imageUrl"]))
      {
        if (bucket != null)
        {
          // bucket.ImageUrl = line
          //   .Replace(LINE_MARKERS["imageUrl"], "")
          //   .Replace(">", "")
          //   .Replace("<", "")
          //   .Trim();
        }
      }
      else if (line.Contains(LINE_MARKERS["filePath"]))
      {
        // file path 
        if (bucket != null)
        {
          bucket.FilePath = line.Replace(LINE_MARKERS["filePath"], "").Trim();
        }
      }
      else if (line.Contains(LINE_MARKERS["tags"]))
      {
        // url
        if (bucket != null)
        {
          bucket.Tags = line.Replace(LINE_MARKERS["tags"], "").Trim().Split(',').ToList();
        }
      }
      else if (line.Contains(LINE_MARKERS["timeStamps"]))
      {
        if (bucket != null)
        {
          var split = line.Split(LINE_MARKERS["timeStamps"]);
          if (bucket.VideoTimeStamps == null)
          {
            bucket.VideoTimeStamps = new List<VideoTimeStamp>();
          }
          bucket.VideoTimeStamps.Add(new VideoTimeStamp { Description = split[1].Trim(), TimeStamp = split[0].Trim() });
        }
      }
    }
    if (bucket != null)
    {
      list.Add(bucket);
    }
    return list;
  }

  public static List<MovieUploadObject> CreateMovieUploadObjectsDummy(string filePath = "")
  {
    var movieUploadObjects = new List<MovieUploadObject>();

    for (int i = 0; i < 10; i++)
    {
      var movieUploadObject = new MovieUploadObject
      {
        Title = $"Movie {i}",
        FilePath = filePath,
        VideoTimeStamps = new List<VideoTimeStamp>()
      };
      for (int j = 0; j < 10; j++)
      {
        movieUploadObject.VideoTimeStamps.Add(new VideoTimeStamp
        {
          Description = $"Description {j}",
          TimeStamp = $"00:0{j}:{100 % (j + 1)}"
        });
      }
      movieUploadObjects.Add(movieUploadObject);
    }

    return movieUploadObjects;
  }
}