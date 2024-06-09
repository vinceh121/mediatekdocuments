<?php

namespace App\Models;

use CodeIgniter\Model;

class Dvd extends MyBaseModel
{
    protected $table            = 'dvd';
    protected $primaryKey       = 'id';
    protected $useAutoIncrement = false;
    protected $returnType       = 'array';
    protected $useSoftDeletes   = false;
    protected $protectFields    = true;
    protected $allowedFields    = [ 'id', 'synopsis', 'realisateur', 'duree' ];

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
        'duree' => 'int'
    ];

    public function aggregates(): self
    {
        return $this->select('dvd.*, document.*, rayon.libelle AS rayon, public.libelle AS public, genre.libelle AS genre, dvd.id AS id')
        ->join('document', 'dvd.id = document.id')
        ->join('rayon', 'document.idRayon = rayon.id')
        ->join('public', 'document.idPublic = public.id')
        ->join('genre', 'document.idGenre = genre.id')
        ->orderBy('dvd.id');
    }
}
