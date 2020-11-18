#! /bin/bash

cd ~/BootstrapAdmin
git pull
dotnet publish src/client/Bootstrap.Client -c Release

rm -f ~/BootstrapAdmin/src/client/Bootstrap.Client/bin/Release/net5.0/publish/appsettings*.json
systemctl stop ba.client
\cp -fr ~/BootstrapAdmin/src/client/Bootstrap.Client/bin/Release/net5.0/publish/* /usr/local/ba/client/
systemctl start ba.client
systemctl status ba.client -l
