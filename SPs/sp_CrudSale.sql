go
create type saleDetailsType as table(
	ProductID int,
	Quantity int,
	Price decimal(18,2)
)

go
create procedure sp_InsertSale(
@Date datetime,
@Total decimal(18,2),
@SaleDetail saleDetailsType readonly
)
as

begin
	insert into Sales(Date,Total) values(@Date,@Total)

	declare @SaleID int
	set @SaleID=SCOPE_IDENTITY()

	insert into dbo.SalesDetails(SaleID,ProductID,Quantity,Price)
	select @SaleID,ProductID,Quantity,Price from @SaleDetail;

	select @SaleID

end
go

create procedure sp_GetProductPrice(
	@ProductID int
)
as
begin
	select Price from Products where ProductID=@ProductID
end

go
create procedure sp_DeleteSale(
	@SaleID int
)
as
begin
	delete from SalesDetails where SaleID=@SaleID
	delete from Sales where SaleID=@SaleID
end

go

create procedure sp_GetAllSales

as
begin 
	select SaleID,[Date] 'Date',Total from Sales
	select SaleDetailsID,SaleID,productID,Quantity,Price from SalesDetails

end

go

create procedure sp_GetSaleByID(
@SaleID int
)

as

begin
	select SaleID,[Date] 'Date',Total  from Sales where SaleID=@SaleID
	select SaleDetailsID,SaleID,ProductID,Quantity,Price from SalesDetails where SaleID=@SaleID
end

go

create procedure sp_DeleteSaleDetails
(
 @SaleID int
 )
 as
 begin
	delete from SalesDetails where SaleID=@SaleID
 end

 go

create procedure sp_InsertSaleDetail
(
	@SaleID int,
	@ProductID int,
	@Quantity int,
	@Price decimal(18,2)
)

as
begin
	insert into SalesDetails(SaleID,ProductID,Quantity,Price)
		Values(@SaleID,@ProductID,@Quantity,@Price)
	
end

go

create procedure sp_UpdateSale
(
	@SaleID int,
	@Date datetime,
	@Total decimal(18,2)
)
as
begin
	update Sales set [Date]=@Date,Total=@Total where SaleID=@SaleID
end

go


