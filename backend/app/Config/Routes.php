<?php
use CodeIgniter\Router\RouteCollection;

/**
 *
 * @var RouteCollection $routes
 */
$routes->group('api/v1', static function ($routes) {
    $routes->resource('books');

    $routes->resource('genres');
    $routes->resource('publics');
    $routes->resource('aisles');
});
