<?php
namespace App\Controllers;

use CodeIgniter\Model;
use App\Models\Genre;

/**
 *
 * @property Model model
 */
class Genres extends MyResourceController
{
    protected $modelName = Genre::class;
}
