<script lang="ts">
    import { page } from "$app/state";

    import TooltipProvider from "$lib/components/ui/tooltip/tooltip-provider.svelte";

    import Navbar from "$lib/components/layout/Navbar.svelte";

    import "../lib/css/index.css";

    const { children } = $props();
</script>

{#if page.url.pathname !== "/login"}
    <Navbar />
{/if}
<TooltipProvider>
    {@render children()}
</TooltipProvider>
