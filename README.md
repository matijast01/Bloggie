# Bloggie
Project requires a few adjustments to run:
1. appsettings.json file hasn't been added to git. this file is need to run and requires a connection string for regular database and auth database, 
with the names as "BloggieDbConnectionString": and "BloggieAuthDbConnectionString":
this also requires a cloudinary api key with the name "Cloudinary":
you can change these names however you will need to change their names wherever they are called aswell.
2. the AuthDbContext file has had its seeded username, email, id  and password for the superadmin account, aswell as all role id's removed, you will need to put your own ones in there for it to work.
3. Migrations folder was not pushed, this will be auto generated whenever you run entity framework migrations to create your own databases.
