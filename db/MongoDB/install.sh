#! /bin/bash
# init mongodb data
mongo ./init.js
mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"
