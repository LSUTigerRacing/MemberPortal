<script lang="ts">
    import {
        Command,
        CommandEmpty,
        CommandGroup,
        CommandInput,
        CommandItem,
        CommandSeparator
    } from "$lib/components/ui/command";

    import type { ProjectState } from "./helpers";

    let { filter = $bindable() }: { filter: ProjectState["filter"] } = $props();
</script>

<Command>
    <CommandInput
        placeholder="Filter by keyword or field"
        bind:value={filter}
        disabled
    />
    <CommandEmpty class="hidden peer-first:focus:block">No results found.</CommandEmpty>
</Command>
