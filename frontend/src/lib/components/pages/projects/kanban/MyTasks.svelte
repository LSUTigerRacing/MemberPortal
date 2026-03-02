<script lang="ts">
    import {
        Table,
        TableBody,
        TableCell,
        TableHead,
        TableHeader,
        TableRow
    } from "$lib/components/ui/table";

    import Filter from "./Filter.svelte";
    import ViewButton from "./ViewButton.svelte";

    const { data } = $props();
</script>

<div class="flex">
    <div></div>
    <div class="grow">
        <div class="flex gap-3">
            <Filter {data} />
            <ViewButton />
        </div>
    </div>
</div>
