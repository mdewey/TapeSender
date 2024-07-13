// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

Console.WriteLine("Let's Go! Let's Go!");

var config = new ConfigurationBuilder()
      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
      .AddUserSecrets<Program>()
      .Build();

var sourceDir = @"C:\Users\markt\Documents\Audacity";
var tapes = new TapeDataFactory().CreateList(sourceDir).ToList();

foreach (var tape in tapes)
{
  Console.WriteLine(tape);
  // await new TapeToAws(new TapeToAwsConfig
  // {
  //   AwsAccessKey = config["DadsTapes:aws:accessKeyId"],
  //   AwsSecretKey = config["DadsTapes:aws:secret"],
  //   AwsBucket = config["DadsTapes:aws:bucket"]
  // }).UploadToAws(tape);
  // Console.WriteLine(tape);
  // await new TapeApi().Process(tape);
}

// get all movies and compare to list to see if any are missing
// var tapesFromApi = await new TapeApi().GetAllTapes();
// write counts to console
// Console.WriteLine($"Tapes from api: {tapesFromApi.Count()}");
Console.WriteLine($"Tapes from readme: {tapes.Count}");
