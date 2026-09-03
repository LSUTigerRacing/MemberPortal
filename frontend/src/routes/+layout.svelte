<script lang="ts">
    import { onMount } from "svelte";

    import { page } from "$app/state";

    import TooltipProvider from "$lib/components/ui/tooltip/tooltip-provider.svelte";

    import Navbar from "$lib/components/layout/Navbar.svelte";

    import { authState } from "$lib/hooks/auth.svelte";

    import "../lib/css/index.css";

    const { children } = $props();

    onMount(() => {
        // Skip on /login — there's no session to fetch yet, and /api/auth/me
        // would just 401. Every other route loads it once so role-gated UI
        // (see routes/admin, UserDropdown.svelte) has something to check.
        if (page.url.pathname !== "/login") authState.load();
    });
</script>

{#if page.url.pathname !== "/login"}
    <Navbar />
{/if}
<TooltipProvider>
    {@render children()}
</TooltipProvider>
