<script lang="ts">
    import { resolve } from "$app/paths";
    import { page } from "$app/state";

    import Ellipsis from "@lucide/svelte/icons/ellipsis";
    import Pencil from "@lucide/svelte/icons/pencil";
    import Save from "@lucide/svelte/icons/save";
    import Settings from "@lucide/svelte/icons/settings";

    import { Button } from "$lib/components/ui/button";
    import {
        DropdownMenuContent,
        DropdownMenu,
        DropdownMenuItem,
        DropdownMenuTrigger
    } from "$lib/components/ui/dropdown-menu";
    import { Input } from "$lib/components/ui/input";
    import {
        Tabs,
        TabsContent,
        TabsList,
        TabsTrigger
    } from "$lib/components/ui/tabs";

    import MyTasks from "$lib/components/pages/projects/kanban/MyTasks.svelte";
    import KanbanBoard from "$lib/components/pages/projects/kanban/KanbanBoard.svelte";
    import { ProjectTabs, ViewMode } from "$lib/components/pages/projects/kanban/helpers";

    import { ProjectTaskPriority } from "../../../../../shared/config/enums";

    let data = $state({
        title: "MP - General",
        tab: ProjectTabs.MyTasks,
        viewMode: ViewMode.Table,
        filter: "",
        tasks: [
            { id: "1", author: "DamienVesper", title: "Finish the Member Portal", priority: ProjectTaskPriority.Medium, completed: false, createdAt: "", updatedAt: "" }
        ]
    });

    let titleState = $state({
        editing: false,
        draftTitle: "MP - General"
    });
</script>
<div class="absolute w-full xl:h-screen bg-[#f3d2fa]">
    <div class="xl:mt-16.75 bg-body"></div>
    <div class="flex items-center gap-2 px-8 bg-body pt-4" class:pb-2={titleState.editing} class:pb-3={!titleState.editing}>
        {#if !titleState.editing}
            <h1 class="text-2xl">{data.title}</h1>
            <Button class="opacity-0 hover:opacity-100" variant="ghost" size="icon-sm" onclick={() => (titleState.editing = !titleState.editing)}>
                <Pencil />
            </Button>
        {:else}
            <Input class="w-auto" bind:value={titleState.draftTitle} />
            <Button size="icon-sm" onclick={() => (titleState.editing = !titleState.editing)}>
                <Save />
            </Button>
        {/if}
        <div class="grow"></div>
        <DropdownMenu>
            <DropdownMenuTrigger>
                {#snippet child({ props })}
                    <Button {...props} variant="ghost" size="icon-sm">
                        <Ellipsis />
                    </Button>
                {/snippet}
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
                <DropdownMenuItem>
                    <Settings />
                    <a href={resolve("/projects/[id]/settings", { id: page.params.id })}>Settings</a>
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    </div>
    <Tabs class="gap-0 bg-body" value={data.tab}>
        <div id="tabs-separator" class="w-full">
            <TabsList class="px-8 py-0 bg-transparent">
                {#each Object.values(ProjectTabs) as value, i (i)}
                    <TabsTrigger class="h-full rounded-none rounded-t-md shadow-none! border-0 border-t-2 border-x-2 data-[state=active]:border-primary data-[state=active]:shadow-none cursor-pointer" {value}>{value}</TabsTrigger>
                {/each}
            </TabsList>
        </div>
        <TabsContent class="bg-primary px-8" value={ProjectTabs.Overview}>
            <KanbanBoard />
        </TabsContent>
        <TabsContent class="bg-[#f3d2fa] px-4 py-4" value={ProjectTabs.MyTasks}>
            <MyTasks bind:data={data} />
        </TabsContent>
    </Tabs>
</div>

<style>
    #tabs-separator {
        box-shadow: inset -1px -2px 0 0 var(--primary);
    }
</style>
