<?php

namespace App\Database\Seeds;

use CodeIgniter\Database\Seeder;

class Services extends Seeder
{
    public function run()
    {
        $this->db->query("INSERT INTO service (id, name) VALUES(1, 'Administration');");
        $this->db->query("INSERT INTO service (id, name) VALUES(2, 'Prêts');");
        $this->db->query("INSERT INTO service (id, name) VALUES(3, 'Culture');");
    }
}
