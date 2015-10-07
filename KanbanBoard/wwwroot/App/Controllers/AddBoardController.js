var KanbanBoardApp;
(function (KanbanBoardApp) {
    var AddBoardController = (function () {
        function AddBoardController(scope, http, modalInstance) {
            var _this = this;
            this.scope = scope;
            this.http = http;
            this.modalInstance = modalInstance;
            scope.save = function () {
                if (_this.scope.boardForm.$valid) {
                    var board = {
                        Name: _this.scope.boardForm.name.$viewValue
                    };
                    _this.http.post(KanbanBoardApp.Settings.ApiLocation + "/boards", board).success(function (response) {
                        _this.scope.$emit('BoardCreated', response);
                        modalInstance.dismiss(null);
                    }).error(function (error, status) {
                        scope.errorMessage = "Unknown error has occured";
                        _this.scope.boardForm.name.$invalid = true;
                    });
                }
            };
            scope.cancel = function () {
                modalInstance.dismiss('cancel');
            };
        }
        return AddBoardController;
    })();
    KanbanBoardApp.AddBoardController = AddBoardController;
})(KanbanBoardApp || (KanbanBoardApp = {}));
