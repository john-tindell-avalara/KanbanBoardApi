module KanbanBoardApp {
    export interface IUpdateTaskScope extends IModalScope {
        updateTaskForm: any;
        currentTask: any;
        columns: any;
    }
}