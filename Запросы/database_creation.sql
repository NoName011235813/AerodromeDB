create database aerodrome_tech;

use aerodrome_tech;

create table maint_types (
	id int not null auto_increment,
	primary key(id),

	name varchar(20) not null,
	period int not null
		check (period between 1 and 600)
);

create table plane_model (
	id int not null auto_increment,
	primary key(id),

	manufacturer varchar(70) not null,
	name varchar(100) not null,

	fuel_tank_volume int not null
		check (fuel_tank_volume between 1 and 1000000),

	places_num int not null
		check (places_num between 1 and 1000),

	max_lifting_capacity int not null
		check (max_lifting_capacity between 1 and 700000)
);

create table group_leader (
	id int not null auto_increment,
	primary key(id),

	name varchar(70) not null,
	phone varchar(14) not null,
	
	experience int not null
		check (experience between 2 and 40),

	passport varchar(14) not null,
	about text
);

create table workload (
	id int not null auto_increment,
	primary key(id),

	maint_type_id int not null,
	index(maint_type_id),
	foreign key (maint_type_id) references maint_types(id)
		on delete cascade
		on update cascade,

	name varchar(10) not null
);

create table plane (
	id int not null auto_increment,
	primary key(id),

	registration_number varchar(10) not null,

	model_id int not null,
	index(model_id),
	foreign key (model_id) references plane_model(id)
		on delete cascade
		on update cascade,

	parking_place_id varchar(15) not null
);

create table tech_group (
	id int not null auto_increment,
	primary key(id),

	group_name varchar(25) not null,
	
	leader_id int not null,
	index(leader_id),
	foreign key (leader_id) references group_leader(id)
		on delete cascade
		on update cascade,

	about text
);

create table works_history (
	id int not null auto_increment,
	primary key(id),

	workload_id int not null,
	index(workload_id),
	foreign key (workload_id) references workload(id)
		on delete cascade
		on update cascade,

	plane_id int not null,
	index(plane_id),
	foreign key (plane_id) references plane(id)
		on delete cascade
		on update cascade,

	t_group_id int not null,
	index(t_group_id),
	foreign key (t_group_id) references tech_group(id)
		on delete cascade
		on update cascade,

	work_date date not null,
	more text
);