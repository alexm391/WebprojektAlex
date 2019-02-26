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

insert into users values(null, "admin", "admin", null, 0, "admin", "admin@admin.com", sha1("admin!"), true);

