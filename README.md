Run tha app and test it from postman 

Name Regex -> { "type": "string", "minLength": 3, "maxLength": 12, "pattern": "^[a-zA-Z]{3,12}$" }
Id Regex -> { "type": "string", "maxLength": 10, "pattern": "^[0-9a-zA-Z]{1,10}$" }

// Postman Examples:
Base URL: http://localhost:49847/api/clients

PUT /api/clients/sdf1 HTTP/1.1
Host: localhost:49847
Content-Type: application/json
Cache-Control: no-cache
Postman-Token: 4893bbe0-a944-7862-fb1d-dd23c747b655

{
  "jsonapi": {
    "version": "1.0"
  },
  "data": {
    "type": "clients",
    "id": "sfdl",
    "attributes": {
      "name": "usera"
    }
  }
}

POST /api/clients HTTP/1.1
Host: localhost:49847
Content-Type: application/json
Cache-Control: no-cache
Postman-Token: 8c80d2b1-d47e-5446-ebf2-8c97f5d5976e

{
  "jsonapi": {
    "version": "1.0"
  },
  "data": {
    "type": "clients",
    "attributes": {
      "client_id": "1234567DR",
      "name": "user"
    }
  }
}

GET /api/clients HTTP/1.1
Host: localhost:49847
Content-Type: application/json
Cache-Control: no-cache
Postman-Token: df5d36ad-bd14-fcbe-71da-92a0c72a571a

