<script lang="ts">
    import { Avatar, AvatarImage, AvatarFallback } from "$lib/components/ui/avatar";
    import { Badge } from "$lib/components/ui/badge";
    import {
        Table,
        TableBody,
        TableCell,
        TableHead,
        TableHeader,
        TableRow
    } from "$lib/components/ui/table";

    import { ProjectTabs } from "./helpers";

    import type { TRAPI } from "../../../../../../../shared/typings/api";
    import { ProjectTaskPriority } from "../../../../../../../shared/config/enums";

    const { tab, tasks }: { tab: ProjectTabs, tasks: TRAPI.ProjectTask[] } = $props();

    function priorityBadgeColor (priority: ProjectTaskPriority): string {
        switch (priority) {
            case ProjectTaskPriority.Low:
                return "bg-green-500";
            case ProjectTaskPriority.Medium:
                return "bg-amber-500";
            case ProjectTaskPriority.High:
                return "bg-red-500";
            default:
                return "bg-gray-500";
        }
    }
</script>

<div class="">
    <Table>
        <TableHeader>
            <TableRow class="bg-background">
                <TableHead class="rounded-tl-xl ps-6">#</TableHead>
                <TableHead>Title</TableHead>
                {#if tab !== ProjectTabs.MyTasks}
                    <TableHead>Assignees</TableHead>
                {/if}
                <TableHead>Priority</TableHead>
                <TableHead>Status</TableHead>
                <TableHead class="rounded-tr-xl pe-6">Due Date</TableHead>
            </TableRow>
        </TableHeader>
        <TableBody class="bg-background">
            {#each tasks as task, i (task.id)}
                <TableRow class="cursor-pointer">
                    <TableCell class="ps-6">{i + 1}</TableCell>
                    <TableCell>{task.title}</TableCell>
                    {#if tab !== ProjectTabs.MyTasks}
                        <TableCell>
                            {#each task.assignees as user, i (i)}
                                <Avatar class="size-8">
                                    <AvatarImage src={user.avatar} alt={user.name} />
                                    <AvatarFallback class="text-xs font-semibold">{user.name.split(" ").map(x => x.substring(0, 1)).join("")}</AvatarFallback>
                                </Avatar>
                            {/each}
                        </TableCell>
                    {/if}
                    <TableCell>
                        <Badge class="text-white {priorityBadgeColor(task.priority)}">{task.priority}</Badge>
                    </TableCell>
                    <TableCell>
                        {#if task.completed}
                            <Badge class="bg-purple-500 text-white">Completed</Badge>
                        {:else}
                            <Badge class="bg-green-500 text-white">Active</Badge>
                        {/if}
                    </TableCell>
                    <TableCell class="pe-6">
                        {#if task.deadline}
                            {new Date(task.deadline).toLocaleString()}
                        {:else}
                            N/A
                        {/if}
                    </TableCell>
                </TableRow>
            {/each}
        </TableBody>
    </Table>
</div>
