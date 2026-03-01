# GitHub Details Get Backend

## Overiew

This Project is an ASP.NET core web API that integrates with the GithUb public REST API to retrieve repository data.

## Features

1. Retrieve Repository Details using username and repository Name
2. Retrieve All Repository in the Database
3. Local Caching

## Technologies Used

1. ASP.NET Core Web API (.NET 8)
2. MS SQL Server
3. Docker

## Setup Instructions

### Normal-way

1. Clone The Github Repository
   </br>
   Execute : </br>
   `git clone https://github.com/harith2001/liquid_labs_harith.git`
   </br>
2. Create Database and Tables
   </br>
   Execute : (In the MS SQL Sever) </br>
   `DB_Schema.sql`
   </br>
3. Add the Database Connection to environment variables
   </br>
   `appsettings.json`
   </br>
   Add this nesscassary details : </br>
   ` "Server=localhost;Database=Harith_Local;User Id=sa;Password=<Password>;TrustServerCertificate=True;"`
   </br>
4. Run Application
   </br>
   Execute :
   `dotnet run`
   </br>
5. Open Browser
   </br>
   Open :
   `http://localhost:<port>/swagger`
   </br>

### Using Docker

1. Clone The Github Repository
   </br>
   Execute : </br>
   `git clone https://github.com/harith2001/liquid_labs_harith.git`
   </br>
2. Verify the docker
   </br>
   Execute :
   `docker --version`
   </br>
   - if using docker desktop: start the application to enable docker
3. build and start the application
   </br>
   Execute :
   `docker compose up --build`
   </br>
4. Open Browser
   </br>
   Open :
   `http://localhost:8080/swagger`
   </br>

## API Endpoints

1. Get Single Repository </br>
   `GET /api/v1/GitHubRepo/{owner}/{repoName}`
2. Get Single Repository </br>
   `GET /api/v1/GitHubRepo/All`

## Author

Harith Danula Vithanage</br>
Github: @harith2001
