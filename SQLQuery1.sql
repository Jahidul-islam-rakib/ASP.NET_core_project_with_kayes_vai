use [CrudKayesVai]

Create table Person(
Id int not null,
Name nvarchar(50) not null,
Gender nvarchar(50) null,
Height nvarchar(50) null,
Weight nvarchar(50) null,
DOB Date null,
HairColor nvarchar(50) null,
PresentAddress nvarchar(200) null,
PermanentAddress nvarchar(200) null,
);


insert into Person(Id, Name, Gender,Height, Weight, DOB, HairColor,PresentAddress,PermanentAddress) values('2', 'habib','male', '4ft','50kg','2024-10-12','red','sl;dflsdfh present', 'parmanent sldfjos');