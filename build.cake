#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

#addin Cake.GitVersioning&version=2.2.13
#addin Cake.Git
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/DotNetMDDocs/bin") + Directory(configuration);
var nugetDir = Directory("./nuget");

var version = GitVersioningGetVersion();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

string GetBranchName()
{
    var appveyorBranchName = EnvironmentVariable("APPVEYOR_REPO_BRANCH");
    if (!string.IsNullOrWhiteSpace(appveyorBranchName))
    {
        return appveyorBranchName;
    }

    return GitBranchCurrent(".").FriendlyName;
}

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(nugetDir);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration
    };

    DotNetCoreBuild("./src/DotNetMDDocs.sln", settings);
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
    });

    Information(GetBranchName());
});

Task("Push-Nuget")
    .WithCriteria(() => !string.IsNullOrWhiteSpace(EnvironmentVariable("NuGetApiKey")))
    .WithCriteria(() => GitBranchCurrent(".").FriendlyName == "master")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var settings = new NuGetPushSettings
    {
        Source = "https://api.nuget.org/v3/index.json",
        ApiKey = EnvironmentVariable("NuGetApiKey")
    };

    NuGetPush(GetFiles("./src/**/*.nupkg"), settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Push-Nuget");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
