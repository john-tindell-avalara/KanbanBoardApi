module KanbanBoardApp {
    var app = angular.module('KanbanBoardApp', ['ngDraggable', 'ui.bootstrap']);

    app.config([
        '$httpProvider', ($httpProvider) => {
            $httpProvider.defaults.useXDomain = true;
            $httpProvider.defaults.headers.common = 'Content-Type: application/json';
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
        }
    ]);

    app.controller("BoardController", ['$scope', '$http', '$modal', BoardController]);
    app.controller("AddBoardController", ['$scope', '$http', '$modalInstance', AddBoardController]);
    app.controller("AddColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', AddColumnController]);
    app.controller("AddTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columnSlug', AddTaskController]);
    app.controller("UpdateTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columns', 'currentTask', UpdateTaskController]);
}