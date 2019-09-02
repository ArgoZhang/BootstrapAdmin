load("./Dicts.js");
load("./Navigations.js");
load("./Groups.js");
load("./Roles.js");
load("./Users.js");

conn = new Mongo("localhost:27017");
db = conn.getDB("BootstrapAdmin");

// Dicts
db.Dicts.drop();
db.createCollection('Dicts');
db.getCollection("Dicts").insert(Dicts);

// Navigations
db.Navigations.drop();
db.createCollection('Navigations');
db.getCollection("Navigations").insert(Navigations);

// Groups
db.Groups.drop();
db.createCollection('Groups');
db.getCollection("Groups").insert(Groups);

// Roles
db.Roles.drop();
db.createCollection('Roles');
db.getCollection("Roles").insert(Roles);

// Users
db.Users.drop();
db.createCollection('Users');
db.getCollection("Users").insert(Users);