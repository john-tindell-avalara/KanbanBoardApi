var KanbanBoardApp;
(function (KanbanBoardApp) {
    var Settings = (function () {
        function Settings() {
        }
        //public static ApiLocation = "https://yeticode-kanbanboardapi.azurewebsites.net";
        Settings.ApiLocation = "http://localhost:2943/";
        return Settings;
    })();
    KanbanBoardApp.Settings = Settings;
    var app = angular.module('KanbanBoardApp', ['ngRoute', 'ngDraggable', 'ui.bootstrap', 'AdalAngular']);
    app.config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider', function ($routeProvider, $httpProvider, adalProvider) {
            $routeProvider.when('/', {
                controller: 'BoardController',
                templateUrl: '/App/Views/Board.html',
                requireADLogin: true
            });
            //$httpProvider.defaults.useXDomain = true;
            //$httpProvider.defaults.headers.common = 'Content-Type: application/json';
            //delete $httpProvider.defaults.headers.common['X-Requested-With'];
            adalProvider.init({
                //instance: 'http://localhost:56134/',
                //tenant: false,
                tenant: 'johnyeticodeco.onmicrosoft.com',
                clientId: '1349a492-7dd3-4105-9605-059e12770f9f',
                extraQueryParameter: 'nux=1'
            }, $httpProvider);
        }]);
    app.controller("BoardController", ['$scope', '$http', '$modal', KanbanBoardApp.BoardController]);
    app.controller("AddBoardController", ['$scope', '$http', '$modalInstance', KanbanBoardApp.AddBoardController]);
    app.controller("AddColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', KanbanBoardApp.AddColumnController]);
    app.controller("AddTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columnSlug', KanbanBoardApp.AddTaskController]);
    app.controller("UpdateTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columns', 'currentTask', KanbanBoardApp.UpdateTaskController]);
})(KanbanBoardApp || (KanbanBoardApp = {}));
