select * from Product
select * from Client
select * from InventoryDeliveries where PackagesUIDs like '%6304150219000013%'
select * from MobileTransaction where UIDCode = '4504210505000020'
select * from InventoryDeliveriesLocation where NeedsMatching = 1 order by 1 desc
select * from InventoryDeliveries where PackagesUIDs like '%4500210317006821%'
select * from InventoryDeliveries where PackagesUIDs like '%0100201013000689%'

select * from DeliveryNote order by tsInsert desc

select * from DeliveryNote where  DeliveryNoteNumber ='wwwww'
update DeliveryNote set DeliveryNoteStatusID=4 where DeliveryNoteNumber ='wwwww'

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
select * from InventoryDeliveries where InventoryDeliveriesID = 201268 order by 1 desc
select * from InventoryDeliveriesLocation  order by tsUpdate desc
select * from InventoryDeliveriesPackages where tsInsert between '2021-06-16' and '2021-06-18'  order by 1 desc
select * from InventoryDeliveriesPackages where tsInsert between '2021-06-16' and '2021-06-18'  order by 1 desc
select * from InventoryDeliveriesLocation where tsInsert between '2021-06-16' and '2021-06-18'  order by 1 desc
select * from InventoryDeliveries where tsInsert between '2021-06-16' and '2021-06-18'  order by 1 desc
select count(*) from InventoryDeliveriesPackages  
select * from InventoryDeliveriesPackages  order by tsInsert desc
select count(*) from InventoryDeliveriesLocation where NeedsMatching = 1
select * from InventoryDeliveriesLocation where ParentID = 394921
select * from InventoryDeliveriesLocation  order by tsUpdate desc
select * from InventoryDeliveriesLocation  order by 1 desc
select * from InventoryDeliveriesLocation where InventoryDeliveriesID is not null and NeedsMatching = 1
select * from InventoryDeliveriesLocation where InventoryDeliveriesLocationID = 391929
select * from InventoryDeliveriesLocation where InventoryDeliveriesID = 171798 order by tsInsert desc
select * from InventoryDeliveriesLocation where InventoryDeliveriesLocationID in (select InventoryDeliveriesLocationID from InventoryDeliveries where PackagesUIDs like '%4503210505000780%') and NeedsMatching = 1

select * from InventoryDeliveriesPackages 	where InventoryDeliveriesPackagesID = 213469
select * from InventoryDeliveriesPackages 	where InventoryDeliveriesPackagesID in 
	(select InventoryDeliveriesPackagesID from InventoryDeliveries where PackagesUIDs like '%4504210504000033%') order by ParentElementID 
select * from InventoryDeliveries where InventoryDeliveriesID = 201748
select * from InventoryDeliveries order by 1 desc
select * from InventoryDeliveries where InventoryDeliveriesID = 171798
select * from InventoryDeliveries where AtomeUID250 = '4504210504000033'
select * from InventoryDeliveries where PackagesUIDs like '%4504210531000026%'
select * from MobileTransaction where InventoryDeliveriesLocationID = 398128
select * from MobileTransaction where ScannedProductCode like '%2203210212000013%'
select * from MobileTransaction where UIDCode like '%0100210511009175%'
select * from MobileTransaction order by tsUpdate desc
select * from MobileTransaction where InventoryDeliveriesLocationID = 396864

select IDL.*, L.Name from InventoryDeliveriesLocation IDL inner join Location L on IDL.LocationToID = L.LocationID  where NeedsMatching = 1 and L.IsBuyer <> 1

select InventoryDeliveriesID, count(*) from InventoryDeliveriesLocation 
group by InventoryDeliveriesID
having count(*) > 1
order by 2 desc

select UIDCode, tsInsert,tsUpdateUserID, count(*) from MobileTransaction 
group by UIDCode, tsInsert, tsUpdateUserID
having count(*) > 1
order by 4 desc


SET IDENTITY_INSERT DeliveryNoteStatus ON;

INSERT into DeliveryNoteStatus(Code, Name, tsInsert, tsInsertUserID, Notes, tsup) values ('ERR', 'Napaka pri uvozu',null,null,null,null,null);

SET IDENTITY_INSERT DeliveryNoteStatus OFF;



update InventoryDeliveriesLocation set NeedsMatching = 1 where InventoryDeliveriesLocationID > 398127
update InventoryDeliveriesLocation set NeedsMatching = 1 where InventoryDeliveriesLocationID = 392473

select * from InventoryDeliveriesLocation where InventoryDeliveriesLocationID = 394921
select * from DeliveryNote order by 1 desc
select * from IssueDocument order by 1 desc
select * from DeliveryNoteItem order by 1 desc
select * from InventoryDeliveries order by 1 desc
select * from InventoryDeliveries where PackagesUIDs like '%1900210209002335%'

select * from MobileTransaction where InventoryDeliveriesLocationID = 398128
select * from MobileTransaction where ScannedProductCode like '%6304150219000013%'
select * from MobileTransaction where UIDCode like '%0100210514002493%'
select * from MobileTransaction where tsInsert between '2021-07-01' and '2021-07-18'  order by 1 desc
