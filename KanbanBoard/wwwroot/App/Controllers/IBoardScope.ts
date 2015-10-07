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
        editColumn(column:any);
        deleteColumn(column:any):void;
        createBoard: () => void;
    }
}