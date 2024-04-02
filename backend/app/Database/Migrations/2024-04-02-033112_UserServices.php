<?php
namespace App\Database\Migrations;

use CodeIgniter\Database\Migration;

class UserServices extends Migration
{

    public function up()
    {
        $this->db->query("-- mediatekdocuments.service definition

CREATE TABLE `service` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` tinytext NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


-- mediatekdocuments.`user` definition

CREATE TABLE `user` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` tinytext NOT NULL DEFAULT '',
  `service_id` int(10) unsigned NOT NULL,
  `password` tinytext DEFAULT NULL,
  `email` tinytext DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_service_FK` (`service_id`),
  CONSTRAINT `user_service_FK` FOREIGN KEY (`service_id`) REFERENCES `service` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;");
    }

    public function down()
    {
        $this->forge->dropTable('user');
        $this->forge->dropTable('service');
    }
}
