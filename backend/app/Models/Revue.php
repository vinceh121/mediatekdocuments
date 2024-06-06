<?php

namespace App\Models;

use CodeIgniter\Model;

class Revue extends MyBaseModel
{
    protected $table            = 'revue';
    protected $primaryKey       = 'id';
    protected $useAutoIncrement = false;
    protected $returnType       = 'array';
    protected $useSoftDeletes   = false;
    protected $protectFields    = true;
    protected $allowedFields    = [ 'id', 'periodicite', 'delaiMiseADispo' ];

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

    protected array $casts = [
        'periodicite' => 'periodicite',
    ];
    protected array $castHandlers = [
        'periodicite' => PeriodiciteCast::class,
    ];
    
    public function aggregates(): self
    {
        return $this->select('revue.*, document.*, rayon.libelle AS rayon, public.libelle AS public, genre.libelle AS genre, revue.id AS id')
        ->join('document', 'revue.id = document.id')
        ->join('rayon', 'document.idRayon = rayon.id')
        ->join('public', 'document.idPublic = public.id')
        ->join('genre', 'document.idGenre = genre.id')
        ->orderBy('revue.id');
    }
}
