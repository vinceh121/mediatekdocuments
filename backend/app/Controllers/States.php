<?php
namespace App\Controllers;

use CodeIgniter\RESTful\ResourceController;
use CodeIgniter\Model;
use App\Models\State;

/**
 *
 * @property Model model
 */
class States extends MyResourceController
{
    protected $modelName = State::class;
}
