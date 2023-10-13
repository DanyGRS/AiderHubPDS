using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace AiderHubAtual
{
    public class OpenStreetMapService
    {
        private const string BaseUrl = "https://nominatim.openstreetmap.org";

        public Coordinates GetCoordinates(string address)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("search", Method.Get);
            request.AddParameter("format", "json");
            request.AddParameter("q", address);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var content = response.Content;
                var results = JsonConvert.DeserializeObject<List<OpenStreetMapResponse>>(content);
                if (results.Count > 0)
                {
                    var result = results[0];
                    return new Coordinates(result.Latitude, result.Longitude);
                }
            }

            return null; // não achou ou deu erro
        }
    }

    public class OpenStreetMapResponse
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }

    public class Coordinates
    {
        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}
