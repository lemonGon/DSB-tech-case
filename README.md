# DSB tech case

Service sends push notification to our customers about disruptions, special offers and customer-tailored travel information

<br />

## Endpoints description

**Get all tokens**
----
Fetches all customers' tokens in the customers' table.
NOTE it will return a **501 not implemented** exception since it is backed by only an harness

* **URL**

  /api/push/get

* **Method:**

  `GET`

*  **URL Params**

  None

* **Success Response:**

    * **Code:** 200 <br />
      **Content:** `{ "tokenOne", "tokenTwo", "tokenThree", //... }`

* **Error Response:**

    * **Code:** %01 NOT IMPLEMENTED <br />
      **Content:** `{
       "status": 501,
       "traceId": "00-82d876ce2e8d114dbde6218b0f8657af-f6f802209c999942-00"
      }`

<br />

**Get customer with his device tokens**
----
Fetches a customer by ID and all his device tokens 

* **URL**

  /api/push/get/:customerId

* **Method:**

  `GET`

*  **URL Params**

   **Required:**

   `customerId=[integer]`

* **Success Response:**

    * **Code:** 200 <br />
      **Content:** `{
       "id": 1,
       "deviceTokens": [
        {
          "token": "123456"
        },
        {
          "token": "654321"
        }
       ]
      }`

  OR

    * **Code:** 204 NO CONTENT <br />


* **Error Response:**

    * **Code:** 400 BAD REQUEST <br />
      **Content:** `{
      "title": "One or more validation errors occurred.",
      "errors": { "customerId": [ "The value is not valid." ]
      }`

<br />

**Post new customer**
----
Saves a new customer with his device token

* **URL**

  /api/push/post/:customerId

* **Method:**

  `POST`

*  **URL Params**

   **Required:**

   `customerId=[integer]`

* **Data Params**

  DeviceToken <br />
  `{ "token": "customerToken" }`

* **Success Response:**

    * **Code:** 201 CREATED <br />
      **Content:** `{
      "id": 999,
      "deviceTokens": [
        {
         "token": "customerToken"
        }
       ]
      }`


* **Error Response:**

    * **Code:** 400 BAD REQUEST <br />
      **Content:** `{
      "title": "One or more validation errors occurred.",
      "errors": { "Token": [ "The Token field is required." ] }
      }`
      
      or
      
      **Content:** `{
      "title": "One or more validation errors occurred.",
      "errors": { "customerId": [ "The value is not valid." ]
      }`

  OR

    * **Code:** 415 UNSUPPORTED MEDIA TYPE <br />
      **Content:** `"title": "Unsupported Media Type",`


<br />

## Project setup

#### Minimum requirements:
* Docker
  
or
  
* NET Core 5.0.x
* Cassandra DB

<br />
Following, my favourite setup:
* Cassandra DB running on a local Docker container;
* The main solution running against my local Kestrel web server for faster development / debug.

* it assumes you have GIT installed on your machine

<br />

##### - Cassandra DB running on a local Docker container
First install Docker (latest version) and then run the following commands to get Cassandra DB up and running for our solution:

`sudo docker network create dsb-cassandra-net`
<br />
`sudo docker run -p 9042:9042 --name dsb_cassandra --network dsb-cassandra-net -d cassandra:latest`
<br />
`sudo docker run -it --rm --network dsb-cassandra-net cassandra:latest cqlsh dsb_cassandra`
<br />
`CREATE KEYSPACE push_data WITH REPLICATION={'class': 'SimpleStrategy', 'replication_factor': 1};`
<br />
`CREATE TABLE push_data.customers (customer_id INT, device_token TEXT, PRIMARY KEY((customer_id), device_token));`
<br />
`INSERT INTO customers (customer_id, device_token) VALUES ( 1, 'myFirstDeviceToken');`
<br />
`exit`
<br />

[Click here for more info about this Docker image](https://hub.docker.com/_/cassandra)

NOTE: You may need to check the Cassandra container IP so that you will be able to replace it in your `appsettings.Development.json` file -> `ContactPoint` although just `localhost` should work.

<br />

##### - Main solution running against my local Kestrel web server
The project is build and runs on .NET core 5.0.x and a cassandra DataBase so the first steps are to install .NET Core 5.0.x on your machine.
You can also have it run against Docker but for faster development and easier debug I prefer having it run against my local Kestrel web server.

* Install .NET Core 5.0.x on our machine
* clone this project `git clone https://github.com/lemonGon/DSB-tech-case.git`
* check if any adjustments is needed in your `Setup.cs` and `appsettings.json` files, particularly in the Cassandra contact point.

<br />

##### - Truth time!
If everything has gone accordingly, when running the solution a swagger interface should pop up on your browser.

If it did not, try to reach this url in the browser manually: [https://localhost:5005/swagger/index.html](https://localhost:5005/swagger/index.html)

When calling the `GET /api/push/get/{customerId}` endpoint using 1 as a `customerId` parameter, you should get a `PushCustomer` model back.

<br />

You can use the post to submit a new customer. Enjoy DSB push notifications!
