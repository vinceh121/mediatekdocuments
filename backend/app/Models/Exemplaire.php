<?php

namespace App\Models;

use CodeIgniter\Model;

class Exemplaire extends MyBaseModel
{
    protected $table            = 'exemplaire';
    protected $primaryKey       = 'id';
    protected $useAutoIncrement = false;
    protected $returnType       = 'array';
    protected $useSoftDeletes   = false;
    protected $protectFields    = true;
    protected $allowedFields    = [ 'id', 'numero', 'dateAchat', 'photo', 'idEtat' ];

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

    public function aggregates(): self
    {
        return $this->select('exemplaire.*, document.*, etat.libelle AS etat')
        ->join('etat', 'exemplaire.idEtat = etat.id')
        ->join('document', 'exemplaire.id = document.id')
        ->orderBy('exemplaire.id');
    }
}
