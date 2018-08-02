#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

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
var buildDir = Directory("./src/DotNetMDDocs/DotNetMDDocs/bin") + Directory(configuration);
var artifactsDir = Directory("./artifacts");
var nugetDir = Directory("./nuget");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(artifactsDir);
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
});

Task("Publish-Application")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
     var settings = new DotNetCorePublishSettings
     {
         Framework = "netcoreapp2.1",
         Configuration = "Release",
         OutputDirectory = "./artifacts/",
         SelfContained = true,
         Runtime = "win-x86"
     };

     DotNetCorePublish("./src/DotNetMDDocs/DotNetMDDocs.csproj", settings);
});

Task("Package-Nuget")
    .IsDependentOn("Publish-Application")
    .Does(() =>
{
    var settings = new NuGetPackSettings 
    {
        OutputDirectory = "./nuget"
    };

    NuGetPack("./src/DotNetMDDocs/DotNetMDDocs.nuspec", settings);
});

Task("Push-Nuget")
    .WithCriteria(() => GitBranchCurrent(Directory(".")).FriendlyName == "master")
    .WithCriteria(() => !string.IsNullOrWhiteSpace(EnvironmentVariable("NuGetApiKey")))
    .IsDependentOn("Package-Nuget")
    .Does(() =>
{
    var settings = new NuGetPushSettings
    {
        Source = "https://api.nuget.org/v3/index.json",
        ApiKey = EnvironmentVariable("NuGetApiKey")
    };

    NuGetPush(GetFiles("./nuget/*.nupkg"), settings);
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
