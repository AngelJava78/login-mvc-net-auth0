# Auth0MvcDemo

Proyecto didáctico ASP.NET Core MVC (.NET 10) para estudiar autenticación con Auth0.

## Configuración en Auth0

Crea una aplicación de tipo **Regular Web Application**.

Configura:

- Allowed Callback URLs: `https://localhost:7184/callback`
- Allowed Logout URLs: `https://localhost:7184/`
- Allowed Web Origins: `https://localhost:7184`

Copia `Domain` y `Client ID` de Auth0 en `appsettings.json`.

## Ejecutar

```bash
dotnet restore
dotnet run
```

Abre `https://localhost:7184`.

## Qué ocurre al hacer Login

1. MVC ejecuta `ChallengeAsync` usando el esquema de Auth0.
2. El navegador es redirigido al Universal Login de Auth0.
3. Auth0 autentica al usuario y regresa a `/callback`.
4. El middleware valida la respuesta OpenID Connect y construye la identidad (`ClaimsPrincipal`).
5. ASP.NET Core crea su cookie de autenticación.
6. Las siguientes peticiones usan esa cookie para reconstruir `HttpContext.User`.

El **ID Token** representa información sobre la identidad autenticada y permite al cliente validar el resultado de autenticación. Sus datos se exponen en la aplicación como claims.

Este ejemplo **no solicita Access Token para una API**, porque no consume ninguna API protegida. Un Access Token sirve para autorizar llamadas a un recurso/API, no como sustituto de la sesión MVC.

## Logout

`AccountController.Logout` cierra la sesión de Auth0 y elimina la cookie local de ASP.NET Core.

## [Authorize]

`HomeController.Profile` usa `[Authorize]`. Si no existe una identidad autenticada, ASP.NET Core inicia el desafío de autenticación en lugar de ejecutar la Action normalmente.

## Auth0 vs SQL Server

**Auth0:** autenticación, credenciales, identidad externa, `sub`, nombre, email y otros claims de identidad.

**SQL Server:** datos propios de la aplicación, por ejemplo preferencias, perfil de negocio, relaciones y entidades del dominio. Si en el futuro relacionamos un registro local con Auth0, usaremos normalmente el identificador estable `sub` de Auth0. Nunca guardaremos la contraseña.

Las carpetas `Repositories`, `DTOs`, `Models` y `Services` se conservan para la evolución didáctica del proyecto. En esta primera versión no contienen implementaciones porque la autenticación básica no requiere SQL Server ni lógica de negocio adicional.
