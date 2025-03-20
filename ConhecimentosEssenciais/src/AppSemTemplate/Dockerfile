# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppSemTemplate.csproj", "AppSemTemplate/"]
RUN dotnet restore "AppSemTemplate/AppSemTemplate.csproj"
WORKDIR "AppSemTemplate"
COPY . .
RUN mkdir -p /root/.aspnet/https
RUN dotnet dev-certs https -v -ep /root/.aspnet/https/aspnetapp.pfx -p teste@123
RUN dotnet dev-certs https -v --trust
RUN dotnet build "AppSemTemplate.csproj" -c $BUILD_CONFIGURATION -o /app/build
#RUN dotnet build "AppSemTemplate.csproj" -c Release -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
COPY --from=build /root/.aspnet/https/aspnetapp.pfx /root/.aspnet/https/
RUN dotnet publish "AppSemTemplate.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#RUN dotnet publish "AppSemTemplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /root/.aspnet/https/aspnetapp.pfx /root/.aspnet/https/
VOLUME /var/data_protection_keys
ENV ASPNETCORE_ENVIRONMENT="Docker"
ENV ASPNETCORE_URLS="http://+:80;https://+:443;"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="teste@123"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.aspnet/https/aspnetapp.pfx
ENTRYPOINT ["dotnet", "AppSemTemplate.dll"]