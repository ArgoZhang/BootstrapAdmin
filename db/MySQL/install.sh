#! /bin/bash

mysql  -e "drop database if exists BootstrapAdmin; create database BootstrapAdmin;" -uroot
mysql -hlocalhost -uroot -DBootstrapAdmin < ~/src/Longbow/BootstrapAdmin/DatabaseScripts/MySQL/install.sql
mysql -hlocalhost -uroot -DBootstrapAdmin < ~/src/Longbow/BootstrapAdmin/DatabaseScripts/MySQL/initData.sql
