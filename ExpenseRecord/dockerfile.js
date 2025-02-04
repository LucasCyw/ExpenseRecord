﻿ARG NET_IMAGE = 6.0 - bullseye - slim
FROM mcr.microsoft.com / dotnet / aspnet:${ NET_IMAGE } AS base
WORKDIR / app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT = Development

FROM mcr.microsoft.com / dotnet / sdk:${ NET_IMAGE } AS build
WORKDIR / src
COPY["src/TodoList.Api/TodoList.Api.csproj", "TodoList.Api/"]
COPY["src/TodoList.Application/TodoList.Application.csproj", "TodoList.Application/"]
COPY["src/TodoList.Domain/TodoList.Domain.csproj", "TodoList.Domain/"]
COPY["src/TodoList.Infrastructure/TodoList.Infrastructure.csproj", "TodoList.Infrastructure/"]
RUN dotnet restore "TodoList.Api/TodoList.Api.csproj"
COPY./ src.
    WORKDIR "/src/TodoList.Api"
RUN dotnet build "TodoList.Api.csproj" - c Release - o / app / build

FROM build AS publish
RUN dotnet publish--no - restore "TodoList.Api.csproj" - c Release - o / app / publish

FROM base AS final
WORKDIR / app
COPY--from = publish / app / publish.
    ENTRYPOINT["dotnet", "TodoList.Api.dll"]