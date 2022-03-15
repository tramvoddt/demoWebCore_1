create table reports(
id int IDENTITY(1,1) primary key,
cmt_id int,
list_user varchar(max),
total int
)
drop table reports