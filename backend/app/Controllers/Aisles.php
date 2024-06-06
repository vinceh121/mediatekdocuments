<?php
namespace App\Controllers;

use CodeIgniter\Model;
use App\Models\Aisle;

/**
 *
 * @property Model model
 */
class Aisles extends MyResourceController
{
    protected $modelName = Aisle::class;
}
