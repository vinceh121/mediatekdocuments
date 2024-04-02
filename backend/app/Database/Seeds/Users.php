<?php

namespace App\Database\Seeds;

use CodeIgniter\Database\Seeder;

class Users extends Seeder
{
    public function run()
    {
        $this->db->query("INSERT INTO `user` (name, service_id, password, email) VALUES('Adminadminton', 1, '$argon2id$v=19$m=16,t=2,p=1$cnhwbXY4bFNlYkJjRGhsag$WyOBUBqe6Y0VR42YkSnUJQ', 'admin@org');");
        $this->db->query("INSERT INTO `user` (name, service_id, password, email) VALUES('Pret Tencieux', 1, '$argon2id$v=19$m=16,t=2,p=1$cnhwbXY4bFNlYkJjRGhsag$WyOBUBqe6Y0VR42YkSnUJQ', 'pret@org');");
        $this->db->query("INSERT INTO `user` (name, service_id, password, email) VALUES('Culture', 1, '$argon2id$v=19$m=16,t=2,p=1$cnhwbXY4bFNlYkJjRGhsag$WyOBUBqe6Y0VR42YkSnUJQ', 'culture@org');");
    }
}
