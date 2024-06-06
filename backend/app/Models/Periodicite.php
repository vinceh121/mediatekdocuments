<?php
namespace App\Models;

enum Periodicite: string
{
    case Mensuel = 'MS';
    case Hebdomadaire = 'HB';
    case Quotidien = 'QT';
}

