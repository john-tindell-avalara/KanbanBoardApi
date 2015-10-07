module KanbanBoardApp {
    var app = angular.module('KanbanBoardApp', ['ngDraggable', 'ui.bootstrap']);
    app.controller("BoardController", ['$scope', '$http', '$modal', BoardController]);
    app.controller("AddBoardController", ['$scope', '$http', '$modalInstance', AddBoardController]);
    app.controller("AddColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', AddColumnController]);
    app.controller("UpdateColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'currentColumn', UpdateColumnController]);
    app.controller("AddTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columnSlug', AddTaskController]);
    app.controller("UpdateTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columns', 'currentTask', UpdateTaskController]);
}