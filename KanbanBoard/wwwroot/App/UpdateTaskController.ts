module KanbanBoardApp {
    export class UpdateTaskController {
        constructor(private scope: IUpdateTaskScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any, private columns: any, private currentTask: any) {
            this.scope.currentTask = currentTask;
            this.scope.columns = columns;
            scope.save = () => {
                if (this.scope.updateTaskForm.$valid) {
                    this.currentTask.Name = this.scope.updateTaskForm.name.$viewValue;
                    this.currentTask.Description = this.scope.updateTaskForm.description.$viewValue;
                    console.log(this.scope.updateTaskForm.columnSlug);
                    this.currentTask.BoardColumnSlug = this.scope.updateTaskForm.columnSlug.$viewValue;
                    this.http.put("/boards/" + this.currentBoard.Slug + "/tasks/" + this.currentTask.Id, this.currentTask).success((response: any) => {
                        this.scope.$emit('TaskUpdated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        scope.errorMessage = "Unknown error has occured";
                        this.scope.updateTaskForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }
}