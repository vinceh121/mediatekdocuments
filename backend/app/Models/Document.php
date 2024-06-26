<?php

namespace App\Models;

use CodeIgniter\Model;

class Document extends MyBaseModel
{
    protected $table            = 'document';
    protected $primaryKey       = 'id';
    protected $useAutoIncrement = false;
    protected $returnType       = 'array';
    protected $useSoftDeletes   = false;
    protected $protectFields    = true;
    protected $allowedFields    = [ 'id', 'titre', 'image', 'idRayon', 'idPublic', 'idGenre' ];

    protected bool $allowEmptyInserts = true;

    // Dates
    protected $useTimestamps = false;
    protected $dateFormat    = 'datetime';
    protected $createdField  = 'created_at';
    protected $updatedField  = 'updated_at';
    protected $deletedField  = 'deleted_at';

    // Validation
    protected $validationRules      = [];
    protected $validationMessages   = [];
    protected $skipValidation       = false;
    protected $cleanValidationRules = true;

    // Callbacks
    protected $allowCallbacks = true;
    protected $beforeInsert   = [];
    protected $afterInsert    = [];
    protected $beforeUpdate   = [];
    protected $afterUpdate    = [];
    protected $beforeFind     = [];
    protected $afterFind      = [];
    protected $beforeDelete   = [];
    protected $afterDelete    = [];
    
    public static function modelOf($id): MyBaseModel
    {
        $first = $id[0];
        
        switch ($first) {
            case '0':
                return model(Book::class);
            case '1':
                return model(Revue::class);
            case '2':
                return model(Dvd::class);
            default:
                throw new \Exception('could not determine document type from id');
        }
    }
}
