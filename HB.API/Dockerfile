#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HB.API/HB.API.csproj", "HB.API/"]
COPY ["HB.Service/HB.Service.csproj", "HB.Service/"]
COPY ["Core/Strateq.Core.Model/Strateq.Core.Model.csproj", "Core/Strateq.Core.Model/"]
COPY ["Core/Strateq.Core.Service/Strateq.Core.Service.csproj", "Core/Strateq.Core.Service/"]
COPY ["Core/Strateq.Core.Database/Strateq.Core.Database.csproj", "Core/Strateq.Core.Database/"]
COPY ["Core/Strateq.Core.Utilities/Strateq.Core.Utilities.csproj", "Core/Strateq.Core.Utilities/"]
COPY ["HB.Utilities/HB.Utilities.csproj", "HB.Utilities/"]
COPY ["HB.Database/HB.Database.csproj", "HB.Database/"]
COPY ["HB.Model/HB.Model.csproj", "HB.Model/"]
COPY ["Core/Strateq.Core.API/Strateq.Core.API.csproj", "Core/Strateq.Core.API/"]
RUN dotnet restore "HB.API/HB.API.csproj"
COPY . .
WORKDIR "/src/HB.API"
RUN dotnet build "HB.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HB.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HB.API.dll"]
