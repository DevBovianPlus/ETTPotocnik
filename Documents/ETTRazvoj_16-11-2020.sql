select * from Location
select * from InventoryStock
select * from InventoryDeliveriesLocation where NeedsMatching=1
select * from InventoryDeliveriesLocation where InventoryDeliveriesLocationID = 129644
select * from InventoryDeliveries
select * from InventoryDeliveries where PackagesUIDs like '%4503201030000265%'
select * from MobileTransaction where UIDCode = '4504201030000015'
select * from InventoryDeliveriesPackages where ElementUID250 like '%4503201030000265%'
select * from InventoryDeliveriesPackages where InventoryDeliveriesPackagesID = 135194 
select * from DeliveryNote
select * from DeliveryNoteItem
select * from DeliveryNoteStatus
select * from Users
select * from Product
select * from Employee
select * from Client


select * from DeliveryNote
where DeliveryNoteID = 37

select * from DeliveryNoteItem
where DeliveryNoteID=37

update DeliveryNote
set DeliveryNoteStatusID = 4,
ProcessError = NULL
where DeliveryNoteID in(37)


delete  from DeliveryNoteItem
where DeliveryNoteItemID in(80,81,82)

select * from InventoryDeliveries
where DeliveryNoteItemID in(80,81,82)
