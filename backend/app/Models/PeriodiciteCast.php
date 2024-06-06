<?php
namespace App\Models;

use CodeIgniter\Entity\Cast\BaseCast;

class PeriodiciteCast extends BaseCast
{

    /**
     *
     * {@inheritdoc}
     * @see \CodeIgniter\Entity\Cast\BaseCast::get()
     */
    public static function get($value, array $params = array())
    {
        return Periodicite::from($value);
    }

    /**
     *
     * {@inheritdoc}
     * @see \CodeIgniter\Entity\Cast\BaseCast::set()
     */
    public static function set($value, array $params = [])
    {
        if (is_string($value)) {
            return $value;
        }

        return $value->value;
    }
}

