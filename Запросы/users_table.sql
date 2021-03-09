create table statuses(
	id int not null auto_increment,
	primary key(id),

	name varchar(20) not null
);

insert into statuses
(name)
values ('Админ'), ('Пользователь'), ('Гость');

create table users(
	id int unsigned not null auto_increment,
	primary key(id),

	status_id int not null default 3,
	index(status_id),
	foreign key (status_id) references statuses(id),

	ulogin varchar(30) not null,
	usalt varchar(6) not null,
	upassword varchar(32) not null default ""
);

drop table users;
drop table statuses;

