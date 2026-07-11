# Chat
## Popis
Jednoduchá WPF aplikácia na real-time chatovanie.
### Stack
C#, WPF, MySQL, ASP.NET
### Zadanie
Navrhnúť a spracovať jednoduchú WPF aplikáciu na real-time chatovanie, 
s databázou a WebSocket/REST API… 

Vsetko stačí keď bude lokálne medzi sebou komunikovať. 

Základom je aby to bolo real-time, 
ak neovládate WebSocket môžete použiť REST 
(samozrejme pri REST platí aby to čo najviac pôsobilo real-time, 
bez extrémnej záťaže pre API poprosím).

Hranice nie sú dané, pravidlá sú jednoduché : 
- Dodržať Deadline: 10.7.2026 20:00
- Odovzdať riešenie (App, API) cez GitHub (public repo)
## Projekty
- WpfChat - WPF klient chatovacia aplikacia
- WpfChat.Domain - domenova kniznica
- WpfChat.WebApi - Web Api server pre spravu chatovania
## Systémové požiadavky
### ASP.NET Core Runtime 10.0.9
Stiahnuť a nainštalovať z [https://dotnet.microsoft.com/en-us/download/dotnet/10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
## Spustenie
Aplikácia je publikovaná na GitHub, na adrese: 
[https://github.com/peterbocej/WpfChat](https://github.com/peterbocej/WpfChat)

1. prejsť do vopred pripraveného adresára
2. Stiahnuť
``` bash
> git clone https://github.com/peterbocej/WpfChat.git
```
3. Prevziať potrebné knižnice
``` bash
> cd WpfChat
> dotnet restore
```
4. Upraviť a uložiť nastavenia databázy a Chat-u
``` bash
> notepad appsettings.json
```
5. Zostaviť
``` bash
dotnet build
```
6. Spustiť server
``` bash
> dotnet run --project .\WpfChat.WebApi\WpfChat.WebApi.csproj
```
7. Spustiť jedného alebo viacerých klientov
``` bash
> dotnet run --project .\WpfChat\WpfChat.csproj
```
## Ovládanie klienta
1. Nastaviť meno
2. Pripojiť
3. Chatovať