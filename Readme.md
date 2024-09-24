# FIRST TIME installation only
1. Install node js (if you don't have)
2. Change directory into `Dynamics` and then type `npm install`
3. Build the solution to discover if any error is present
4. Drop your main (Not the Auth one) database
5. Run update-database in Nuget Package Console (in Visual Studio only)
- Side note: Package Console and Powershell have different commands
6. Run your application

# SECOND TIME AND AFTER:
1. Run `dotnet restore` and `dotnet build`
2. If there is any new migrations, run `update-database -context dbname`

