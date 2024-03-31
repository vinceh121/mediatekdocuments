<?php

namespace App\Models;

use CodeIgniter\Model;

class Book extends Model
{
    protected $table            = 'livre';
    protected $primaryKey       = 'id';
    protected $useAutoIncrement = false;
    protected $returnType       = 'array';
    protected $useSoftDeletes   = false;
    protected $protectFields    = true;
    protected $allowedFields    = [ 'ISBN', 'auteur', 'collection' ];

    protected bool $allowEmptyInserts = false;

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
        return $this->select('*, rayon.libelle AS rayon, public.libelle AS public, genre.libelle AS genre, livre.id AS id')
        ->join('document', 'livre.id = document.id')
        ->join('rayon', 'document.idRayon = rayon.id')
        ->join('public', 'document.idPublic = public.id')
        ->join('genre', 'document.idGenre = genre.id');
    }
}
