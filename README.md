# Moon.OData

[![Build status](https://ci.appveyor.com/api/projects/status/nxs33kk72okhsdbp?svg=true)](https://ci.appveyor.com/project/djanosik/moon-odata)
[![NuGet](https://img.shields.io/nuget/v/Moon.OData.svg)](https://www.nuget.org/packages/Moon.OData)

Simple and easy to use library for parsing OData query options in .NET applications. There is no documentation and will never be. 
If you still want to use this library, take a look at sample projects and / or source code.

```c#
[HttpGet("api/entities")]
public Task<IOData<Entity>> GetEntities(ODataOptions<Entity> options)
{
    // Do whatever you want with OData query options, 
    // build custom Micro ORM integration, etc.

    var query = new ODataSqlQuery(
        "SELECT FROM Entities WHERE OwnerId = @p0",
        10456, options
    );
}
```