<script lang="ts">
    import {
        Command,
        CommandEmpty,
        CommandGroup,
        CommandInput,
        CommandItem,
        CommandSeparator
    } from "$lib/components/ui/command";

    let { data = $bindable() } = $props();
</script>

<Command>
    <CommandInput
        placeholder="Filter by keyword or field"
        bind:value={data.filter}
    />
    <CommandEmpty class="hidden peer-first:focus:block">No results found.</CommandEmpty>
</Command>
