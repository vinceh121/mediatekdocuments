<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use CodeIgniter\Model;
use App\Models\ModelPublic;

/**
 *
 * @property Model model
 */
class Publics extends MyResourceController
{
    protected $modelName = ModelPublic::class;
}
