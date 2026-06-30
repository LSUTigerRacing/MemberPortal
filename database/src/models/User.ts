import { pgTable } from "drizzle-orm/pg-core";

import { roles, shirtSizes, subsystems, systems } from "./_enums.ts";

import { Role } from "@/common/config/enums.ts";

export const User = pgTable("user", t => ({
    id: t.uuid().primaryKey().defaultRandom(),
    name: t.text().notNull(),
    email: t.text().unique().notNull(),
    role: roles().notNull().default(Role.Member),

    /**
     * This is the student's 89 number.
     */
    sid: t.integer().notNull(),
    system: systems().notNull(),
    subsystem: subsystems().notNull(),

    shirtSize: shirtSizes(),
    hazingStatus: t.boolean().notNull().default(false),
    feeStatus: t.boolean().notNull().default(false),
    gradYear: t.integer().notNull(),

    createdAt: t.timestamp({ withTimezone: true }).notNull().defaultNow(),
    updatedAt: t.timestamp({ withTimezone: true }).notNull().defaultNow()
}));
