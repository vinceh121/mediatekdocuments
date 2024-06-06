<?php
namespace App\Controllers;

use App\Models\Book;
use CodeIgniter\Model;
use CodeIgniter\HTTP\IncomingRequest;
use App\Models\Document;
use App\Models\BookDvd;
use App\Models\Exemplaire;

/**
 *
 * @property IncomingRequest request
 * @property Model model
 */
class Exemplaires extends MyResourceController
{

    protected $modelName = Exemplaire::class;

    protected array $fields = [
        'id',
        'numero',
        'dateAchat',
        'photo',
        'idEtat'
    ];

    public function show($id = null, $numero = null)
    {
        if (!$id || !$numero) {
            return $this->failNotFound();
        }

        $data = $this->model->aggregates()
            ->where('exemplaire.id', $id)
            ->where('exemplaire.numero', $numero)
            ->first();

        if ($data) {
            return $this->respond($data);
        } else {
            return $this->failNotFound();
        }
    }

    public function update($id = null, $numero = null)
    {
        $body = $this->request->getJSON();

        if (property_exists($body, 'id')) {
            return $this->fail('id cannot be specified');
        }

        if (property_exists($body, 'numero')) {
            return $this->fail('numero cannot be specified');
        }

        if ($this->model->update([
            'id' => $id,
            'numero' => $numero
        ], $body)) {
            return $this->respondUpdated();
        } else {
            return $this->failNotFound();
        }
    }

    public function create()
    {
        $body = $this->request->getJSON();

        if (!property_exists($body, 'id')) {
            return $this->fail("body must contain id", 400);
        }

        $this->model->insert($body);
    }
}
