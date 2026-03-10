<script lang="ts">
    import CircleSmall from "@lucide/svelte/icons/circle-small";
    import Ellipsis from "@lucide/svelte/icons/ellipsis";
    import Plus from "@lucide/svelte/icons/plus";

    import { Button } from "$lib/components/ui/button";
    import {
        Card,
        CardContent,
        CardHeader,
        CardFooter
    } from "$lib/components/ui/card";
    import { DropdownMenu, DropdownMenuContent, DropdownMenuTrigger } from "$lib/components/ui/dropdown-menu";

    import KanbanCard from "./KanbanCard.svelte";

    import type { TRAPI } from "../../../../../../../shared/typings/api";

    const { column }: { column: TRAPI.ProjectColumn } = $props();
</script>

<Card>
    <CardHeader class="flex items-center">
        <CircleSmall color={column.color} fill={column.color} />
        {column.title}
        <div class="grow"></div>
        <DropdownMenu>
            <DropdownMenuTrigger>
                {#snippet child({ props })}
                    <Button {...props} size="icon" variant="ghost" class="h-8 w-8 p-0 hover:bg-primary hover:text-white hover:border-primary">
                        <Ellipsis />
                    </Button>
                {/snippet}
            </DropdownMenuTrigger>
            <DropdownMenuContent></DropdownMenuContent>
        </DropdownMenu>
    </CardHeader>
    <CardContent>
        {#each column.tasks as task (task.id)}
            <KanbanCard {task} />
        {/each}
    </CardContent>
    <CardFooter>
        <Button variant="ghost" class="w-full mx-0.5 flex justify-start">
            <Plus />
            Add a card
        </Button>
    </CardFooter>
</Card>
