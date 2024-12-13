## Microservice 

### Preparation before build docker
Before building docker compose, you should follow below link to setup dotnet certificate for HTTPS

https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-9.0


### Docker
In the ```src``` folder, you open cmd and type in
````
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build
````
