create table Products(
ProductID int identity(1,1) primary key,
Name varchar(100) not null,
Price decimal(18,2) not null
)
--drop table Sales
create table Sales(
SaleID int identity(1,1) primary key,
Date datetime not null,
Total decimal(18,2) not null
)
--drop table SalesDetails
create table SalesDetails(
SaleDetailsID int identity(1,1) primary key,
SaleID int not null,
ProductID int not null,
Quantity int not null,
Price decimal(18,2) not null
FOREIGN KEY(SaleID) REFERENCES Sales(SaleID),
FOREIGN KEY(ProductID) REFERENCES Products(ProductID)
)
