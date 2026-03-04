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

    const { tab, tasks }: { tab: ProjectTabs, tasks: TRAPI.ProjectTask[] } = $props();
</script>

<Table>
    <TableHeader>
        <TableRow>
            <TableHead>#</TableHead>
            <TableHead>Title</TableHead>
            {#if tab !== ProjectTabs.MyTasks}
                <TableHead>Assignees</TableHead>
            {/if}
            <TableHead>Priority</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Due Date</TableHead>
        </TableRow>
    </TableHeader>
    <TableBody>
        {#each tasks as task, i (task.id)}
            <TableRow>
                <TableCell>{i + 1}</TableCell>
                <TableCell>{task.title}</TableCell>
                <TableCell>
                    {#each task.assignees as user, i (i)}
                        <Avatar class="size-8">
                            <AvatarImage src={user.avatar} alt={user.name} />
                            <AvatarFallback class="text-xs font-semibold">{user.name.split(" ").map(x => x.substring(0, 1)).join("")}</AvatarFallback>
                        </Avatar>
                    {/each}
                </TableCell>
                <TableCell></TableCell>
                <TableCell>
                    {#if task.completed}
                        <Badge class="bg-green-500 text-white">Active</Badge>
                    {:else}
                        <Badge class="bg-purple-500 text-white">Completed</Badge>
                    {/if}
                </TableCell>
                <TableCell>
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
