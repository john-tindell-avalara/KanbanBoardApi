module KanbanBoardApp {
    var app = angular.module('KanbanBoardApp', ['ngRoute', 'ngDraggable', 'ui.bootstrap', 'AdalAngular']);
    
    
    app.config(
        ['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', ($routeProvider, $httpProvider, adalProvider) => {
            $routeProvider.when('/', {
                controller: 'BoardController',
                templateUrl: '/App/Views/Board.html',
                requireADLogin: true
            });

            //$httpProvider.defaults.useXDomain = true;
            //$httpProvider.defaults.headers.common = 'Content-Type: application/json';
            //delete $httpProvider.defaults.headers.common['X-Requested-With'];

            adalProvider.init(
                {
                    //instance: 'http://localhost:56134/',
                    //tenant: false,
                    tenant: 'yeticode2.onmicrosoft.com',
                    clientId: '294b29a5-f786-4745-adca-3f43185fdb36',
                    extraQueryParameter: 'nux=1'
                    //cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
                },
                $httpProvider
            );
        }]);

    app.controller("BoardController", ['$scope', '$http', '$modal', BoardController]);
    app.controller("AddBoardController", ['$scope', '$http', '$modalInstance', AddBoardController]);
    app.controller("AddColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', AddColumnController]);
    app.controller("AddTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columnSlug', AddTaskController]);
    app.controller("UpdateTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columns', 'currentTask', UpdateTaskController]);
}