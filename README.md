# AGSR-TestTask
To test in docker after clonning repo you need to make this actions
1) in src folder open terminal and run docker-compose up (this will create sql, console and web api)
2) after this you need to find name for console app open another terminal (run docker ps, on my local is src_patientconsole_1) and run commands to enter it you need to run: 1) docker exec -it {consoleapp container name} sh 2) dotnet Patient.Console.dll
3) now you can click 1 in console window to add 100 recipients with random names, surnames, genders, active status and birth date.
4) to test postman you need to import file from root folder with name - PatientApiCollection.postman_collection.json (keep in mind to set Docker environment to make host variable points to docker port instead of local port)
