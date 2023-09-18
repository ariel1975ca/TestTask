# TestTask
A simple test system in which the user enters their name, chooses a test, executes  it, and at the end sees their result.

DB configuration:

 - use the file "Data And Index Configuration" to add data to your created container and to configure the "Indexing Policies".
 - you can also use the "TestTaskDbDump" to restore the tests data and some example or test compilation data as well.
 
 
Client access configuration:

Set the connections details in "appsettings.json" file you will found in "TestTaskWebApi" project:
  - In the "TestTaskCosmosDb" section:
    - Specify the Cosmos DB Uri in the "Account" field
	- Specify the Cosmos DB primary key in the "Key" field
	- In case you use different database and container names, specify them in the "DatabaseName" and "ContainerName" fields respectively.

	
 
