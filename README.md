# Service.Info
Service.Info is a .NET Standard 2.1 library for receiving information about services/daemons on Windows and Linux. Mac support will maybe follow

## Prerequisites
### General
Starting/Stopping/Restarting services/daemons requires administrative rights of the program which is calling the method otherwise it will fail and no service/daemon will execute the command. So make sure that your program has this right if you want to use this functionality.
### Windows
WMI is used under Windows, so there should be no need to install additional stuff.
### Linux
Following programs must be installed:
* service
* systemctl

