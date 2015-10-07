module KanbanBoardApp {
    export class Settings {
        //public static ApiLocation = "https://yeticode-kanbanboardapi.azurewebsites.net";
        public static ApiLocation = "http://localhost:2943/";
    }
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
                    tenant: 'johnyeticodeco.onmicrosoft.com',
                    clientId: '1349a492-7dd3-4105-9605-059e12770f9f',
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