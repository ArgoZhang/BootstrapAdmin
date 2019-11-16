# init mongodb data
@echo off

mongo ./init.js
mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"
