<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use App\Models\Book;
use CodeIgniter\Model;
use CodeIgniter\HTTP\IncomingRequest;
use App\Models\Document;
use App\Models\BookDvd;
use App\Models\Dvd;

/**
 *
 * @property IncomingRequest request
 * @property Model model
 */
class Dvds extends ResourceController
{

    protected $format = 'json';

    protected $modelName = Dvd::class;

    public function index()
    {
        /** @var Book $builder */
        $builder = $this->model->aggregates();

        if ($this->request->getGet('realisateur')) {
            $builder->like('realisateur', sprintf('%%%s%%', $this->request->getGet('realisateur')));
        }

        if ($this->request->getGet('title')) {
            $builder->like('titre', sprintf('%%%s%%', $this->request->getGet('title')));
        }

        if ($this->request->getGet('synopsis')) {
            $builder->like('synopsis', sprintf('%%%s%%', $this->request->getGet('synopsis')));
        }

        if ($this->request->getGet('id')) {
            $builder->where('dvd.id', $this->request->getGet('id'));
        }

        if ($this->request->getGet('genre')) {
            $builder->where('idGenre', $this->request->getGet('genre'));
        }

        if ($this->request->getGet('public')) {
            $builder->where('idPublic', $this->request->getGet('public'));
        }

        if ($this->request->getGet('aisle')) {
            $builder->where('idRayon', $this->request->getGet('aisle'));
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

    public function update($id = null)
    {
        $body = $this->request->getJSON();

        if (property_exists($body, 'id')) {
            return $this->fail('id cannot be specified');
        }

        if (
            $this->model->update($id, $body)
            && model(Document::class)->update($id, $body)
        ) {
            return $this->respondUpdated();
        } else {
            return $this->failNotFound();
        }
    }

    public function delete($id = null)
    {
        $this->model->delete($id);
        return $this->respondDeleted();
    }

    public function create()
    {
        $body = $this->request->getJSON();

        if (property_exists($body, 'id')) {
            return $this->fail("body can't contain id", 400);
        }

        $lastId = $this->model->orderBy('dvd.id', 'DESC')->first()['id'];
        $newId = sprintf("%'.05d", intval($lastId) + 1);
        $body->id = $newId;

        log_message('info', sprintf('creating book with id %s', $newId));

        model(Document::class)->insert($body);
        model(BookDvd::class)->insert($body);
        $this->model->insert($body);
    }
}
