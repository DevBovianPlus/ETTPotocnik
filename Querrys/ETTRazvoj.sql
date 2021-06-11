select * from Product
select * from Client
select * from InventoryDeliveries where PackagesUIDs like '%6304150219000013%'
select * from MobileTransaction where UIDCode = '0100201013000689'
select * from InventoryDeliveriesLocation where NeedsMatching = 1 order by 1 desc
select * from InventoryDeliveries where PackagesUIDs like '%4500210317006821%'
select * from InventoryDeliveries where PackagesUIDs like '%0100201013000689%'

select * from InventoryDeliveriesLocation IDL 
	inner join MobileTransaction MT on IDL.InventoryDeliveriesLocationID = MT.InventoryDeliveriesLocationID  
where IDL.NeedsMatching = 1  and UIDCode='4500210317006821' order by 1 desc

select * from InventoryDeliveriesLocation IDL 
	inner join MobileTransaction MT on IDL.InventoryDeliveriesLocationID = MT.InventoryDeliveriesLocationID  
where UIDCode='0100210114006906' order by 1 desc

select UIDCode, LF.Name as LFrom, LT.Name as LTo, * from InventoryDeliveriesLocation IDL 
	inner join MobileTransaction MT on IDL.InventoryDeliveriesLocationID = MT.InventoryDeliveriesLocationID  
	inner join Location LF on  LF.LocationID = IDL.LocationFromID 
	inner join Location LT on  LT.LocationID = IDL.LocationToID
where  UIDCode='4503210317000285' order by IDL.tsInsert desc

select UIDCode, LF.Name as LFrom, LT.Name as LTo, * from InventoryDeliveriesLocation IDL 
	inner join MobileTransaction MT on IDL.InventoryDeliveriesLocationID = MT.InventoryDeliveriesLocationID  
	inner join Location LF on  LF.LocationID = IDL.LocationFromID 
	inner join Location LT on  LT.LocationID = IDL.LocationToID
where IDL.NeedsMatching = 1 order by IDL.tsInsert desc

select * from Location
select * from Users