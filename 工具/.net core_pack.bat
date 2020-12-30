E:
del /Q E:\Develop\Git\nupkgs\.netcore
cd E:\Develop\Git\Common\Hx.Sdk.Entity
dotnet build
dotnet pack --output E:\Develop\Git\nupkgs\.netcore

cd E:\Develop\Git\Common\Hx.Sdk.Common
dotnet build
dotnet pack --output E:\Develop\Git\nupkgs\.netcore

cd E:\Develop\Git\Common\Hx.Sdk.NetCore.Cache
dotnet build
dotnet pack --output E:\Develop\Git\nupkgs\.netcore 

cd E:\Develop\Git\Common\Hx.Sdk.NetCore.Config
dotnet build
dotnet pack --output E:\Develop\Git\nupkgs\.netcore 

cd E:\Develop\Git\Common\Hx.Sdk.NetCore.Core
dotnet build
dotnet pack --output E:\Develop\Git\nupkgs\.netcore 

cd E:\Develop\Git\Common\Hx.Sdk.NetCore.ImageSharp
dotnet build
dotnet pack --output E:\Develop\Git\nupkgs\.netcore 

pause