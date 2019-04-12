create database webProject collate utf8_general_ci;
use webProject;

create table users(
	id int not null auto_increment,
    firstname varchar(100) null,
	lastname varchar(100) not null,
    birthdate date null,
	gender tinyint null,
	username varchar(100) null,
	email varchar(40) not null unique,
    passwrd varchar(40) not null,
    isAdmin boolean not null default false,
    constraint id_PK primary key(id)
)engine=InnoDB;

create table rooms(
	roomNr int not null unique,
    beds int not null,
    price decimal not null,
    constraint roomNr_PK primary key(roomNr)
)engine=InnoDB;

create table bookings(
	id int not null auto_increment,
    idUser int not null,
    roomNr int not null,
    startDate date not null,
    endDate date not null,
    price decimal not null,
    constraint idUser_FK foreign key(idUser) references users(id),
	constraint roomNr_FK foreign key(roomNr) references rooms(roomNr),
	constraint id_PK primary key(id)
)engine=InnoDB;

insert into users values(null, "admin", "admin", null, 0, "admin", "admin@admin.com", sha1("admin!"), true);

insert into rooms values(1, 1, 30);
insert into rooms values(2, 2, 40);
insert into rooms values(3, 3, 50);
insert into rooms values(4, 1, 30);

insert into bookings values(null, 2, 1, "2019-04-20", "2019-04-22", 60);
insert into bookings values(null, 4, 4, "2019-04-20", "2019-04-22", 60);










