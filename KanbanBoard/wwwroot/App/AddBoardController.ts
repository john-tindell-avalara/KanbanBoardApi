module KanbanBoardApp {
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
}