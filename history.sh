dotnet new webapi --no-https -n AdvWorksAPI;
dotnet add package Serilog.AspNetCore;
dotnet add package Serilog.Sinks.File;
dotnet add package System.IdentityModel.Tokens.Jwt;
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 6.0.14;
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.3;
