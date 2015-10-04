var KanbanBoardApp;
(function (KanbanBoardApp) {
    var UpdateColumnController = (function () {
        function UpdateColumnController(scope, http, modalInstance, currentBoard, currentColumn) {
            var _this = this;
            this.scope = scope;
            this.http = http;
            this.modalInstance = modalInstance;
            this.currentBoard = currentBoard;
            this.currentColumn = currentColumn;
            scope.currentColumn = currentColumn;
            scope.save = function () {
                if (_this.scope.updateColumnForm.$valid) {
                    currentColumn.Name = _this.scope.updateColumnForm.name.$viewValue;
                    _this.http.put("/boards/" + _this.currentBoard.Slug + "/columns/" + currentColumn.Slug, currentColumn).success(function (response) {
                        _this.scope.$emit('ColumnUpdated', response);
                        modalInstance.dismiss(null);
                    }).error(function (error, status) {
                        scope.errorMessage = "Unknown error has occured";
                        _this.scope.updateColumnForm.name.$invalid = true;
                    });
                }
            };
            scope.cancel = function () {
                modalInstance.dismiss('cancel');
            };
        }
        return UpdateColumnController;
    })();
    KanbanBoardApp.UpdateColumnController = UpdateColumnController;
})(KanbanBoardApp || (KanbanBoardApp = {}));
