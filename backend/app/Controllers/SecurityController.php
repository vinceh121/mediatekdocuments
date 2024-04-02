<?php
namespace App\Controllers;

use App\Controllers\BaseController;
use CodeIgniter\HTTP\ResponseInterface;
use App\Models\User;
use CodeIgniter\API\ResponseTrait;
use App\Models\Service;

class SecurityController extends BaseController
{
    use ResponseTrait;

    public function login()
    {
        $body = $this->request->getJSON();

        $email = $body->email;
        $password = $body->password;

        $user = model(User::class)->where('email', $email)->first();

        if (!$user || !password_verify($password, $user['password'])) {
            log_message('info', sprintf('failed authentication for user %s', $email));

            return $this->failForbidden('Mauvais email ou mot de passe');
        } else {
            if ($user['service_id'] == Service::CULTURE) {
                return $this->failForbidden("Les utilisateurs du service culture n'ont pas l'autorisation d'utiliser cette application");
            }

            log_message('info', sprintf('successful authentication for user %s', $email));
            session()->set('userId', $user['id']);

            return $this->respond(null, 200);
        }
    }
}
