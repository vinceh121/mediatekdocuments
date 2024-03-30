<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use CodeIgniter\Model;
use App\Models\Aisle;

/**
 *
 * @property Model model
 */
class Aisles extends ResourceController
{
    protected $format = 'json';
    protected $modelName = Aisle::class;

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
