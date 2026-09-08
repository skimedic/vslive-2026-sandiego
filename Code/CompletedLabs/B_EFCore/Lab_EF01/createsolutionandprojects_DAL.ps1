dotnet new globaljson --sdk-version 10.0.100 --roll-forward feature --force --test-runner Microsoft.Testing.Platform
dotnet new nugetconfig --force 
dotnet new install xunit.v3.templates
dotnet new sln -n AutoLot
dotnet new classlib -lang c# -n AutoLot.Models -o .\AutoLot.Models -f net10.0
dotnet sln AutoLot.slnx add AutoLot.Models
dotnet add AutoLot.Models package Microsoft.EntityFrameworkCore.SqlServer -v '[10.*,11.0)'
dotnet add AutoLot.Models package Microsoft.VisualStudio.Threading.Analyzers -v '[18.*,19.0)'
 
dotnet new classlib -lang c# -n AutoLot.Dal -o .\AutoLot.Dal -f net10.0
dotnet sln AutoLot.slnx add AutoLot.Dal
dotnet add AutoLot.Dal reference AutoLot.Models
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore.SqlServer -v '[10.*,11.0)'
dotnet add AutoLot.Dal package Microsoft.EntityFrameworkCore.Design -v '[10.*,11.0)'
dotnet add AutoLot.Dal package Microsoft.VisualStudio.Threading.Analyzers -v '[18.*,19.0)'
dotnet new classlib -lang c# -n AutoLot.Services -o .\AutoLot.Services -f net10.0
dotnet sln AutoLot.slnx add AutoLot.Services
dotnet add AutoLot.Services reference AutoLot.Models
dotnet add AutoLot.Services reference AutoLot.Dal
dotnet add AutoLot.Services package Microsoft.AspNetCore.Hosting.Abstractions -v '[2.3.*,3.0)'
dotnet add AutoLot.Services package Microsoft.AspNetCore.Http.Abstractions -v '[2.3.*,3.0)'
dotnet add AutoLot.Services package Microsoft.AspNetCore.Mvc.Abstractions -v '[2.3.*,3.0)'
dotnet add AutoLot.Services package Microsoft.Extensions.DependencyInjection.Abstractions -v '[10.*,11.0)'
dotnet add AutoLot.Services package Microsoft.VisualStudio.Threading.Analyzers -v '[18.*,19.0)'
dotnet add AutoLot.Services package Serilog -v '[4.*,5.0)'
dotnet add AutoLot.Services package Serilog.AspNetCore -v '[9.*,10.0)'
dotnet add AutoLot.Services package Serilog.Enrichers.Environment  -v '[3.*,4.0)'
dotnet add AutoLot.Services package Serilog.Sinks.Console -v '[6.*,7.0)'
dotnet add AutoLot.Services package Serilog.Sinks.File -v '[7.*,8.0)'
dotnet add AutoLot.Services package Serilog.Sinks.MSSqlServer -v '[9.*,10.0)'
dotnet new xunit3 -lang c# -n AutoLot.Dal.Tests -o .\AutoLot.Dal.Tests -f net10.0
dotnet sln AutoLot.slnx add AutoLot.Dal.Tests
dotnet add AutoLot.Dal.Tests reference AutoLot.Dal
dotnet add AutoLot.Dal.Tests reference AutoLot.Models
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore.SqlServer -v '[10.*,11.0)'
dotnet add AutoLot.Dal.Tests package Microsoft.EntityFrameworkCore.Design -v '[10.*,11.0)'
dotnet add AutoLot.Dal.Tests package Microsoft.Extensions.Configuration.Json -v '[10.*,11.0)'
dotnet add AutoLot.Dal.Tests package Microsoft.VisualStudio.Threading.Analyzers -v '[18.*,19.0)'
dotnet add AutoLot.Dal.Tests package xunit.v3.mtp-2 -v '[3.*,4.0)'


