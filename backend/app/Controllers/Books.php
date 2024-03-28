<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use App\Models\Book;
use CodeIgniter\Model;
use CodeIgniter\HTTP\IncomingRequest;

/**
 *
 * @property Model model
 */
class Books extends ResourceController
{

    protected $format = 'json';

    protected $modelName = Book::class;

    public function index()
    {
        /** @var Book $builder */
        $builder = $this->model->join('document', 'livre.id = document.id');

        if ($this->request->getGet('author')) {
            $builder->like('auteur', sprintf('%%%s%%', $this->request->getGet('author')));
        }

        if ($this->request->getGet('title')) {
            $builder->like('titre', sprintf('%%%s%%', $this->request->getGet('title')));
        }

        if ($this->request->getGet('isbn')) {
            $builder->like('ISBN', sprintf('%%%s%%', $this->request->getGet('isbn')));
        }

        return $this->respond($builder->findAll());
    }

    public function show($id = null)
    {
        if (!$id) {
            return $this->failNotFound();
        }

        return $this->respond($this->model->join('document', 'livre.id = document.id')
            ->find($id));
    }

    public function update($id = null)
    {
        return $this->fail(lang('RESTful.notImplemented', [
            'update'
        ]), 501);
    }
}
