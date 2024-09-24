# FIRST TIME installation only
1. Install node js (if you don't have)
2. Change directory into `Dynamics` and then type `npm install`
3. Build the solution to discover if any error is present
4. Drop your main (Not the Auth one) database
5. Run update-database in Nuget Package Console (in Visual Studio only)
## IMPORTANT:
### 1. IF YOU ARE USING POWERSHELL COMMANDS:
- Also please `cd` into the main folder where all 4 projects are located
- `dotnet ef migrations add --project Dynamics.DataAccess --startup-project Dynamics --context ApplicationDbContext`: add migration
- `dotnet ef database update --project Dynamics.DataAccess --startup-project Dynamics --context ApplicationDbContext`: Update database using existing migrations
### 2. Nuget Package Console (Visual Studio):
- If you are using Nuget Package Console, change the default project to Dynamics.DataAccess
- Make sure the `-context` param target the database class
- `update-database -context ApplicationDbContext`: Update database

- For other commands please use Google

6. Run your application
# SECOND TIME AND AFTER:
1. Run `dotnet restore` and `dotnet build`
2. If there is any new migrations, run `update-database -context dbname`