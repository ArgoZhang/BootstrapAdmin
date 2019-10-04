#! /bin/bash

mkdir /usr/local/ba
mkdir /usr/local/ba/admin
mkdir /usr/local/ba/client

cd ~/
git clone https://gitee.com/LongbowEnterprise/BootstrapAdmin.git

cp ~/BootstrapAdmin/src/admin/Bootstrap.Admin/appsettings.json /usr/local/admin
cp ~/BootstrapAdmin/src/admin/Bootstrap.Admin/BootstrapAdmin.db /usr/local/admin
cp ~/BootstrapAdmin/src/client/Bootstrap.Client/appsettings.json /usr/local/client

cp ~/BootstrapAdmin/services/* /usr/lib/systemd/system
systemctl enable ba.admin
systemctl enable ba.client
systemctl enable nginx
