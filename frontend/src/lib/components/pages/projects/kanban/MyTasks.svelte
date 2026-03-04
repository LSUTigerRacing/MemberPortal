<script lang="ts">
    import Filter from "./Filter.svelte";
    import TaskTable from "./TaskTable.svelte";
    import ViewButton from "./ViewButton.svelte";

    let { data = $bindable() } = $props();
</script>

<div class="flex">
    <div></div>
    <div class="grow">
        <div class="flex gap-3">
            <Filter bind:data={data} />
            <ViewButton viewMode={data.viewMode} />
        </div>
        <TaskTable tab={data.tab} tasks={data.tasks} />
    </div>
</div>
