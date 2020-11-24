
select * from Client

select * from MeasuringUnit

select * from DeliveryNoteItem

/*delete from DeliveryNoteItem
dbcc checkident(DeliveryNoteItem, reseed, 0)


delete from InventoryDeliveries
dbcc checkident(InventoryDeliveries, reseed, 0)

delete from InventoryDeliveriesPackages
dbcc checkident(InventoryDeliveriesPackages, reseed, 0)

delete from DeliveryNote
dbcc checkident(DeliveryNote, reseed, 0)

delete from InventoryDeliveriesLocation
dbcc checkident(InventoryDeliveriesLocation, reseed, 0)

delete from InventoryStock
dbcc checkident(InventoryStock, reseed, 0)

delete from Product
dbcc checkident(Product, reseed, 0)

delete from MeasuringUnit
dbcc checkident(MeasuringUnit, reseed, 0)
where MeasuringUnitID between 11 and 11

delete from MobileTransaction
dbcc checkident(MobileTransaction, reseed, 0)

delete from IssueDocument
dbcc checkident(IssueDocument, reseed, 0)*/

create table InventoryStock (
	InventoryStockID int not null identity(1,1) primary key,
	ProductID int not null,
	LocationID int not null,
	Quantity decimal(18,2) not null,
	Notes varchar(500) null,
	tsInsert datetime null,
	tsInsertUserID datetime null,
	tsUpdate datetime null,
	tsUpdateUserID datetime null,
	QuantityPcs int  null,

	constraint FK_ProductID foreign key(ProductID) references Product(ProductID),
	constraint FK_LocationID foreign key(LocationID) references Location(LocationID)
);

alter table InventoryDeliveries
add InventoryStockID int null,
	Quantity decimal(18,3) null,
	UnitOfMeasureID int null,

constraint FK_InventoryStock foreign key (InventoryStockID) references InventoryStock(InventoryStockID.
constraint FK_InventoryDeliveriesUnitOfMeasure foreign key(UnitOfMeasureID) references MeasuringUnit(MeasuringUnitID));


create table InventoryDeliveriesLocation(
	InventoryDeliveriesLocationID int not null identity(1,1) primary key,
	InventoryDeliveriesID int not null,
	LocationFromID int not null,
	Notes varchar(300) null,
	tsInsert datetime null,
	tsInsertUserID datetime null,
	tsUpdate datetime null,
	tsUpdateUserID datetime null,
	IsMobileTransaction bit null,
	ParentID int null,
	NeedsMatching bit null,

	constraint FK_InventoryDeliveriesID foreign key(InventoryDeliveriesID) references InventoryDeliveries(InventoryDeliveriesID),
	constraint FK_InventoryDeliveriesLocationLocationID foreign key(LocationID) references Location(LocationID)

)


select * from InventoryDeliveriesLocation where InventoryDeliveriesLocationID = 3882

select * from InventoryStock

select * from Product

select * from Location

select * from Client

select * from ClientType

select * from Categorie

alter table InventoryDeliveriesLocation
add LocationToID int null,
EmployeeID int not null,
constraint FK_LocationToID foreign key(LocationToID) references Location(LocationID),
constraint FK_EmployeeID foreign key(EmployeeID) references Employee(EmployeeID);

alter table InventoryDeliveries
add LastLocationID int null,
constraint FK_LastLocationID foreign key(LastLocationID) references Location(LocationID);

create table MobileTransaction(
	MobileTransactionID int not null identity(1,1) primary key,
	InventoryDeliveriesLocationID int not null,
	ScannedProductCode varchar(500) not null,
	Notes varchar(300) null,
	tsInsert datetime null,
	tsInsertUserID int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
	UIDCode varchar(250) null,
	SupplierID int null,
	ProductID int null,
	Quantity decimal(18,3) null,
	UnitOfMeasureID int null,

	constraint FK_InventoryDeliveriesLocationID foreign key(InventoryDeliveriesLocationID) references InventoryDeliveriesLocation(InventoryDeliveriesLocationID),
	constraint FK_MobileSupplierID foreign key(SupplierID) references Client(ClientID),
	constraint FK_MobileProductID foreign key(ProductID) references Product(ProductID),
	constraint FK_MobileUnitOfMeasure foreign key(UnitOfMeasureID) references MeasuringUnit(MeasuringUnitID)
);

select * from InventoryDeliveriesLocation
select * from InventoryDeliveries where DeliveryNoteItemID = 9

select * from InventoryDeliveriesPackages where InventoryDeliveriesPackagesID=61


create table IssueDocument(
	IssueDocumentID int not null identity(1,1) primary key,
	IssueNumber varchar(20) not null,
	BuyerID int not null,
	IssueDate datetime not null,
	Name varchar(300) null,
	Notes varchar(2000) null,
	InternalDocument varchar(50) null, /*Interna številka (dobavnica)*/
	InvoiceNumber varchar(50) not null,
	tsInsert datetime null,
	tsInsertUserID int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
	IssueStatus int null,

	constraint FK_BuyerID foreign key(BuyerID) references Client(ClientID),
	constraint FK_IssueStatus foreign key(IssueStatus) references IssueDocumentStatus(IssueDocumentStatusID)
);

create table IssueDocumentPosition(
	IssueDocumentPositionID int not null identity(1,1) primary key,
	IssueDocumentID int not null,
	SupplierID int not null,
	Quantity decimal(18,3) not null,
	UID250 varchar(250) not null,
	Name varchar(300) not null,
	Notes varchar(1000) null,
	tsInsert datetime null,
	tsInsertUserID int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
	ProductID int null,

	constraint FK_IssueDocumentID foreign key(IssueDocumentID) references IssueDocument(IssueDocumentID),
	constraint FK_SupplierID foreign key(SupplierID) references Client(ClientID),
	constraint FK_IssueDocumentPosition_ProductID foreign key(ProductID) references Product(ProductID),
);

create table Settings(
	SettingsID int not null identity(1,1) primary key,
	IssueDocumentNumber int null,
);

create table IssueDocumentStatus(
	IssueDocumentStatusID int not null identity(1,1) primary key,
	Code varchar(30) not null,
	Name varchar(150) not null,
	Notes varchar(500) null,
	tsInsert datetime null,
	tsInsertUserID int null,
	tsUpdate datetime null,
	tsUpdateUserID int null,
);

select * from Location

select * from DeliveryNote

select * from InventoryDeliveries

select * from IssueDocumentPosition

select * from InventoryDeliveriesLocation order by 1 desc

select * from MobileTransaction

select idl.*, mt.UIDCode from InventoryDeliveriesLocation as idl join MobileTransaction as mt on idl.InventoryDeliveriesLocationID = mt.InventoryDeliveriesLocationID
where idl.IsMobileTransaction = 1 and InventoryDeliveriesID is null

select * from Product

select * from Users

select * from Employee

select * from InventoryDeliveries order by 1 desc

/*delete from MobileTransaction
dbcc checkident(MobileTransaction, reseed, 0);

delete from InventoryDeliveriesLocation
where InventoryDeliveriesID is null*/
/*4503191216000562*/
select * from InventoryDeliveriesLocation order by 1 desc
select * from InventoryDeliveries where AtomeUID250 like '%0102200721000001%'

/*delete from InventoryDeliveriesLocation
where InventoryDeliveriesLocationID between 3883 and 3894*/

select * from InventoryDeliveries where InventoryDeliveriesID = 269

 
/*delete from Users
where UserID between 31 and 63*/

select * from Location

delete from Location where LocationID = 18

select * from MeasuringUnit

select * from Users

create table DeliveryNoteStatus(
	DeliveryNoteStatusID int not null identity(1,1) primary key,
	Code varchar(30) not null,
	Name varchar(150) not null,
	Notes varchar(500) null,
	tsInsert datetime null,
	tsInsertUserID int null
);