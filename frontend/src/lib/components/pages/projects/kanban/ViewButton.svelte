<script lang="ts">
    import Table2 from "@lucide/svelte/icons/table-2";
    import Settings from "@lucide/svelte/icons/settings";
    import SquareChartGantt from "@lucide/svelte/icons/square-chart-gantt";
    import SquareKanban from "@lucide/svelte/icons/square-kanban";

    import { Button } from "$lib/components/ui/button";
    import {
        DropdownMenuContent,
        DropdownMenu,
        DropdownMenuItem,
        DropdownMenuTrigger
    } from "$lib/components/ui/dropdown-menu";
    import {
        Popover,
        PopoverContent,
        PopoverTrigger
    } from "$lib/components/ui/popover";
    import { Tabs, TabsList, TabsTrigger } from "$lib/components/ui/tabs";

    import { ViewMode } from "./helpers";

    const { viewMode }: { viewMode: ViewMode } = $props();
</script>

<Popover>
    <PopoverTrigger>
        {#snippet child({ props })}
            <Button {...props} >
                <Settings />
                View
            </Button>
        {/snippet}
    </PopoverTrigger>
    <PopoverContent align="end" class="w-auto">
        <TabsList>
            <TabsTrigger value={ViewMode.Table}>
                <Table2 />
                Table
            </TabsTrigger>
            <TabsTrigger value={ViewMode.Board}>
                <SquareKanban />
                Board
            </TabsTrigger>
            <TabsTrigger value={ViewMode.Roadmap}>
                <SquareChartGantt />
                Roadmap
            </TabsTrigger>
        </TabsList>
        
        <!-- <DropdownMenuItem>
        </DropdownMenuItem>
        <DropdownMenuItem>
        </DropdownMenuItem>
        <DropdownMenuItem>
        </DropdownMenuItem> -->
    </PopoverContent>
</Popover>
