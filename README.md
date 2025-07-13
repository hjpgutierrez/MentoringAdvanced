# .NET MentoringAdvanced

## Overview

This project is a program designed for .NET specialists who want to fill their technical gaps or systematize knowledge in architecture and advanced technological topics. This course also covers topics regarding Code Quality, SDLC, and CI/CD which are crucial to becoming Lead Software Developers. we will implement RabbitMQ, MongoDB, and MS SQL Server as its core services. These services are containerized using Docker and can be easily set up and run using Docker Compose.

---

## Prerequisites

Before running the services, ensure you have the following installed on your machine:

-   [Docker](https://www.docker.com/get-started)
-   [Docker Compose](https://docs.docker.com/compose/install/)

---

## Running the Services

To run the required services (RabbitMQ, MongoDB, and MS SQL Server), follow these steps:

1.  Open a terminal.
2.  Navigate to the `DevInstall` folder where the `docker-compose.yml` file is located:

    bash

    ```
    cd DevInstall
    ```

3.  Run the following command to start the services:

    bash

    ```
    docker-compose up -d
    ```

    This command will start all the services in detached mode.

4.  Verify that the services are running:

    bash

    ```
    docker ps
    ```

5.  Once .NET API (Carting and Catalog) are running, it will execute the database migrations, so you can use swagger to interact with the presentation layers.

---

## Accessing the Services

Once the services are running, you can access them as follows:

-   **RabbitMQ Management UI**:

    -   URL: [http://localhost:15672](http://localhost:15672/)
    -   Default Credentials:
        -   Username: `guest`
        -   Password: `guest`

-   **MongoDB**:

    -   Host: `localhost`
    -   Port: `27017`
    -   Authentication: (If authentication is disabled, no credentials are required. Otherwise, use the credentials specified in the `docker-compose.yml` file.)
    -   ConnectionString: `mongodb://{user}:{password}@localhost:27017`

-   **MS SQL Server**:

    -   Host: `localhost`
    -   Port: `1433`
    -   Default Credentials:
        -   Username: `sa`
        -   Password: `{YourPasswordHere}`
    -   ConnectionString:
        -   Default: `Server=localhost,1433;Database=Catalog;User Id=sa;Password={YourPasswordHere};TrustServerCertificate=true`
        -   Test: `Server=localhost,1433;Database=CatalogIntegrationTest;User Id=sa;Password={YourPasswordHere};TrustServerCertificate=true`

-   **Auth0**:
    -   Contact the developer team to create Manager and Customer users in [Auth0 Portal](https://dev-5tqrr085ocpf71db.us.auth0.com/)

---

## Stopping the Services

To stop the services, run the following command from the `DevInstall` folder:

bash

```
docker-compose down
```

---

## SonarQube

1. To start SonarQube, execute the following command to run it as a Docker container:

```
 docker run -d --name sonarqube -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true -p 9000:9000 sonarqube:latest
```

2. SonarScanner is required to analyze your project. Install it globally using the following command:

```
dotnet tool install --global dotnet-sonarscanner
```

3. Once the project is created in the [Sonar UI](http://localhost:9000), it will generate a token, copy the project name and token:

```
 dotnet sonarscanner begin /k:"MentoringAdvanced" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_0c9ded2b962a66d937d197fcd6af6ea52bb6e5c7"
```

4. SonarScanner analyzes your project binaries, so ensure your code is compiled before proceeding. Use the following command to build your project:

```
 dotnet build
```

5.  After building your project, finalize the scan to generate the analysis report:

```
dotnet sonarscanner end /d:sonar.token="sqp_0c9ded2b962a66d937d197fcd6af6ea52bb6e5c7"
```

6. Go back to the [Sonar UI](http://localhost:9000) and navigate to your project dashboard to view the analysis results

---

## Troubleshooting

-   **If you encounter issues with MongoDB authentication**:

    -   Ensure that the `docker-compose.yml` file is configured to either disable authentication (`mongod --noauth`) or use the correct credentials in your application.

-   **If ports are already in use**:

    -   Make sure no other services are running on the same ports (e.g., `5672`, `15672`, `27017`, `1433`).
    -   You can update the `docker-compose.yml` file to use different host ports if needed.

-   **If you encounter issues, check the SonarQube logs using the following command**:

    -   `docker logs sonarqube`
