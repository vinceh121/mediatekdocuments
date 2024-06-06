<?php
namespace App\Controllers;

use CodeIgniter\Model;
use CodeIgniter\HTTP\IncomingRequest;
use App\Models\Document;
use App\Models\BookDvd;
use App\Models\Revue;

/**
 *
 * @property IncomingRequest request
 * @property Model model
 */
class Revues extends MyResourceController
{

    protected $modelName = Revue::class;

    protected array $searchFields = [
        'titre'
    ];

    protected array $fields = [
        'id',
        'periodicite',
        'delaiMiseADispo',
        'idGenre',
        'idPublic',
        'idRayon'
    ];

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

        $lastId = $this->model->orderBy('revue.id', 'DESC')->first()['id'];
        $newId = sprintf("%'.05d", intval($lastId) + 1);
        $body->id = $newId;

        log_message('info', sprintf('creating revue with id %s', $newId));

        model(Document::class)->insert($body);
        $this->model->insert($body);
    }
}
