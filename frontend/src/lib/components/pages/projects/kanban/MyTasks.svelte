<script lang="ts">
    import Filter from "./Filter.svelte";
    import TaskTable from "./TaskTable.svelte";
    import ViewButton from "./ViewButton.svelte";
    import type { ProjectState } from "./helpers";

    import type { TRAPI } from "../../../../../../../shared/typings/api";

    let {
        filter = $bindable(),
        viewMode = $bindable(),
        tab,
        tasks
    }: Pick<ProjectState, "filter" | "viewMode" | "tab"> & { tasks: TRAPI.ProjectTask[] } = $props();
</script>

<div class="flex">
    <div></div>
    <div class="grow">
        <div class="flex gap-3 mb-4">
            <Filter bind:filter={filter} />
            <ViewButton bind:viewMode={viewMode} />
        </div>
        <TaskTable tab={tab} tasks={tasks} />
    </div>
</div>
