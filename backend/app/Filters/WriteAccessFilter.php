<?php
namespace App\Filters;

use CodeIgniter\Filters\FilterInterface;
use CodeIgniter\HTTP\RequestInterface;
use CodeIgniter\HTTP\ResponseInterface;
use App\Models\User;
use App\Models\Service;

class WriteAccessFilter implements FilterInterface
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
        if ($request->getMethod() === 'get') {
            return;
        }

        $session = session();

        if (!$session->get('userId')) {
            return response()->setStatusCode(401)->setJSON([
                'messages' => [
                    'error' => 'need to be authenticated'
                ]
            ]);
        }

        $user = model(User::class)->find($session->get('userId'));

        if ($user['service_id'] != Service::ADMIN) {
            return response()->setStatusCode(403)->setJSON([
                'messages' => [
                    'error' => 'not enough permission'
                ]
            ]);
        }
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
