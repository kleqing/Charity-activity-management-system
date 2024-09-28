# For first time installation:
0. Install node js first (Because of tailwind) (If you already have, don't install)
1. Run `dotnet restore` and `dotnet build` to check for errors
2. Run the migrations:  
**Before running make sure you are at the parrent folder (The folder that contains all the projects)**  
2.1. Powershell:  
  Add migration: `dotnet ef migrations add "Initial" --project Dynamics.DataAccess --startup-project Dynamics --context ApplicationDbContext  `  
  Update database: `dotnet ef database update --project Dynamics.DataAccess --startup-project Dynamics --context ApplicationDbContext  `  
2.2. Nuget:
  Select the default projects to Dynamics.DataAccess  
   Add migration: `Add-Migration initial -context ApplicationDbContext`  
   Update database: `Update-Database -context ApplicationDbContext`

3. Run the project

# For second time installation:
1. Run `dotnet restore` and `dotnet build` to check for errors
2. Run the project
