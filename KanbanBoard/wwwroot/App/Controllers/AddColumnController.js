var KanbanBoardApp;
(function (KanbanBoardApp) {
    var AddColumnController = (function () {
        function AddColumnController(scope, http, modalInstance, currentBoard) {
            var _this = this;
            this.scope = scope;
            this.http = http;
            this.modalInstance = modalInstance;
            this.currentBoard = currentBoard;
            scope.save = function () {
                if (_this.scope.columnForm.$valid) {
                    var column = { Name: _this.scope.columnForm.name.$viewValue };
                    _this.http.post(KanbanBoardApp.Settings.ApiLocation + "/boards/" + _this.currentBoard.Slug + "/columns", column).success(function (response) {
                        _this.scope.$emit('newColumnCreated', response);
                        modalInstance.dismiss(null);
                    }).error(function (error, status) {
                        if (status === 409) {
                            scope.errorMessage = "Column with this name already exists";
                        }
                        else {
                            scope.errorMessage = "Unknown error has occured";
                        }
                        _this.scope.columnForm.name.$invalid = true;
                    });
                }
            };
            scope.cancel = function () {
                modalInstance.dismiss('cancel');
            };
        }
        return AddColumnController;
    })();
    KanbanBoardApp.AddColumnController = AddColumnController;
})(KanbanBoardApp || (KanbanBoardApp = {}));
