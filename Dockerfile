#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KeyBooking_backend/KeyBooking_backend.csproj", "."]
RUN dotnet restore "./KeyBooking_backend.csproj"
COPY KeyBooking_backend .
WORKDIR "/src/."
RUN dotnet build "KeyBooking_backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KeyBooking_backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeyBooking_backend.dll"]