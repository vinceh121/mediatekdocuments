<?php

namespace App\Database\Migrations;

use CodeIgniter\Database\Migration;

class NullableImages extends Migration
{
    public function up()
    {
        $this->db->table('document')->update(['image' => null], ['image' => '']);
    }

    public function down()
    {
        $this->db->table('document')->update(['image' => ''], ['image' => null]);
    }
}
