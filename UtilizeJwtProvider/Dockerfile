FROM microsoft/aspnetcore-build:1.1.2
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
#FROM microsoft/aspnetcore
#WORKDIR /app
#COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "out/UtilizeJwtProvider.dll"]