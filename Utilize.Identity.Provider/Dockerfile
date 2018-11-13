FROM microsoft/dotnet:2.1-sdk-alpine AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore -s https://utilize.myget.org/F/utilize/api/v3/index.json -s https://api.nuget.org/v3/index.json

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine3.7
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Utilize.Identity.Provider.dll"]