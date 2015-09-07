/// <reference path="../scripts/typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular.d.ts" />


module KanbanBoardApp {

    export interface IBoardScope extends ng.IScope {
        loading: boolean;
        boards: any;
        columns: any;
        tasks: any;
        loadBoard(item: any): void;
        createTask(columnSlug: string): void;
        deleteTask(task: any): void;
        editTask(task: any): void;
        updateTask(task: any): void;
        onDragComplete($data: any, $event: any, columnSlug: string): void;
        currentBoard: any;

        columnForm: any;
        createColumn(): void;
        deleteColumn(column:any):void;
        createBoard: () => void;
    }

    export class BoardController {
        constructor(private scope: IBoardScope, private http: ng.IHttpService, private modal: angular.ui.bootstrap.IModalService) {
            this.scope.loading = true;

            http.get("/boards").success((response: any) => {
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

            scope.loadBoard = (item) => {
                this.scope.currentBoard = item;
                this.http.get("/boards/" + item.Slug + "/columns").success((response: any) => {
                    this.scope.columns = response.Items;
                });

                this.http.get("/boards/" + item.Slug + "/tasks").success((response: any) => {
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
                this.http.delete("/boards/" + this.scope.currentBoard.Slug + "/tasks/" + task.Id).success((response: any) => {
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
                this.http.put("/boards/" + this.scope.currentBoard.Slug + "/tasks/" + task.Id, task).success((response: any) => {
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

            scope.deleteColumn = (column: any) => {
                this.http.delete("/boards/" + this.scope.currentBoard.Slug + "/columns/" + column.Slug).success((response: any) => {
                    var index = this.scope.columns.indexOf(column);
                    if (index > -1) {
                        this.scope.columns.splice(index, 1);
                    }
                });
            }

            scope.onDragComplete = (data, event, columnSlug) => {
                data.BoardColumnSlug = columnSlug;
                this.http.put("/boards/" + this.scope.currentBoard.Slug + "/tasks/" + data.Id, data).success((response: any) => {
                    // do something
                });
            };
        }
    }

    export interface IModalScope extends ng.IScope {
        save: () => void;
        cancel: () => void;
        errorMessage: string;
    }

    export interface IUpdateTaskScope extends IModalScope {
        taskForm: any;
        currentTask: any;
        columns: any;
    }

    export class UpdateTaskController {
        constructor(private scope: IUpdateTaskScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any, private columns: any, private currentTask: any) {
            this.scope.currentTask = currentTask;
            this.scope.columns = columns;
            scope.save = () => {
                if (this.scope.taskForm.$valid) {
                    this.currentTask.Name = this.scope.taskForm.name.$viewValue;
                    this.currentTask.Description = this.scope.taskForm.description.$viewValue;
                    console.log(this.scope.taskForm.columnSlug);
                    this.currentTask.BoardColumnSlug = this.scope.taskForm.columnSlug.$viewValue;
                    this.http.put("/boards/" + this.currentBoard.Slug + "/tasks/" + this.currentTask.Id, this.currentTask).success((response: any) => {
                        this.scope.$emit('TaskUpdated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        scope.errorMessage = "Unknown error has occured";
                        this.scope.taskForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }

    export interface IAddTaskScope extends IModalScope {
        taskForm;
    }

    export class AddTaskController {
        constructor(private scope: IAddTaskScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any, private columnSlug: string) {
            scope.save = () => {
                if (this.scope.taskForm.$valid) {

                    var task = {
                        Name: this.scope.taskForm.name.$viewValue,
                        Description: this.scope.taskForm.description.$viewValue,
                        BoardColumnSlug: this.columnSlug
                    };
                    this.http.post("/boards/" + this.currentBoard.Slug + "/tasks", task).success((response: any) => {
                        this.scope.$emit('newTaskCreated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        scope.errorMessage = "Unknown error has occured";
                        this.scope.taskForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }

    export interface IAddColumnScope extends IModalScope {
        columnForm;
    }

    export class AddColumnController {
        constructor(private scope: IAddColumnScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any) {
            scope.save = () => {
                if (this.scope.columnForm.$valid) {
                    var column = { Name: this.scope.columnForm.name.$viewValue };
                    this.http.post("/boards/" + this.currentBoard.Slug + "/columns", column).success((response: any) => {
                        this.scope.$emit('newColumnCreated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        if (status === 409) {
                            scope.errorMessage = "Column with this name already exists";
                        } else {
                            scope.errorMessage = "Unknown error has occured";
                        }
                        this.scope.columnForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }

    export interface IAddBoardScope extends IModalScope {
        boardForm;
    }

    export class AddBoardController {
        constructor(private scope: IAddBoardScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance) {
            scope.save = () => {
                if (this.scope.boardForm.$valid) {

                    var board = {
                        Name: this.scope.boardForm.name.$viewValue
                    };
                    this.http.post("/boards", board).success((response: any) => {
                        this.scope.$emit('BoardCreated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        scope.errorMessage = "Unknown error has occured";
                        this.scope.boardForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }


    var app = angular.module('KanbanBoardApp', ['ngDraggable', 'ui.bootstrap']);
    app.controller("BoardController", ['$scope', '$http', '$modal', BoardController]);
    app.controller("AddBoardController", ['$scope', '$http', '$modalInstance', AddBoardController]);
    app.controller("AddColumnController", ['$scope', '$http', '$modalInstance', 'currentBoard', AddColumnController]);
    app.controller("AddTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columnSlug', AddTaskController]);
    app.controller("UpdateTaskController", ['$scope', '$http', '$modalInstance', 'currentBoard', 'columns', 'currentTask', UpdateTaskController]);
}