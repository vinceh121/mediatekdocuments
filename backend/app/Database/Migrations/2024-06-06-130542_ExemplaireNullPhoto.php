<?php
namespace App\Database\Migrations;

use CodeIgniter\Database\Migration;

class ExemplaireNullPhoto extends Migration
{

    public function up()
    {
        $this->forge->modifyColumn('exemplaire', [
            'photo' => [
                'type' => 'VARCHAR',
                'constraint' => '500',
                'default' => null,
                'null' => true
            ]
        ]);

        $this->db->table('exemplaire')->update([
            'photo' => null
        ], [
            'photo' => ''
        ]);
    }

    public function down()
    {
        $this->forge->modifyColumn('exemplaire', [
            'photo' => [
                'type' => 'VARCHAR',
                'constraint' => '500',
                'default' => '',
                'null' => false
            ]
        ]);

        $this->db->table('exemplaire')->update([
            'photo' => ''
        ], [
            'photo' => null
        ]);
    }
}
