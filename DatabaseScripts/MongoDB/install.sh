# init mongodb data
mongo ~/src/Longbow/BootstrapAdmin/DatabaseScripts/MongoDB/init.js
mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"