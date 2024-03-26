<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use CodeIgniter\HTTP\Response;

class Books extends ResourceController
{

    protected $format = 'json';

    public function index(): Response
    {
        $db = \Config\Database::connect();
        $res = $db->table('livre')->get();
        return $this->respond($res->getResultArray());
    }
}
