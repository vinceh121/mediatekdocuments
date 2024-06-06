<?php
namespace App\Controllers;

use CodeIgniter\Model;
use CodeIgniter\HTTP\IncomingRequest;
use App\Models\Order;
use App\Models\BookDvdOrder;

/**
 *
 * @property IncomingRequest request
 * @property Model model
 */
class BookDvdOrders extends MyResourceController
{
    protected $modelName = BookDvdOrder::class;
    protected array $searchFields = [];
    protected array $fields = [
        'id',
        'nbExemplaire',
        'idLivreDvd',
        'dateCommande',
        'montant'
    ];

    public function update($id = null)
    {
        $body = $this->request->getJSON();

        if (property_exists($body, 'id')) {
            return $this->fail('id cannot be specified');
        }

        if ($this->model->update($id, $body) || model(Order::class)->update($id, $body)) {
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

        if (property_exists($body, 'dateCommande')) {
            return $this->fail("body can't contain dateCommande", 400);
        }

        $body->dateCommande = (new \DateTime())->format(\DateTime::ISO8601_EXPANDED);

        $last = $this->model->orderBy('commandedocument.id', 'DESC')->first();

        if ($last) {
            $lastId = $last['id'];
            $newId = sprintf("%'.05d", intval($lastId) + 1);
            $body->id = $newId;
        } else {
            $body->id = sprintf("%'.05d", 0);
        }

        model(Order::class)->insert($body);
        $this->model->insert($body);

        return $this->respondCreated();
    }
}
