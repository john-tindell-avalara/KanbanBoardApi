var KanbanBoardApp;
(function (KanbanBoardApp) {
    var app = angular.module('KanbanBoardApp', ['ngDraggable', 'ui.bootstrap']);
    app.controller("BoardController", ['$scope', '$http', '$modal', KanbanBoardApp.BoardController]);
    app.controller("AddBoardController", ['$scope', '$http', '$modalInstance', KanbanBoardApp.AddBoardController]);
    app.controller("AddColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', KanbanBoardApp.AddColumnController]);
    app.controller("UpdateColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'currentColumn', KanbanBoardApp.UpdateColumnController]);
    app.controller("AddTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columnSlug', KanbanBoardApp.AddTaskController]);
    app.controller("UpdateTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columns', 'currentTask', KanbanBoardApp.UpdateTaskController]);
})(KanbanBoardApp || (KanbanBoardApp = {}));
