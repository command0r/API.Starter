# .NET 6 Boilerplate WebAPI
## Intro
This solution features a rich set of packages that present the most essential combination required for the active development phase. This is a 'boilerplate' that can be used as a starting point for a project of any size with any Architectural style spanning from the Clean Architecture toward the Microservice Architecture. Domain-driven design and appropriate solution structuring offers a clear and concise way for the feature implementation.

## Structure
As per the 'Clean Architecture' concept, Host (WebAPI project) relies upon Infrastructure component, which, in turn, is dependent on the Core (Application --> Domain --> Shared modules).

## Features
The main feature are the following:
* Audit logging (security and compliance access auditing)
* Identity and permission management (IdentityService)
* Background and long-running Jobs (with Hangfire)
* Caching (EF + Redis)
* CORS policies mgmt.
* Localization
* Mailing services
* Multitenancy (Finbuckle)
* Domain events and notifications
* Persistence (MS SQL initially that can easily be adjusted for MySQL, Postgres or Oracle databases)
* Log management and exception handling infrastructure

## Coding approach
* The framework offers a strict coding approach that utilizes CQRS + MediatR patterns
* Specification pattern is offered for further optimization and more laconic code structure
* Serilog 'sincs' allow connecting a wide variety of different monitoring tools. For the local development the [SEQ Console](https://datalust.co/seq) is preferred
* StyleCop rulesets are used as a preventative measure that encourages proper code styling

## Local deployment
* Clone code from the repository
* Restore dependencies with ```dotnet restore``` (```dotnet restore --verbosity normal``` to observe detailed restore logs)
* In case of 'package not found' error run ```dotnet new nugetconfig```
* Ensure that ```database.json``` and ```hangfire.json``` in Host --> Configurations are updated with correct connection info <b>before</b> running the project
* Run the project ```dotnet watch run```
* Use pre-configured Postman collection to test APIs (the collection can be imported from the 'postman' directory) 

## Other dependencies (optional)
* [Docker](https://www.docker.com/products/docker-desktop/)
* [SEQ Console](https://datalust.co/seq) - For viewing structured logs during development
  * Pull the image ```docker pull datalust/seq```
  * For **development** (no authentication): ```docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -e SEQ_FIRSTRUN_NOAUTHENTICATION=true -p 5341:80 datalust/seq:latest```
  * For **production** (with authentication): ```docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -e SEQ_FIRSTRUN_ADMINPASSWORD=YourSecurePassword -p 5341:80 datalust/seq:latest```
  * Access SEQ console via the browser at ```http://localhost:5341/```
  * **Note**: Newer SEQ versions require either authentication setup or explicit opt-out. The development command above disables authentication for local development convenience.