using System.Net.Http;
using System.Net.Http.Headers;



Task("Cake.Core.Scripting.HttpClient.New")
    .Does(() =>
{
    var client = new HttpClient();
});

Task("Cake.Core.Scripting.HttpClient.Handler")
    .Does(() =>
{
    var client = new HttpClient(new HttpClientHandler {
        UseDefaultCredentials = true
    });
});

Task("Cake.Core.Scripting.HttpClient.Header")
    .Does(() =>
{
    var client = new HttpClient { BaseAddress = new Uri("http://example.com/") };
    client.DefaultRequestHeaders
        .Accept
        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

Task("Cake.Core.Scripting.HttpClient")
    .IsDependentOn("Cake.Core.Scripting.HttpClient.New")
    .IsDependentOn("Cake.Core.Scripting.HttpClient.Handler")
    .IsDependentOn("Cake.Core.Scripting.HttpClient.Header");