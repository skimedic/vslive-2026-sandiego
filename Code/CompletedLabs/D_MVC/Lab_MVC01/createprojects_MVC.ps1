rem create the ASP.NET Core Web App (MVC) project and add it to the solution
dotnet new mvc -lang c# -n AutoLot.Mvc -au none -o .\AutoLot.Mvc -f net10.0
dotnet sln AutoLot.slnX add AutoLot.Mvc
dotnet add AutoLot.Mvc reference AutoLot.Models
dotnet add AutoLot.Mvc reference AutoLot.Dal
dotnet add AutoLot.Mvc reference AutoLot.Services

rem add packages
dotnet add AutoLot.Mvc package LigerShark.WebOptimizer.Core -v '[3.*,4)'
dotnet add AutoLot.Mvc package Microsoft.Web.LibraryManager.Build -v '[3.*,4.0)'
dotnet add AutoLot.Mvc package Microsoft.EntityFrameworkCore.SqlServer -v '[10.*,11.0)'
dotnet add AutoLot.Mvc package Microsoft.EntityFrameworkCore.Design -v '[10.*,11.0)'
dotnet add AutoLot.Mvc package Microsoft.VisualStudio.Threading.Analyzers -v '[18.*,19.0)'

pause
