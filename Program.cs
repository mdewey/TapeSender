// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

Console.WriteLine("Let's Go! Let's Go!");

var config = new ConfigurationBuilder()
      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
      .AddUserSecrets<Program>()
      .Build();

// var sourceFile = @"C:\Users\markt\Desktop\movie_project\test\README.md";
var sourceFile = @"D:\Homemovies\README.md";
var movies = new MovieDataFactory()
    .CreateListFromReadme(sourceFile).ToList();

foreach (var movie in movies)
{
  Console.WriteLine(movie);
  new MovieDataFactory().GetThumbnailAndLength(movie);
  await new MovieToAws(new MovieToAwsConfig
  {
    AwsAccessKey = config["HomeMovies:aws:accessKeyId"],
    AwsSecretKey = config["HomeMovies:aws:secret"],
    AwsBucket = config["HomeMovies:aws:bucket"]
  }).UploadToAws(movie);
  Console.WriteLine(movie);
  await new MovieApi().ProcessMovie(movie);
}

// get all movies and compare to list to see if any are missing
var moviesFromApi = await new MovieApi().GetAllMovies();
// write counts to console
Console.WriteLine($"Movies from api: {moviesFromApi.Count()}");
Console.WriteLine($"Movies from readme: {movies.Count}");
