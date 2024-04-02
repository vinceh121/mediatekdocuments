<?php
namespace App\Filters;

use CodeIgniter\Filters\FilterInterface;
use CodeIgniter\HTTP\RequestInterface;
use CodeIgniter\HTTP\ResponseInterface;
use App\Models\User;

class AuthFilter implements FilterInterface
{

    /**
     * Do whatever processing this filter needs to do.
     * By default it should not return anything during
     * normal execution. However, when an abnormal state
     * is found, it should return an instance of
     * CodeIgniter\HTTP\Response. If it does, script
     * execution will end and that Response will be
     * sent back to the client, allowing for error pages,
     * redirects, etc.
     *
     * @param RequestInterface $request
     * @param array|null $arguments
     *
     * @return RequestInterface|ResponseInterface|string|void
     */
    public function before(RequestInterface $request, $arguments = null)
    {
        $header = $request->header('Authorization');

        if (!$header || is_array($header)) {
            return response()->setStatusCode(401)
                ->appendHeader('WWW-Authenticate', 'Basic realm=RESTAPI')
                ->setJSON([
                'message' => 'exactly one basic authorization must be provided'
            ]);
        }

        if (!str_starts_with($header->getValue(), 'Basic ')) {
            return $this->wrongUsernameOrPassword();
        }

        $authB64 = substr($header->getValue(), strlen('Basic '));

        $auth = base64_decode($authB64, true);
        $authParts = explode(':', $auth);

        if (count($authParts) !== 2) {
            return $this->wrongUsernameOrPassword();
        }

        $email = $authParts[0];
        $password = $authParts[1];

        $user = model(User::class)->where('email', $email)->first();

        if (!$user || !password_verify($password, $user['password'])) {
            return $this->wrongUsernameOrPassword();
        }
    }

    private function wrongUsernameOrPassword()
    {
        return response()->setStatusCode(403)->setJSON([
            'message' => 'wrong username or password'
        ]);
    }

    /**
     * Allows After filters to inspect and modify the response
     * object as needed.
     * This method does not allow any way
     * to stop execution of other after filters, short of
     * throwing an Exception or Error.
     *
     * @param RequestInterface $request
     * @param ResponseInterface $response
     * @param array|null $arguments
     *
     * @return ResponseInterface|void
     */
    public function after(RequestInterface $request, ResponseInterface $response, $arguments = null)
    {}
}
