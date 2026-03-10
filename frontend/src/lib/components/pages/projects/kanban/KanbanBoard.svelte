<script lang="ts">
    import Plus from "@lucide/svelte/icons/plus";

    import { Button } from "$lib/components/ui/button";
    import {
        Tooltip,
        TooltipContent,
        TooltipTrigger
    } from "$lib/components/ui/tooltip";

    import Filter from "./Filter.svelte";
    import KanbanColumn from "./KanbanColumn.svelte";
    import ViewButton from "./ViewButton.svelte";

    import type { ProjectState } from "./helpers";

    let { filter = $bindable(), viewMode = $bindable(), columns }: Pick<ProjectState, "filter" | "viewMode" | "columns"> = $props();
</script>

<div class="flex gap-3 mb-4">
    <Filter bind:filter={filter} />
    <ViewButton bind:viewMode={viewMode} />
</div>
<div class="flex gap-4 items-center overflow-auto w-full h-full">
    {#each columns as column (column.id)}
        <KanbanColumn {column} />
    {/each}
    <Tooltip>
        <TooltipTrigger>
            {#snippet child({ props })}
                <Button {...props} class="bg-gray-400 hover:bg-gray-500 rounded-full ms-5" variant="ghost">
                    <Plus />
                </Button>
            {/snippet}
        </TooltipTrigger>
        <TooltipContent>
            <span>Create new column</span>
        </TooltipContent>
    </Tooltip>
</div>
