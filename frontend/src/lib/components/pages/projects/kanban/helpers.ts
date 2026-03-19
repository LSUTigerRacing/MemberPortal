import type { TRAPI } from "../../../../../../../shared/typings/api";

export interface ProjectState {
    title: string
    tab: ProjectTabs
    viewMode: ViewMode
    filter: string
    columns: TRAPI.ProjectColumn[]
}

export enum ProjectTabs {
    Overview = "Overview",
    MyTasks = "My Tasks"
}

export enum ViewMode {
    Table = "Table",
    Board = "Board",
    Roadmap = "Roadmap"
}
