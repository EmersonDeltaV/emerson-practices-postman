# Introduction

This project explains a better way to utilize Postman in developing and testing API Projects. 

# Content

## Source Codes
* `src/Emerson.Practices.Postman.PeopleDirectoryApp` - Developed using ASP.Net 8.0, the source code of the sample Rest API that the postman collection is created for.

## Postman Workspaces
* `postman\Ideal` - The ideal way of using Postman, utilizing environment, random variables, pre-request scripts, post-response scripts.
* `postman\BadExample` - An unoptimized example of using Postman, No environment, variables, pre-scripts or post-scripts is used, all plain-old-collection of hardcoded requests.

# How To Run

1. Clone the repo.
2. Open the `src/Emerson.Practices.Postman.PeopleDirectoryApp/Emerson.Practices.Postman.PeopleDirectoryApp.sln` solution.
3. Run the `Emerson.Practices.Postman.PeopleDirectoryApp` project.
4. Wait until the swagger is loaded.
5. Open your Postman and Create a new Workspace `Postman Ideal`
6. Load the newly created Workspace `Postman Ideal`.
7. Import all the collection and environments from this folder `postman\Ideal`.
8. Navigate to the `Collections\Main`.
9. Select the environment `Local`.
10. Run the requests 1-by-1 from `Authorization` to `Cleanup` folder.
11. You may also want to create a new Workspace and import the collections from the `postman\BadExample` folder.

# Notable Postman Features

1. **Environments**, each environments contains a specific variable dedicated to the defined environment, such as:
   1. baseUrl
   2. username
   3. password
2. **Login**
   1. The request Body contains the variable `{{username}}` and `{{password}}`.
   2. The Post-Response script contains 2 notable content.
      1.  The test itself confirming that the response should return 200 status code.
      2.  The setter of environment `token` based on the response of the request.
3.  **All Requests**, all requests other than Login uses the `{{token}}` variable as thier `Authorization`.
4. **Add Person**
   1. The Pre-Request script generate new variable called `newDateOfBirth` to generate a date from 5~30 years ago.
   2. The request Body uses the random generator variables `{{$randomFirstName}}`, `{{$randomLastName}}`, and `{{$randomExampleEmail}}` to generate a more realistic payload based on the needed property.
   3. The Post-Request script stores the variable `{{personId}}` for succeeding API requests.