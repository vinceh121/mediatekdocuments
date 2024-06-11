<?php
namespace App\Controllers;

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
class Dvds extends MyResourceController
{

    protected $modelName = Dvd::class;
    
    protected array $searchFields = [ 'realisateur', 'titre', 'synopsis' ];
    protected array $fields = [ 'id', 'genre', 'public', 'aisle', 'idRayon', 'idPublic', 'idGenre' ];

    public function update($id = null)
    {
        $body = $this->request->getJSON();

        if (property_exists($body, 'id')) {
            return $this->fail('id cannot be specified');
        }

        if ($this->model->update($id, $body) || model(Document::class)->update($id, $body)) {
            return $this->respondUpdated();
        } else {
            return $this->failNotFound();
        }
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
