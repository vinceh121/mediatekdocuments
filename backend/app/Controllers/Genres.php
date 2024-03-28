<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use CodeIgniter\Model;
use App\Models\Genre;

/**
 *
 * @property Model model
 */
class Genres extends ResourceController
{

    protected $format = 'json';

    protected $modelName = Genre::class;

    public function index()
    {
        return $this->respond($this->model->findAll());
    }

    public function show($id = null)
    {
        if (!$id) {
            return $this->failNotFound();
        }

        return $this->respond($this->model->find($id));
    }
}
