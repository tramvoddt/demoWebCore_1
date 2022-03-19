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
update comments set sts=0 where id=49
update reports set status=0 where id=7
delete from reports where cmt_id=51
