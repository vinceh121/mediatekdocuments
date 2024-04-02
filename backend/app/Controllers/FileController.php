<?php
namespace App\Controllers;

use App\Controllers\BaseController;
use CodeIgniter\HTTP\ResponseInterface;
use CodeIgniter\Files\File;

class FileController extends BaseController
{

    public function get(string $fileName)
    {
        return $this->response->download(WRITEPATH . 'uploads/' . $fileName, null, true); // FIXME path traversal
    }

    public function post()
    {
        $this->request->getFile('photo')->store();
        return $this->response->appendBody('OK');
    }
}
