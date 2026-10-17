# Auth0MvcDemo + Books API

Aplicación ASP.NET Core MVC (.NET 10) didáctica con autenticación Auth0 y un CRUD de libros consumido desde una API REST externa.

## Flujo actual

1. El usuario inicia sesión en Auth0.
2. ASP.NET Core mantiene la sesión mediante su cookie de autenticación.
3. `[Authorize]` protege `BooksController`.
4. `BooksController` usa `IBooksApiService`.
5. `BooksApiService` usa `HttpClient` para llamar a `http://localhost:5190/api/books`.
6. La API accede a SQL Server mediante Dapper.

La API todavía no está protegida. Por tanto, la MVC no envía un Access Token en esta versión.

## Configuración

En `appsettings.json` configura Auth0 y la URL de la API:

```json
{
  "Auth0": {
    "Domain": "TU_DOMINIO.auth0.com",
    "ClientId": "TU_CLIENT_ID"
  },
  "BooksApi": {
    "BaseUrl": "http://localhost:5190/"
  }
}
```

Primero inicia `BooksApiDemo` y comprueba `http://localhost:5190/api/books`. Después inicia esta MVC.

## Access Token para las APIs

La aplicación MVC solicita ahora un Access Token de Auth0 para el audience `https://demo-api`.

Configura en `appsettings.json` el `ClientSecret` de la **Regular Web Application** de la MVC (no el Client Secret de la aplicación Machine to Machine utilizada por Users API):

```json
"Auth0": {
  "Domain": "dev-angel-java.us.auth0.com",
  "ClientId": "TU_CLIENT_ID",
  "ClientSecret": "TU_CLIENT_SECRET_DE_LA_APLICACION_MVC",
  "Audience": "https://demo-api"
}
```

Después de cambiar esta configuración, cierra sesión e inicia sesión nuevamente para que Auth0 emita un Access Token para el nuevo audience.

`BooksApiService` y `UsersApiService` recuperan el `access_token` asociado a la sesión autenticada y lo envían en cada llamada HTTP usando:

```text
Authorization: Bearer <access_token>
```

Las APIs todavía deben configurarse para validar ese token. Mientras sigan públicas, recibir el encabezado `Authorization` no cambia su comportamiento actual.
