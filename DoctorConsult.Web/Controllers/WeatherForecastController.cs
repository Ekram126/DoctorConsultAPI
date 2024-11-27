using Microsoft.AspNetCore.Mvc;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Threading.Tasks;


namespace DoctorConsult.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // Helper method to extract video ID from URL
        static string GetVideoIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["v"];
        }

        [HttpGet]
        public async  Task<IEnumerable<WeatherForecast>> Get(string VIDEO_ID= "Eay7iLnYwLU")
        {





            //// Your YouTube API key
            //string apiKey = "AIzaSyAogwuFZCLVUVuwjYhsbXsY0cudCVBONtQ";

            //// URL of the YouTube video
            //string videoUrl = "https://www.youtube.com/watch?v=" + VIDEO_ID;

            //// Extract the video ID from the URL
            //string videoId = GetVideoIdFromUrl(videoUrl);

            //// Create the YouTube service
            //var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            //{
            //    ApiKey = apiKey,
            //    ApplicationName = GetType().ToString()
            //});

            //// Request video details
            //var request = youtubeService.Videos.List("snippet,contentDetails,statistics");
            //request.Id = videoId;

            //// Execute the request and get the response
            //VideoListResponse response = await request.ExecuteAsync();

            //// Extract video details from the response
            //if (response.Items.Count > 0)
            //{
            //    var video = response.Items[0];
            //    Console.WriteLine($"Title: {video.Snippet.Title}");
            //    Console.WriteLine($"Description: {video.Snippet.Description}");
            //    Console.WriteLine($"Published At: {video.Snippet.PublishedAt}");
            //    Console.WriteLine($"View Count: {video.Statistics.ViewCount}");
            //    Console.WriteLine($"Like Count: {video.Statistics.LikeCount}");
            //    Console.WriteLine($"Dislike Count: {video.Statistics.DislikeCount}");
            //}
            //else
            //{
            //    Console.WriteLine("Video not found.");
            //}

















            var date = DateTime.UtcNow;
            DateTime date2 = date.ToLocalTime();

            var rng = new Random();


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string Summary { get; set; }
}