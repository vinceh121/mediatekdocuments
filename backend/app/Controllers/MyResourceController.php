<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use App\Models\Book;
use CodeIgniter\Model;
use CodeIgniter\HTTP\IncomingRequest;
use App\Models\Document;
use App\Models\BookDvd;

/**
 *
 * @property IncomingRequest request
 * @property Model model
 */
abstract class MyResourceController extends ResourceController
{

    protected $format = 'json';

    protected array $searchFields = [];

    protected array $fields = [];

    public function index()
    {
        /** @var Book $builder */
        $builder = $this->model->aggregates();

        foreach ($this->searchFields as $f) {
            if ($this->request->getGet($f)) {
                $builder->like($f, sprintf('%%%s%%', $this->request->getGet($f)));
            }
        }

        foreach ($this->fields as $f) {
            if ($this->request->getGet($f)) {
                $builder->where($f, $this->request->getGet($f));
            }
        }

        return $this->respond($builder->findAll());
    }

    public function show($id = null)
    {
        if (!$id) {
            return $this->failNotFound();
        }

        return $this->respond($this->model->aggregates()
            ->find($id));
    }

    public function delete($id = null)
    {
        $this->model->delete($id);
        return $this->respondDeleted();
    }
}
