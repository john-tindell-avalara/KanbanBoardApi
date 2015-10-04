module KanbanBoardApp {
    export interface IModalScope extends ng.IScope {
        save: () => void;
        cancel: () => void;
        errorMessage: string;
    }
}