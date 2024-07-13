using System.Text.Json;

public class TapeApi
{
  // static readonly HttpClient client = new HttpClient();

  public string TapeApiRoot { get; set; } = "https://****.azurewebsites.net/api/v2/tapes";


  public async Task<List<TapeUploadObject>> GetAllTapes()
  {
    var client = new HttpClient();
    var response = await client.GetAsync($"{TapeApiRoot}");
    var content = await response.Content.ReadAsStringAsync();
    var tapes = JsonSerializer.Deserialize<List<TapeUploadObject>>(content);
    return tapes ?? new List<TapeUploadObject>();
  }

  public async Task UploadMetaDataToApi(TapeUploadObject tape)
  {
    Console.WriteLine("sending meta data to API");
    Console.WriteLine(tape);
    try
    {
      //TODO: remove before sending to production
      var httpClientHandler = new HttpClientHandler();
      httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
      {
        return true;
      };
      var client = new HttpClient(httpClientHandler);
      var content = new StringContent(JsonSerializer.Serialize(tape), System.Text.Encoding.UTF8, "application/json");
      var response = await client.PostAsync(this.TapeApiRoot, content);
      response.EnsureSuccessStatusCode();
      string responseBody = await response.Content.ReadAsStringAsync();
      Console.WriteLine(responseBody);
    }
    catch (HttpRequestException e)
    {
      Console.WriteLine("\nException Caught!");
      Console.WriteLine("Message :{0} ", e.Message);
      Console.WriteLine("InnerException :{0} ", e.InnerException);
      Console.WriteLine(tape);
    }
  }

  public async Task Process(TapeUploadObject tape)
  {
    await UploadMetaDataToApi(tape);
  }
}