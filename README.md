## AspNet.Mvc.AssetVersioning

Adds a `UrlHelper.VersionedContent` URL extension helper to append SHA256 hash to content URLs in ASP.NET MVC 5.

## Install

    Install-Package AspNet.Mvc.AssetVersioning

## Usage

In `Views/web.config`, add:

```xml
<configuration>
  ...
  <system.web.webPages.razor>
    <pages ...>
      <namespaces>
        ...
        <add namespace="System.Web.Mvc.AssetVersioning"/>
      </namespaces>
    </pages>
  </system.web.webPages.razor>
</configuration>
```

You can then use the helper in `.cshtml` files like so:

```html
<script type="text/javascript" src="@Url.VersionedContent("~/scripts/jquery.js")"></script>
```

And the helper will append a query string parameter containing the SHA256 hash of the content. This is cached for the lifetime of the request cache (`HttpContext.Cache`).