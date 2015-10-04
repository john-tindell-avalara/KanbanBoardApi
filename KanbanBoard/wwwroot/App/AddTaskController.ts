module KanbanBoardApp {
    export class AddTaskController {
        constructor(private scope: IAddTaskScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any, private columnSlug: string) {
            scope.save = () => {
                if (this.scope.addTaskForm.$valid) {

                    var task = {
                        Name: this.scope.addTaskForm.name.$viewValue,
                        Description: this.scope.addTaskForm.description.$viewValue,
                        BoardColumnSlug: this.columnSlug
                    };
                    this.http.post("/boards/" + this.currentBoard.Slug + "/tasks", task).success((response: any) => {
                        this.scope.$emit('newTaskCreated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        scope.errorMessage = "Unknown error has occured";
                        this.scope.addTaskForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }
}