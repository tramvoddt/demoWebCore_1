create table reports(
id int IDENTITY(1,1) PRIMARY KEY,
cmt_id int,
list_user varchar(max),
total int
)
select*from save_post
select*from collection
delete from reports
set identity_insert reports off;
select*from users
alter table reports
add status int default 0;
update users set avt=null where id=8
update reports set status=0 where id=7
delete from reports where cmt_id=51
delete from users where id =1008