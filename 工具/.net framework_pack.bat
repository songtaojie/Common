E:
del /Q E:\Develop\Git\nupkgs\.netframework 

cd E:\Develop\Git\Common\Hx.Sdk.NetFramework.Core
msbuild Hx.Sdk.NetFramework.Core.csproj -p:Configuration=Debug
nuget spec -Force
nuget pack -OutputDirectory E:\Develop\Git\nupkgs\.netframework

cd E:\Develop\Git\Common\Hx.Sdk.NetFramework.Cache
msbuild Hx.Sdk.NetFramework.Cache.csproj -p:Configuration=Debug
nuget spec -Force
nuget pack -OutputDirectory E:\Develop\Git\nupkgs\.netframework

cd E:\Develop\Git\Common\Hx.Sdk.NetFramework.Logs
msbuild Hx.Sdk.NetFramework.Logs.csproj -p:Configuration=Debug
nuget spec -Force
nuget pack -OutputDirectory E:\Develop\Git\nupkgs\.netframework

cd E:\Develop\Git\Common\Hx.Sdk.NetFramework.Web
msbuild Hx.Sdk.NetFramework.Web.csproj -p:Configuration=Debug
nuget spec -Force
nuget pack -OutputDirectory E:\Develop\Git\nupkgs\.netframework

pause