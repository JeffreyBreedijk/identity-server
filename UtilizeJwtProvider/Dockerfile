FROM microsoft/dotnet:1.1-sdk
WORKDIR /app

# copy csproj and restore as distinct layers
#cd COPY *.csproj ./

# copy and build everything else
COPY . ./

RUN ["dotnet", "restore"]
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/UtilizeJwtProvider.dll"]