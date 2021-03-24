/*Add BuyerID to Location table*/
alter table Location
add BuyerID int null,
constraint FK_LocationBuyerID foreign key(BuyerID) references Client(ClientID);

/* Insert clients - buyers from locations */
insert into Client(ClientTypeID, Name)
values(2,'Èerin s.p.'),
(2,'Vidmar s.p.'),
(2,'Buh s.p.');


/* Update BuyerID values in Location table */
update Location
set Location.BuyerID = c.ClientID
from Location l join Client c on l.Name = c.Name

/*For esier access to  MobileTransaction for calucalting stock we add foreign key to it*/
alter table IssueDocumentPosition
add MobileTransactionID int null,
constraint FK_IsDocPosMobileTrans foreign key(MobileTransactionID) references MobileTransaction(MobileTransactionID);

