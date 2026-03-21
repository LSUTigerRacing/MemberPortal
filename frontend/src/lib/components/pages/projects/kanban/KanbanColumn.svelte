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
    import {
        DropdownMenu,
        DropdownMenuContent,
        DropdownMenuGroup,
        DropdownMenuItem,
        DropdownMenuLabel,
        DropdownMenuSeparator,
        DropdownMenuTrigger
    } from "$lib/components/ui/dropdown-menu";

    import KanbanCard from "./KanbanCard.svelte";

    import type { TRAPI } from "../../../../../../../shared/typings/api";

    const { column }: { column: TRAPI.ProjectColumn } = $props();
</script>

<Card class="gap-2 px-2 py-0">
    <CardHeader class="flex items-center text-sm font-semibold ps-1 pe-0 pt-2">
        <CircleSmall color={column.color} fill={column.color} />
        {column.title}
        <div class="grow"></div>
        <DropdownMenu>
            <DropdownMenuTrigger>
                {#snippet child({ props })}
                    <Button {...props} size="icon" variant="ghost" class="h-8 w-8 p-0">
                        <Ellipsis />
                    </Button>
                {/snippet}
            </DropdownMenuTrigger>
            <DropdownMenuContent>
                <DropdownMenuLabel>List Actions</DropdownMenuLabel>
                <DropdownMenuGroup>
                    <DropdownMenuItem>Add card</DropdownMenuItem>
                    <DropdownMenuItem>Copy list</DropdownMenuItem>
                    <DropdownMenuItem>Move list</DropdownMenuItem>
                    <DropdownMenuItem>Move all cards in this list</DropdownMenuItem>
                    <DropdownMenuSeparator />
                    <DropdownMenuLabel>Change color</DropdownMenuLabel>
                    <DropdownMenuItem></DropdownMenuItem>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem>Archive this list</DropdownMenuItem>
                    <DropdownMenuItem>Archive all cards in this list</DropdownMenuItem>
                </DropdownMenuGroup>
            </DropdownMenuContent>
        </DropdownMenu>
    </CardHeader>
    <CardContent class="max-h-[50dvh] px-0.5 overflow-y-auto flex flex-col gap-2">
        {#each column.tasks as task (task.id)}
            <KanbanCard {task} />
            <KanbanCard {task} />
        {/each}
    </CardContent>
    <CardFooter class="p-0 ps-2 pb-2">
        <Button variant="ghost" class="w-full flex justify-start">
            <Plus />
            Add a card
        </Button>
    </CardFooter>
</Card>
