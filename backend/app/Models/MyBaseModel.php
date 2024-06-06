<?php
namespace App\Models;

use CodeIgniter\Model;

abstract class MyBaseModel extends Model
{
    /**
     * {@inheritDoc}
     * @see \CodeIgniter\Model::update()
     */
    public function update($id = null, $row = null): bool
    {
        try
        {
            return parent::update($id, $row);
        }
        catch (\CodeIgniter\Database\Exceptions\DataException $e)
        {
            if ($this->allowEmptyInserts && $e->getMessage() !== lang('Database.emptyDataset', ['update']))
            {
                throw $e;
            }

            return false;
        }
    }
    
}

