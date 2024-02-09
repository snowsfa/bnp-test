Exercise description

As a software developer you should create a service class/layer that allows you to retrieve and store the prices of a list of securities. Please note that:

- A security is a financial instrument identified by an ISIN, an alphanumeric code of 12 characters.

- The service should be written according to SOLID principles with the usage of Dependency Injection.

- The service should have a method which receives as input a list of ISIN.

- The service has to retrieve and store the price for each ISIN in a SQL server database.

- The price of an ISIN must be retrieved through an external web API: https://securities.dataprovider.com/securityprice/{isin}

- The service should be unit tested
