FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DC-Tower_Elevator_Challenge/DC-Tower_Elevator_Challenge.csproj", "DC-Tower_Elevator_Challenge/"]
RUN dotnet restore "DC-Tower_Elevator_Challenge/DC-Tower_Elevator_Challenge.csproj"
COPY . .
WORKDIR "/src/DC-Tower_Elevator_Challenge"
RUN dotnet build "DC-Tower_Elevator_Challenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DC-Tower_Elevator_Challenge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DC-Tower_Elevator_Challenge.dll"]
