import type { API } from "$lib/modules/API";
import type { Unpacked } from "$lib/utils";

import { Subsystem, System } from "@/common/config/enums";

export enum ViewMode {
    Gallery,
    List
}

export enum SortOrder {
    Ascending,
    Descending
}

/**
 * All possible props passed to child components of admin page.
 */
export interface AdminProps {
    viewMode: ViewMode;
    sortOrder: SortOrder;
    filters: {
        systems: System[];
        subsystems: Subsystem[];
        years: number[];
        name: string;
    };
    users: Awaited<ReturnType<API["fetchUsers"]>>["data"];
    activeUser: Unpacked<AdminProps["users"]>["id"];
    filteredCount: number;
}
