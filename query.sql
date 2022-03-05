create table react(
id int IDENTITY(1,1) primary key,
cmt_id int, 
list_user varchar(max),
total int
)
select* from comments
delete from react