<script lang="ts">
    import { page } from "$app/state";

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
</script>
<div class="xl:mt-16.75 px-8 pt-4">
    <h1 class="text-2xl mb-2">{data.title}</h1>
    <Tabs class="gap-4" value={data.tab}>
        <TabsList class="bg-background justify-start rounded-none border-b p-0">
            {#each Object.values(ProjectTabs) as value, i (i)}
                <TabsTrigger class="bg-background border-b-border dark:data-[state=active]:bg-background data-[state=active]:border-border data-[state=active]:border-b-background h-full rounded-none rounded-t border border-transparent data-[state=active]:-mb-0.5 data-[state=active]:shadow-none! dark:border-b-0 dark:data-[state=active]:-mb-0.5" {value}>{value}</TabsTrigger>
            {/each}
        </TabsList>
        <TabsContent value={ProjectTabs.Overview}>
            <KanbanBoard />
        </TabsContent>
        <TabsContent value={ProjectTabs.MyTasks}>
            <MyTasks bind:data={data} />
        </TabsContent>
    </Tabs>
</div>
