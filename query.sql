create table comments(
id int IDENTITY(1,1) PRIMARY KEY,
post_id int,
user_id int,
content text,
created_at datetime null default current_timestamp
)
