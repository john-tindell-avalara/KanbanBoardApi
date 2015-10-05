module KanbanBoardApp {
    export class UpdateColumnController {
        constructor(private scope: IUpdateColumnScope, private http: ng.IHttpService, private modalInstance: angular.ui.bootstrap.IModalServiceInstance, private currentBoard: any, private currentColumn: any) {
            scope.currentColumn = currentColumn;
            scope.save = () => {
                if (this.scope.updateColumnForm.$valid) {
                    currentColumn.Name = this.scope.updateColumnForm.name.$viewValue;
                    this.http.put("http://localhost:2943/boards/" + this.currentBoard.Slug + "/columns/" + currentColumn.Slug, currentColumn).success((response: any) => {
                        this.scope.$emit('ColumnUpdated', response);
                        modalInstance.dismiss(null);
                    }).error((error: any, status: number) => {
                        scope.errorMessage = "Unknown error has occured";
                        this.scope.updateColumnForm.name.$invalid = true;
                    });
                }
            }

            scope.cancel = () => {
                modalInstance.dismiss('cancel');
            };
        }
    }
}