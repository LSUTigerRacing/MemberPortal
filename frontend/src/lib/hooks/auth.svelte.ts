import { api } from "$lib/modules/API";

import { Role } from "../../../../shared/config/enums";
import { isAtLeast as roleIsAtLeast } from "../../../../shared/config/permissions";

/**
 * Reactive holder for the current user's role/Finance permissions, loaded
 * once from GET /api/auth/me.
 *
 * IMPORTANT: this is UX convenience only, not a security boundary. The app
 * has no server-side rendering (`ssr = false`, no hooks.server.ts) so this
 * state is entirely client-side and can be forged by anyone with devtools.
 * The real enforcement is the ASP.NET [Authorize] policies in the backend —
 * every check here mirrors one already made there. Use this only to hide/
 * show UI, never to gate anything that matters on its own.
 */
class AuthState {
    role = $state<Role>(Role.Unverified);
    isFinance = $state(false);
    loaded = $state(false);

    async load (): Promise<void> {
        try {
            const { data } = await api.fetchMe();
            this.role = data.role;
            this.isFinance = data.isFinance;
        } catch {
            // Not logged in / session expired — leave at Unverified defaults.
            this.role = Role.Unverified;
            this.isFinance = false;
        } finally {
            this.loaded = true;
        }
    }

    isAtLeast (minimum: Role): boolean {
        return roleIsAtLeast(this.role, minimum);
    }

    /** Financial dashboard "purchasing": Finance flag or Admin AA. */
    canAccessFinance (): boolean {
        return this.isFinance || this.isAtLeast(Role.Admin);
    }

    /** Order approve/reject: Finance flag (SuperAdmin bypasses everything). */
    canApproveOrder (): boolean {
        return this.isFinance || this.role === Role.Superadmin;
    }

    /** Delete/Archive project: SubsystemLead AA, or the project's own creator. */
    canDeleteProject (projectAuthorId: string, currentUserId: string): boolean {
        return this.isAtLeast(Role.SubsystemLead) || projectAuthorId === currentUserId;
    }
}

export const authState = new AuthState();
