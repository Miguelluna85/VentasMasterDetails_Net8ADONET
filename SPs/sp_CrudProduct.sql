create procedure sp_InsertProduct(
@Name varchar(100),
@Price decimal(18,2))
as
begin
	insert into dbo.Products([Name],Price)values(@Name,@Price)

	--select SCOPE_IDENTITY() as id
end

--
--
go
create procedure sp_DeleteProduct(
@ProductID int
)

as
begin
	delete from Products where ProductID=@ProductID
	--select SCOPE_IDENTITY() as id
end

--
--
go
create procedure sp_GetAllProducts
as
begin
	select ProductID,Name,Price from Products

	--select SCOPE_IDENTITY() as id
end
--
--
go
create procedure sp_GetProductByID(
@ProductID int 
)
as
begin
	select ProductID,Name,Price from Products where ProductID=@ProductID
	--select SCOPE_IDENTITY() as id
end

--
--
go
create procedure sp_UpdateProduct(
@ProductID int,
@Name varchar(100),
@Price decimal(18,2)
)
as
begin
	update  dbo.Products set [Name]=@Name,Price=@Price where ProductID=@ProductID
	--select SCOPE_IDENTITY() as id
end
go
