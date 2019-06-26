# VitalisTest
Sample example of implementaion of protected API with Identity Server 4.
Solution contains 4 projects:

1. Authentication Server - does all authentication and athorization work
2. Book Store api - allows to do CRUD operations with book entities
3. Book Store MVC client - MVC web app that allows to do CRUD operations with book entities through Book store API
4. Book Store CL Tool - cl tool that allows to do CRUD operations with book entities through Book store API

#How to set up:

1. Set DefaultConnection string to your local DB through appsettings.json file in Authentication Server and Book Store projects. Then update the databases for all projects' migrations
2. Authentication Server and Book Store app urls are configured in launchSettings.json.
3. Make sure that "AuthenticationServerEndpoint" and "BookStoreApiEndpoint" properties of appsettings.json of Book Store, Book Store MVC client and Book Store CL Tool point to the correct endpoints
4. Launch Authentication Server and Book Store to work with api through MVC client or cl tool. 
5. Create a user on Authentication server to address Api through MVC client

