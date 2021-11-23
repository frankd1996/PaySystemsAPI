# PaySystemsAPI

PaySystemsApi is a monolithic ASP.NET Core Web Api, currently in production, This application serves as the backend of the Xamarin Forms PaySystemsMobile application: https://github.com/frankd1996/PaySystemsMobile
In this backend project you can find the following implementations:

-Clean architecture: this architecture is achieved by creating the web project and several class libraries in the same solution. A clean architecture allows a strong decoupling between classes and responsibilities, allowing solution changes (such as changing database provider) with very little effort and time.
Disadvantage: a clean architecture requires more development time
Advantage: Major changes to be made in the future require little effort due to strong decoupling


![Clean Arquitecture](https://i.postimg.cc/nhdftryq/2021-11-22-20h17-27.png)


-Dependency injection: design pattern that is responsible for extracting the responsibility for creating instances of a component to delegate it to another. Especially useful for using the class that maps the database through the Entity Framework.

-Entity Framework: it is a powerful ORM that allows us to map the database and its tables without the obligation to use the SQL language. It also allows working in database first and code first mode.

-LinQ: is a uniform query syntax in C # and VB.NET used to save and retrieve data from different sources. Especially useful in Entity Framework to query and map data from tables in databases and also works with any collection, not necessarily reacted with a database

-Authentication and authorization: we handle this security issue through the Microsoft.AspNetCore.Identity nuget package, and it allows the implementation of authentication and authorizations under the JWT standard quickly.

-Persistence: for data persistence we use SQL Server
