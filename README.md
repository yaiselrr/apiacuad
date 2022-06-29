Simas Torreon Backend
=====================

Backend en .NET Core para el proyecto Simas Torren

1. Instalar EF tools para migraciones

	```ssh
	dotnet tool install --global dotnet-ef
	```
	
2. Ejecutar las migraciones

	```ssh
	dotnet ef database update --context ApplicationDBContext
	```
	
