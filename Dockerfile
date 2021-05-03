FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ./src/OK.ReadingIsGood.Host/OK.ReadingIsGood.Host.csproj ./
COPY ./src/OK.ReadingIsGood.Identity.Business/OK.ReadingIsGood.Identity.Business.csproj ./
COPY ./src/OK.ReadingIsGood.Identity.Contracts/OK.ReadingIsGood.Identity.Contracts.csproj ./
COPY ./src/OK.ReadingIsGood.Identity.Persistence/OK.ReadingIsGood.Identity.Persistence.csproj ./
COPY ./src/OK.ReadingIsGood.Product.Business/OK.ReadingIsGood.Product.Business.csproj ./
COPY ./src/OK.ReadingIsGood.Product.Contracts/OK.ReadingIsGood.Product.Contracts.csproj ./
COPY ./src/OK.ReadingIsGood.Product.Persistence/OK.ReadingIsGood.Product.Persistence.csproj ./
COPY ./src/OK.ReadingIsGood.Order.Business/OK.ReadingIsGood.Order.Business.csproj ./
COPY ./src/OK.ReadingIsGood.Order.Contracts/OK.ReadingIsGood.Order.Contracts.csproj ./
COPY ./src/OK.ReadingIsGood.Order.Persistence/OK.ReadingIsGood.Order.Persistence.csproj ./
COPY ./src/OK.ReadingIsGood.Shared.Core/OK.ReadingIsGood.Shared.Core.csproj ./
COPY ./src/OK.ReadingIsGood.Shared.MessageBus/OK.ReadingIsGood.Shared.MessageBus.csproj ./
COPY ./src/OK.ReadingIsGood.Shared.Persistence/OK.ReadingIsGood.Shared.Persistence.csproj ./

RUN dotnet restore OK.ReadingIsGood.Host.csproj

COPY ../src ./
RUN dotnet publish ./OK.ReadingIsGood.Host -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "OK.ReadingIsGood.Host.dll"]