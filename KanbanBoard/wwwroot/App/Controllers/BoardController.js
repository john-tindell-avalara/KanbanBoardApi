var KanbanBoardApp;
(function (KanbanBoardApp) {
    var BoardController = (function () {
        function BoardController(scope, http, modal) {
            var _this = this;
            this.scope = scope;
            this.http = http;
            this.modal = modal;
            this.scope.loading = true;
            http.get(KanbanBoardApp.Settings.ApiLocation + "/boards").success(function (response) {
                _this.scope.loading = false;
                _this.scope.boards = response.Items;
                _this.scope.loadBoard(_this.scope.boards[0]);
            });
            scope.$on('BoardCreated', function (event, args) {
                _this.scope.boards.push(args);
            });
            scope.$on('newColumnCreated', function (event, args) {
                _this.scope.columns.push(args);
            });
            scope.$on('newTaskCreated', function (event, args) {
                _this.scope.tasks.push(args);
            });
            scope.$on('TaskUpdated', function (event, args) {
                for (var i in _this.scope.tasks) {
                    if (_this.scope.tasks[i].Id === args.Id) {
                        _this.scope.tasks[i] = args;
                        break;
                    }
                }
            });
            scope.$on('ColumnUpdated', function (event, args) {
                for (var i in _this.scope.columns) {
                    if (_this.scope.columns[i].Id === args.Id) {
                        _this.scope.columns[i] = args;
                        break;
                    }
                }
            });
            scope.loadBoard = function (item) {
                _this.scope.currentBoard = item;
                _this.http.get(KanbanBoardApp.Settings.ApiLocation + "/boards/" + item.Slug + "/columns").success(function (response) {
                    _this.scope.columns = response.Items;
                });
                _this.http.get(KanbanBoardApp.Settings.ApiLocation + "/boards/" + item.Slug + "/tasks").success(function (response) {
                    _this.scope.tasks = response.Items;
                });
            };
            scope.createBoard = function () {
                modal.open({
                    animation: true,
                    templateUrl: 'AddBoardModal.html',
                    controller: 'AddBoardController',
                    scope: _this.scope
                });
            };
            scope.createTask = function (columnSlug) {
                modal.open({
                    animation: true,
                    templateUrl: 'AddTaskModal.html',
                    controller: 'AddTaskController',
                    scope: _this.scope,
                    resolve: {
                        currentBoard: function () {
                            return _this.scope.currentBoard;
                        },
                        columnSlug: function () {
                            return columnSlug;
                        }
                    }
                });
            };
            scope.deleteTask = function (task) {
                _this.http.delete(KanbanBoardApp.Settings.ApiLocation + "/boards/" + _this.scope.currentBoard.Slug + "/tasks/" + task.Id).success(function (response) {
                    var index = _this.scope.tasks.indexOf(task);
                    if (index > -1) {
                        _this.scope.tasks.splice(index, 1);
                    }
                });
            };
            scope.editTask = function (task) {
                var newTask = angular.copy(task);
                modal.open({
                    animation: true,
                    templateUrl: 'UpdateTaskModal.html',
                    controller: 'UpdateTaskController',
                    scope: _this.scope,
                    resolve: {
                        currentBoard: function () {
                            return _this.scope.currentBoard;
                        },
                        columns: function () {
                            return _this.scope.columns;
                        },
                        currentTask: function () {
                            return newTask;
                        }
                    }
                });
            };
            scope.updateTask = function (task) {
                _this.http.put(KanbanBoardApp.Settings.ApiLocation + "/boards/" + _this.scope.currentBoard.Slug + "/tasks/" + task.Id, task).success(function (response) {
                    // do something
                });
            };
            scope.createColumn = function () {
                modal.open({
                    animation: true,
                    templateUrl: 'AddColumnModal.html',
                    controller: 'AddColumnController',
                    scope: _this.scope,
                    resolve: {
                        currentBoard: function () {
                            return _this.scope.currentBoard;
                        }
                    }
                });
            };
            scope.editColumn = function (column) {
                var newColumn = angular.copy(column);
                modal.open({
                    animation: true,
                    templateUrl: 'UpdateColumnModal.html',
                    controller: 'UpdateColumnController',
                    scope: _this.scope,
                    resolve: {
                        currentBoard: function () {
                            return _this.scope.currentBoard;
                        },
                        currentColumn: function () {
                            return newColumn;
                        }
                    }
                });
            };
            scope.deleteColumn = function (column) {
                _this.http.delete(KanbanBoardApp.Settings.ApiLocation + "/boards/" + _this.scope.currentBoard.Slug + "/columns/" + column.Slug).success(function (response) {
                    var index = _this.scope.columns.indexOf(column);
                    if (index > -1) {
                        _this.scope.columns.splice(index, 1);
                    }
                });
            };
            scope.onDragComplete = function (data, event, columnSlug) {
                data.BoardColumnSlug = columnSlug;
                _this.http.put(KanbanBoardApp.Settings.ApiLocation + "/boards/" + _this.scope.currentBoard.Slug + "/tasks/" + data.Id, data).success(function (response) {
                    // do something
                });
            };
        }
        return BoardController;
    })();
    KanbanBoardApp.BoardController = BoardController;
})(KanbanBoardApp || (KanbanBoardApp = {}));
