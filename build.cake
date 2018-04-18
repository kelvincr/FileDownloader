#addin "Cake.FileHelpers"
#addin "Cake.Npm"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "debug");
var solution = Argument("solution", "./Downloader.sln");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Example/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solution ,new DotNetCoreRestoreSettings
         {
         });
});

Task("ClientRestore")
.Does(() =>
{

		var settings = 
        new NpmInstallSettings 
        {
            WorkingDirectory = "src/FileAudit"
        };
    NpmInstall(settings);

});

Task("Build")
    .IsDependentOn("Restore")
	.IsDependentOn("ClientRestore")
    .Does(() =>
{


 DotNetCoreBuild(
            solution,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var projects = GetFiles("test/**/*.csproj");
        foreach(var project in projects)
        {
            DotNetCoreTest(
                project.GetDirectory().FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    Logger = "trx"
                });
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
