using System.Diagnostics;
using System.Net.Http.Json;
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
        var client = _httpClientFactory.CreateClient();
        var payload = await client.GetFromJsonAsync<TestApiResponse>("http://127.0.0.1:5000/api/test");

        return Json(payload ?? new TestApiResponse());
    }

    [HttpPost]
    public async Task<IActionResult> TestPayloadWithInput([FromBody] TestApiRequest request)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("http://127.0.0.1:5000/api/testPost", new { x = request.X });
        var payload = await response.Content.ReadFromJsonAsync<TestApiResponse>();

        return Json(payload ?? new TestApiResponse());
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
