<script lang="ts">
    import KanbanColumn from "./KanbanColumn.svelte";

    import type { TRAPI } from "../../../../../../../shared/typings/api";

    const { columns }: { columns: TRAPI.ProjectColumn[] } = $props();
</script>

<div class="grid xl:grid-cols-5 pt-4">
    {#each columns as column, i (i)}
        <KanbanColumn {column} />
    {/each}
</div>
