FROM microsoft/dotnet:2.2-sdk as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY MTGBot.csproj /build/

RUN dotnet restore ./build/MTGBot.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./MTGBot.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "MTGBot.dll"] 
