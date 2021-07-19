select UIDCode, tsInsert, count(*) from MobileTransaction 
group by UIDCode, tsInsert
having count(*) > 1
order by 3 desc

select AtomeUID250, count(*) from InventoryDeliveries 
group by AtomeUID250
having count(*) > 1
order by 1 desc


select * from MobileTransaction where UIDCode = '4504210302000008'

0100210419000983
0100210421002021
0100210421003839
0100210421005964
0100210421004354
0100210421002026
0100210416002796
0100210421006671
0100210421000864
0100210421001432

select count(*) from MobileTransaction

alter table DeliveryNote add SalePermission varchar(4000)
alter table MobileTransaction add RowCnt int;

select * from Users

select * from InventoryDeliveriesLocation order by 1 desc


select UIDCode, tsInsert,tsUpdateUserID, count(*) from MobileTransaction 
group by UIDCode, tsInsert, tsUpdateUserID
having count(*) > 1
order by 4 desc
