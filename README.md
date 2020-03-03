# SDGs
A suite of application to manage the [SDGs - Sustainable Development Goals](http://sustainabledevelopment.un.org).
The repository contains a ASP.NET web application to manage the SDGs indicators into a SQL Server database, and a c# application to move the SDGs indicators, from a SDGs database into a PxWeb database. 

## SDGs WA 

The SDGs Web Application `SDGs WA` is a ASP.NET web application and provides the following functionalities:


1.	Management of SDGs indicators and subindicators (data and metadata);
2.	Import/export of SDGs indicators and subindicators (data and metadata);
3.	User management and profiling;

## What you’ll need
In order to build the `SDGs WA` application, your environment should fulfill the following requirements:

* Visual Studio 2017 or later
* .NET Framework 4.0 or later
* SQL Server 2014 or later

## How to build
Download and unzip the source code in your workspace or import the project in Visual Studio.
Before building the application, you must create and populate a SDGs database. You can use the following sql scripts:
```
cd SDGs\SDGs_WA\SQL_script;
> Sqlcmd -S .\{instanceSQLSERVER} -i create_SDGs_WA.sql
> Sqlcmd -S .\{instanceSQLSERVER} -i populate_SDGs_WA.sql
```
The scripts create and polpulate the SDGs_WA database with arabic metadata SDGs examples.

The script will populate the `USERS/ROLES` tables with the `SDGs WA` users:
```
Administrator user:
Username: user1
Password: user1

Generic user:
Username: user2
Password: user2
``` 

After DB installation, you must configure the database connection string in the file `Web.config` of SDGS_WA project: 
```
 ...
 <add name="conString" connectionString="Server={instanceSQLSERVER};Database=SDGs_WA;Trusted_Connection=True;" />
 ...
```

Now you can build and run the application under Visual Studio.


## MDT2PxWeb

`MDT2PxWeb` is a c# application to migrate the SDGS indicators from a multidimensional table to [The Nordic Data Model (CNMM) 2.3](https://www.scb.se/en/services/statistical-programs-for-px-files/px-web/px-web-med-sql-databas/) database, 
used by the [PxWeb](https://www.scb.se/en/services/statistical-programs-for-px-files/px-web/), a web application developed by Statistics Sweden together with other National Statistical Institutions (NSI) to disseminate public statistics in a structured way.


## What you’ll need
In order to build the `MDT2PxWeb` application, your environment should fulfill the following requirements:

* Visual Studio 2017 or later
* .NET Framework 4.0 or later
* SQL Server 2014 or later
* SDGs Database
 
## How to build
Download and unzip the source code in your workspace or import the project in Visual Studio.
Before building the application, you must edit the configuration file `config.json`

## Usage
After building, you can also use the application by commad line, to create or update the database:
```
> MDT2PxWeb config.json [print [populate_PxWeb_subindicatorData.sql]] - insert SDGs data into the PxWeb database
 
> MDT2PxWeb config.json update [populate_PxWeb_subindicatorData.sql]  - update SDGs data into the PxWeb database
```
The tool creates the sql file `populate_PxWeb_subindicatorData.sql` to populate a PxWeb database.

To create and populate the PxWeb database, according to the Nordic Data Model (CNMM) 2.3, you can use the following sql script file, located "SDGs\MDT2PxWeb\SQL script - PxWeb" folder:  

```
create_DB_PxWeb.sql
create_SDGs_Subindicator_PxWeb.sql
populate_PxWeb_generics.sql
populate_PxWeb_subindicatorData.sql
 
``` 
The files above manage also a secondary language in PxWeb. You can found the arabic as primary language and the english as secondary.  

## License
`SDGs WA` and `MDT2PxWeb` are EUPL-licensed
