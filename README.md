# -hvFR

An ASP.NET Core MVC page for exercising a separate JSON API.

## Run locally

The project targets .NET 10. Start the MVC app from the repository root:

```sh
dotnet run --project MvcHv
```

The page calls a backend API at `http://127.0.0.1:5000/` by default. Set the
`BackendApi:BaseUrl` value in `MvcHv/appsettings.json` to the backend's base
URL, or override it with the `BackendApi__BaseUrl` environment variable. Keep
the trailing slash so the relative API paths resolve correctly.

The backend is expected to provide:

- `GET /api/test`
- `POST /api/testPost`, with a JSON `x` value
- `POST /api/search`, with JSON `query` and `top_k` values

Search requests require a non-empty query and a `top_k` value of at least 1.
Backend connection, HTTP, or JSON-response failures are reported to the page
as a `502 Bad Gateway` response.

## Build

```sh
dotnet build MvcHv
```