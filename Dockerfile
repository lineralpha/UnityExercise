#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS pre-build
COPY . ./src/
RUN mkdir ./proj && cd ./src && \
  find . -type f -a \( -iname "*.sln" -o -iname "*.csproj" \) \
    -exec cp --parents "{}" ../proj/ \;

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# Copy only the project files to preserve directory structure 
COPY --from=pre-build ./proj ./src/
RUN dotnet restore "./src/UnityExercise.Web/UnityExercise.Web.csproj"
# Copy everything else
COPY --from=pre-build ./src ./src

WORKDIR "./src/UnityExercise.Web"
RUN dotnet build "UnityExercise.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UnityExercise.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls -l -a /app
ENTRYPOINT ["dotnet", "UnityExercise.Web.dll"]