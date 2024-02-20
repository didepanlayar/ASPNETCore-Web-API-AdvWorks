dotnet new webapi --no-https -n AdvWorksAPI;
dotnet add package Serilog.AspNetCore;
dotnet add package Serilog.Sinks.File;
dotnet add package System.IdentityModel.Tokens.Jwt;
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 6.0.14;
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.3;
docker cp Databases/AdventureWorksLT.bak mssql:/var/opt/mssql/data;
### Restore from Microsoft SQL with DBeaver
# RESTORE FILELISTONLY FROM DISK = '/var/opt/mssql/data/AdventureWorksLT.bak'
# GO

# RESTORE DATABASE data_adventureworkslt
# FROM DISK = '/var/opt/mssql/data/AdventureWorksLT.bak'
# WITH 
# 	MOVE 'AdventureWorksLT_Data' TO '/var/opt/mssql/data/AdventureWorksLT_Data.mdf',
# 	MOVE 'AdventureWorksLT_Log' TO '/var/opt/mssql/data/AdventureWorksLT_Log.ldf'
# GO

# SELECT name FROM sys.databases; 
