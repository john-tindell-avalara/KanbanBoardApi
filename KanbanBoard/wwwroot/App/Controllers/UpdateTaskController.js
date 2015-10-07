var KanbanBoardApp;
(function (KanbanBoardApp) {
    var UpdateTaskController = (function () {
        function UpdateTaskController(scope, http, modalInstance, currentBoard, columns, currentTask) {
            var _this = this;
            this.scope = scope;
            this.http = http;
            this.modalInstance = modalInstance;
            this.currentBoard = currentBoard;
            this.columns = columns;
            this.currentTask = currentTask;
            this.scope.currentTask = currentTask;
            this.scope.columns = columns;
            scope.save = function () {
                if (_this.scope.updateTaskForm.$valid) {
                    _this.currentTask.Name = _this.scope.updateTaskForm.name.$viewValue;
                    _this.currentTask.Description = _this.scope.updateTaskForm.description.$viewValue;
                    console.log(_this.scope.updateTaskForm.columnSlug);
                    _this.currentTask.BoardColumnSlug = _this.scope.updateTaskForm.columnSlug.$viewValue;
                    _this.http.put("http://localhost:2943/boards/" + _this.currentBoard.Slug + "/tasks/" + _this.currentTask.Id, _this.currentTask).success(function (response) {
                        _this.scope.$emit('TaskUpdated', response);
                        modalInstance.dismiss(null);
                    }).error(function (error, status) {
                        scope.errorMessage = "Unknown error has occured";
                        _this.scope.updateTaskForm.name.$invalid = true;
                    });
                }
            };
            scope.cancel = function () {
                modalInstance.dismiss('cancel');
            };
        }
        return UpdateTaskController;
    })();
    KanbanBoardApp.UpdateTaskController = UpdateTaskController;
})(KanbanBoardApp || (KanbanBoardApp = {}));
