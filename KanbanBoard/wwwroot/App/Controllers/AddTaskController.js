var KanbanBoardApp;
(function (KanbanBoardApp) {
    var AddTaskController = (function () {
        function AddTaskController(scope, http, modalInstance, currentBoard, columnSlug) {
            var _this = this;
            this.scope = scope;
            this.http = http;
            this.modalInstance = modalInstance;
            this.currentBoard = currentBoard;
            this.columnSlug = columnSlug;
            scope.save = function () {
                if (_this.scope.addTaskForm.$valid) {
                    var task = {
                        Name: _this.scope.addTaskForm.name.$viewValue,
                        Description: _this.scope.addTaskForm.description.$viewValue,
                        BoardColumnSlug: _this.columnSlug
                    };
                    _this.http.post(KanbanBoardApp.Settings.ApiLocation + "/boards/" + _this.currentBoard.Slug + "/tasks", task).success(function (response) {
                        _this.scope.$emit('newTaskCreated', response);
                        modalInstance.dismiss(null);
                    }).error(function (error, status) {
                        scope.errorMessage = "Unknown error has occured";
                        _this.scope.addTaskForm.name.$invalid = true;
                    });
                }
            };
            scope.cancel = function () {
                modalInstance.dismiss('cancel');
            };
        }
        return AddTaskController;
    })();
    KanbanBoardApp.AddTaskController = AddTaskController;
})(KanbanBoardApp || (KanbanBoardApp = {}));
