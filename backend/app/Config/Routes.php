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

    $routes->resource('genres');
    $routes->resource('publics');
    $routes->resource('aisles');

    $routes->group('security', static function ($routes) {
        $routes->post('login', 'SecurityController::login');
    });
});
