create table reports(
id int IDENTITY(1,1) PRIMARY KEY,
cmt_id int,
list_user varchar(max),
total int
)
select*from reports
select*from users
delete from reports
set identity_insert reports off;
select*from comments
alter table reports
add status int default 0;
update reports set list_user='[5]' where id=6