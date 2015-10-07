module KanbanBoardApp {
    export class AddColumnController {
        constructor(private scope: IAddColumnScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any) {
            scope.save = () => {
                if (this.scope.columnForm.$valid) {
                    var column = { Name: this.scope.columnForm.name.$viewValue };
                    this.http.post(KanbanBoardApp.Settings.ApiLocation +  "/boards/" + this.currentBoard.Slug + "/columns", column).success((response: any) => {
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
}