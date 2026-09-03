/**
 * Numeric hierarchy backing "and above" (AA) role checks. Higher number =
 * more access. This is the frontend/shared mirror of the backend's
 * Authorization/RoleLevels.cs — keep the two in sync if the ladder changes.
 *
 * NOTE: this file is UX convenience only. The frontend has no server-side
 * rendering (`ssr = false`, no hooks.server.ts) and cannot enforce anything
 * — the real security boundary is the ASP.NET [Authorize] policies backed
 * by RoleLevels.cs. Never treat a passing check here as a security decision.
 */

import { Role } from "./enums.js";

export const ROLE_LEVELS: Record<Role, number> = {
    [Role.Superadmin]: 99,
    [Role.Admin]: 98,
    [Role.SystemLead]: 97,
    [Role.SubsystemLead]: 95,
    [Role.Member]: 5,
    [Role.Unverified]: 0
};

export function roleLevel (role: Role): number {
    return ROLE_LEVELS[role] ?? 0;
}

export function isAtLeast (actual: Role, minimum: Role): boolean {
    return roleLevel(actual) >= roleLevel(minimum);
}
