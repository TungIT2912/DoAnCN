## Base image cho runtime
## 1) Base runtime image
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 7100
#EXPOSE 7101
#
## 2) Build image
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
## Copy csproj and restore
#COPY ["WebQuanLyNhaKhoa.csproj", "./"]
#RUN dotnet restore "WebQuanLyNhaKhoa.csproj"
#
## Copy all and publish
#COPY . .
#RUN dotnet publish "WebQuanLyNhaKhoa.csproj" -c Release -o /app/publish
#
## 3) Final image
#FROM base AS final
#WORKDIR /app
#
## Copy the published output from build stage
#COPY --from=build /app/publish .
#
## Copy the certificate into the final container
#COPY --from=build /src/aspnetapp.pfx /https/aspnetapp.pfx
#
## Set environment variables in final image
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Aa@123456789
#
#ENTRYPOINT ["dotnet", "WebQuanLyNhaKhoa.dll"]
#
#
##
##FROM mcr.microsoft.com/dotnet/aspnet:8.0
##ARG source
##WORKDIR /inetpub/wwwroot
##COPY ${source:-bin/release/net8.0/publish} .
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebQuanLyNhaKhoa.csproj", "./"]
RUN dotnet restore "WebQuanLyNhaKhoa.csproj"
COPY . .
RUN dotnet publish "WebQuanLyNhaKhoa.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /src/aspnetapp.pfx /https/
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Aa@123456789
ENTRYPOINT ["dotnet", "WebQuanLyNhaKhoa.dll"]