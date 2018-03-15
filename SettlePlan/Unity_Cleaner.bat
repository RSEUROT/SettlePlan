set currentpath=%~dp0Library 
RD %currentpath% /Q/S
set currentpath=%~dp0.vs 
RD %currentpath% /Q/S
DEL /Q/S *.sln
DEL /Q/S *.csproj
