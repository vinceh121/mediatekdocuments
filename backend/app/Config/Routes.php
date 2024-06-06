<?php
use CodeIgniter\Router\RouteCollection;

/**
 *
 * @var RouteCollection $routes
 */

$routes->get('content/(:any)', 'FileController::get/$1');
$routes->post('content', 'FileController::post');

$routes->group('api/v1', static function ($routes) {
    $routes->resource('books');
    $routes->resource('dvds');
    $routes->resource('revues');
    $routes->resource('bookdvdorders', [
        'controller' => 'BookDvdOrders'
    ]);

    $routes->resource('exemplaires', [
        'except' => [
            'show',
            'update'
        ]
    ]);
    $routes->get('exemplaires/(:segment)/(:segment)', 'Exemplaires::show/$1/$2');
    $routes->patch('exemplaires/(:segment)/(:segment)', 'Exemplaires::update/$1/$2');

    $routes->resource('genres');
    $routes->resource('publics');
    $routes->resource('aisles');
    $routes->resource('states');

    $routes->group('security', static function ($routes) {
        $routes->post('login', 'SecurityController::login');
    });
});
