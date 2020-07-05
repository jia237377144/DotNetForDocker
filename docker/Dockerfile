#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["JLP.Web/JLP.Web.csproj", "JLP.Web/"]
COPY ["JLP.DB/JLP.DB.csproj", "JLP.DB/"]
RUN dotnet restore "JLP.Web/JLP.Web.csproj"
COPY . .
WORKDIR "/src/JLP.Web/"

FROM build AS publish
RUN dotnet publish "JLP.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JLP.Web.dll"]