<?php
namespace App\Controllers;

use App\Controllers\BaseController;
use CodeIgniter\HTTP\ResponseInterface;
use CodeIgniter\Files\File;

class FileController extends BaseController
{

    public function get(string $fileName)
    {
        return $this->response->download(WRITEPATH . 'uploads/' . helper('security')->sanitize_filename($fileName), null, true);
    }

    public function post()
    {
        $this->request->getFile('photo')->store();
        return $this->response->appendBody('OK');
    }
}
