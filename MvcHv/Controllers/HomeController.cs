using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MvcHv.Models;

namespace MvcHv.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> TestPayload()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            var payload = await client.GetFromJsonAsync<TestApiResponse>("api/test");

            return Json(payload ?? new TestApiResponse());
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException)
        {
            return BackendUnavailable();
        }
    }

    [HttpPost]
    public async Task<IActionResult> TestPayloadWithInput([FromBody] TestApiRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            var response = await client.PostAsJsonAsync("api/testPost", new { x = request.X });
            response.EnsureSuccessStatusCode();
            var payload = await response.Content.ReadFromJsonAsync<TestApiResponse>();

            return Json(payload ?? new TestApiResponse());
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException)
        {
            return BackendUnavailable();
        }
    }

    [HttpPost]
    public async Task<IActionResult> SearchPayload([FromBody] SearchApiRequest request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Query) || request.TopK < 1)
        {
            return BadRequest(new { error = "A query is required and TopK must be at least 1." });
        }

        try
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            var response = await client.PostAsJsonAsync("api/search", new
            {
                query = request.Query,
                top_k = request.TopK
            });
            response.EnsureSuccessStatusCode();
            var payload = await response.Content.ReadFromJsonAsync<object>();

            return Json(payload ?? new { });
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException)
        {
            return BackendUnavailable();
        }
    }

    private ObjectResult BackendUnavailable()
    {
        return StatusCode(StatusCodes.Status502BadGateway, new
        {
            error = "The backend API request failed. Check that the API is running and configured correctly."
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
