#! /bin/bash

mkdir /usr/local/ba
mkdir /usr/local/ba/admin
mkdir /usr/local/ba/client

cp ~/BootstrapAdmin/src/admin/Bootstrap.Admin/appsettings.json /usr/local/admin
cp ~/BootstrapAdmin/src/admin/Bootstrap.Admin/BootstrapAdmin.db /usr/local/admin
cp ~/BootstrapAdmin/src/client/Bootstrap.Client/appsettings.json /usr/local/client

cp * /usr/lib/systemd/system/
systemctl enable ba.admin
systemctl enable ba.client
systemctl enable nginx
