module KanbanBoardApp {
    export class BoardController {
        constructor(private scope: IBoardScope, private http: ng.IHttpService, private modal: angular.ui.bootstrap.IModalService) {
            this.scope.loading = true;

            http.get(KanbanBoardApp.Settings.ApiLocation +  "/boards").success((response: any) => {
                this.scope.loading = false;
                this.scope.boards = response.Items;

                this.scope.loadBoard(this.scope.boards[0]);
            });

            scope.$on('BoardCreated', (event, args) => {
                this.scope.boards.push(args);
            });

            scope.$on('newColumnCreated', (event, args) => {
                this.scope.columns.push(args);
            });

            scope.$on('newTaskCreated', (event, args) => {
                this.scope.tasks.push(args);
            });

            scope.$on('TaskUpdated', (event, args) => {
                for (var i in this.scope.tasks) {
                    if (this.scope.tasks[i].Id === args.Id) {
                        this.scope.tasks[i] = args;
                        break;
                    }
                }
            });

            scope.$on('ColumnUpdated', (event, args) => {
                for (var i in this.scope.columns) {
                    if (this.scope.columns[i].Id === args.Id) {
                        this.scope.columns[i] = args;
                        break;
                    }
                }
            });

            scope.loadBoard = (item) => {
                this.scope.currentBoard = item;
                this.http.get(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + item.Slug + "/columns").success((response: any) => {
                    this.scope.columns = response.Items;
                });

                this.http.get(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + item.Slug + "/tasks").success((response: any) => {
                    this.scope.tasks = response.Items;
                });
            };

            scope.createBoard = () => {
                modal.open({
                    animation: true,
                    templateUrl: 'AddBoardModal.html',
                    controller: 'AddBoardController',
                    scope: this.scope
                });
            };

            scope.createTask = (columnSlug: string) => {
                modal.open({
                    animation: true,
                    templateUrl: 'AddTaskModal.html',
                    controller: 'AddTaskController',
                    scope: this.scope,
                    resolve: {
                        currentBoard: () => {
                            return this.scope.currentBoard;
                        },
                        columnSlug: () => {
                            return columnSlug;
                        }
                    }
                });
            };

            scope.deleteTask = (task: any) => {
                this.http.delete(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + this.scope.currentBoard.Slug + "/tasks/" + task.Id).success((response: any) => {
                    var index = this.scope.tasks.indexOf(task);
                    if (index > -1) {
                        this.scope.tasks.splice(index, 1);
                    }
                });
            }

            scope.editTask = (task: any) => {
                var newTask = angular.copy(task);
                modal.open({
                    animation: true,
                    templateUrl: 'UpdateTaskModal.html',
                    controller: 'UpdateTaskController',
                    scope: this.scope,
                    resolve: {
                        currentBoard: () => {
                            return this.scope.currentBoard;
                        },
                        columns: () => {
                            return this.scope.columns;
                        },
                        currentTask: () => {
                            return newTask;
                        }
                    }
                });
            };

            scope.updateTask = (task: any) => {
                this.http.put(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + this.scope.currentBoard.Slug + "/tasks/" + task.Id, task).success((response: any) => {
                    // do something
                });
            };

            scope.createColumn = () => {
                modal.open({
                    animation: true,
                    templateUrl: 'AddColumnModal.html',
                    controller: 'AddColumnController',
                    scope: this.scope,
                    resolve: {
                        currentBoard: () => {
                            return this.scope.currentBoard;
                        }
                    }
                });
            };

            scope.editColumn = (column: any) => {
                var newColumn = angular.copy(column);
                modal.open({
                    animation: true,
                    templateUrl: 'UpdateColumnModal.html',
                    controller: 'UpdateColumnController',
                    scope: this.scope,
                    resolve: {
                        currentBoard: () => {
                            return this.scope.currentBoard;
                        },
                        currentColumn: () => {
                            return newColumn;
                        }
                    }
                });
            };

            scope.deleteColumn = (column: any) => {
                this.http.delete(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + this.scope.currentBoard.Slug + "/columns/" + column.Slug).success((response: any) => {
                    var index = this.scope.columns.indexOf(column);
                    if (index > -1) {
                        this.scope.columns.splice(index, 1);
                    }
                });
            }

            scope.onDragComplete = (data, event, columnSlug) => {
                data.BoardColumnSlug = columnSlug;
                this.http.put(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + this.scope.currentBoard.Slug + "/tasks/" + data.Id, data).success((response: any) => {
                    // do something
                });
            };
        }
    }
}