
This project is a simple ASP.NET Core Web API created for a university assignment.
The goal of this project was to build a backend system for managing rooms and reservations in a training center. 
The application does not use a database, and all data is stored in
static in-memory lists that are initialized when the application starts.
In this project I implemented two controllers, which are RoomsController and ReservationsController. The RoomsController is responsible for 
managing rooms, and the ReservationsController is responsible for managing reservations.
The application allows users to get all rooms, get a single room by its id, and get rooms by building code using a route parameter. 
It is also possible to filter rooms using query parameters, for example by minimum capacity or projector availability.
The user can add a new room, update existing room data, and delete a room.
For reservations, the application allows getting all reservations, getting a reservation by id, and filtering reservations 
using query parameters such as date, status, and roomId. It is also possible to create a new reservation, update
an existing one, and delete a reservation.
In this project I used different ways of passing data. Route parameters are used for endpoints like getting a room by id or 
getting rooms by building code. Query parameters are used for filtering data. JSON request body is used in POST and PUT requests.
I also implemented basic validation and business rules. Some fields are required, such as name, buildingCode, organizerName, and topic. 
Capacity must be greater than zero. EndTime must be later than StartTime. A reservation cannot be created if the room does not exist or if the room is inactive. 
The system also checks for overlapping reservations, and if two reservations overlap in time for the same room, the API returns a 409 Conflict status.
The API returns proper HTTP status codes. It returns 200 OK for successful requests, 201 Created when a new resource is created, 204 No Content when 
something is deleted, 404 Not Found when the resource does not exist, and 409 Conflict
when there is a conflict such as overlapping reservations.
I tested the application using Postman. I checked different types of requests such as GET, POST, PUT, and DELETE. I also
tested filtering, valid and invalid input
data, and error cases like 404 Not Found and 409 Conflict.
While working on this project, I learned how to build a REST API using ASP.NET Core, how routing works,
and how to pass data using route parameters, query strings, and request body. I also understood better how HTTP status codes
should be used in practice and how to validate input data and handle simple business logic.
The project was created using C#, ASP.NET Core Web API, Rider, GitHub, and Postman.