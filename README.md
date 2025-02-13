#Name Brian Nyamwe
# ITCtodoapp
InternationalTradeCentretodo app assignment

# 1) Setup Instructions for Running the Application Locally
Prerequisites
Ensure you have the following installed:

		.NET 8 SDK (Ensure it's installed: dotnet --version)
		Node.js & npm (for React frontend)
		MongoDB Atlas (or a locally running MongoDB instance)
		Visual Studio 2022 (Recommended for API development)
		VS Code or any IDE (for React frontend)
		Backend Setup (ASP.NET Core + MongoDB)
		Clone the repository from GitHub


git clone https://github.com/brianomusee/ITCAssignment.git
cd TaskManagerAPI
Configure MongoDB Connection


Restore Dependencies & Build the API

dotnet restore
dotnet build
Run the API


dotnet run
The API should now be running on https://localhost:7032
Access Swagger UI: https://localhost:7032/swagger
Frontend Setup (React + PrimeReact)
Navigate to the frontend folder


cd ../TaskManagerFrontend
Install Dependencies


npm install
Start the React App


npm start
The frontend should now be accessible at http://localhost:3000



# 2) Brief Documentation of API Endpoints
Method	Endpoint	Description
GET	/api/task	Retrieve all tasks
GET	/api/task/{id}	Get a task by ID
POST	/api/task	Create a new task
PUT	/api/task/{id}	Update an existing task
DELETE	/api/task/{id}	Soft delete a task
Request Body for Creating a Task (POST /api/task)

				json  Example

				{
				  "title": "Complete project",
				  "description": "Finish the API and frontend integration",
				  "status": "TODO",
				  "priority": "HIGH",
				  "dueDate": "2025-02-20T00:00:00Z"
				}
				Response Example

				json

				{
				  "title": "Complete project",
				  "description": "Finish the API and frontend integration",
				  "status": "TODO",
				  "priority": "HIGH",
				  "dueDate": "2025-02-20T00:00:00Z",
				  "createdAt": "2025-02-13T12:00:00Z",
				  "updatedAt": "2025-02-13T12:00:00Z"
				}
# 3) Summary of Approach, Assumptions, and Limitations
# Approach
		Clean Architecture:

		Repository Pattern for data access.
		DTOs for data transfer and validation.
		AutoMapper for model-to-DTO mapping.
		Logging integrated using ILogger<>.
		Security & Best Practices:

		Soft deletion for data integrity.
		CORS configured to allow http://localhost:3000.
		Swagger UI enabled for API testing.
# Assumptions
		MongoDB Atlas is used, but it can be replaced with a local MongoDB instance.
		Frontend runs on http://localhost:3000 and interacts with the API.
		Limitations
		No authentication/authorization implemented.
		Soft deletion means deleted tasks still exist in the database.
		Limited validation for fields beyond basic constraints.
