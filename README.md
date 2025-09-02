# Microservicios con ASP.NET Core + Docker 

Repositorio  que muestra una **arquitectura de microservicios** con **ASP.NET Core** empaquetada en **Docker** y orientada a despliegue en **Azure**. Enfatiza desacoplo y  comunicación asíncrona. 

## Arquitectura 

* **Servicios independientes** (ejemplos): `Catalog`, `Basket`, `Ordering`, `Identity`.
* **Comunicación**: HTTP/REST para consultas 
* **Persistencia poliglota**: `SQL Server`, `PostgreSQL`, `MySQL` vía **EF Core** (Code‑First, migraciones).
* **Patrones**: **CQRS** en capa de aplicación.


## Infra & DevOps

* **Contenedores**: `Dockerfile` por servicio y `docker-compose` para entorno local.
* **Azure**: bases de datos administradas y pipelines de **Azure DevOps/GitHub Actions** para CI/CD.

## Estructura de carpetas

```text
/src/Services/<ServiceName>   API + Application + Domain del servicio
/src/BuildingBlocks           Reutilizables (contratos, utilidades, mensajes)
/deploy                       Docker Compose y manifiestos/plantillas de despliegue
/tests                        Unit/Integration por servicio
```


