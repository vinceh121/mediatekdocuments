<?php
namespace App\Controllers;

use App\Controllers\BaseController;
use CodeIgniter\HTTP\ResponseInterface;
use CodeIgniter\Files\File;
use CodeIgniter\Security\Security;

class FileController extends BaseController
{

    public function get(...$pathSegments)
    {
        $path = implode('/', $pathSegments);
        $clean = \Config\Services::security()->sanitizeFilename($path, true);
        return $this->response->download(WRITEPATH . 'uploads/' . $clean, null, true);
    }

    public function post()
    {
        $path = $this->request->getFile('photo')->store();
        return $this->response->setStatusCode(201)->setJSON([
            'path' => $path,
        ]);
    }
}
