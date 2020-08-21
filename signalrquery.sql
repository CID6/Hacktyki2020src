use CarsDB

delete from CarProducts
delete from CarProductCarFeature

insert into CarProducts values(1111, 12111, 1, 1)
insert into CarProducts values(1111, 1223, 1, 1)
insert into CarProducts values(1111, 16523, 1, 1)
insert into CarProducts values(1111, 12723, 2, 1)
insert into CarProducts values(1111, 12923, 1, 1)
insert into CarProducts values(1111, 12023, 1, 1)
go

UPDATE CarProducts
set [Year] = 2222
where [VIN] = 12111

UPDATE CarProducts
set [Year] = 1111
where [VIN] = 12111

select [Year], [VIN], CarModels.Name as ModelName, CarFactories.Name as FactoryName from [dbo].CarProducts
inner join [dbo].CarModels on CarModelId = [dbo].CarModels.Id
inner join [dbo].CarFactories on FactoryId = [dbo].CarFactories.Id