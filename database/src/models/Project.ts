import { pgTable, primaryKey } from "drizzle-orm/pg-core";
import { relations } from "drizzle-orm";

import { User } from "./User.js";

import {
    projectPriority,
    projectStatus,
    subsystems,
    projectTaskPriority
} from "./enums.js";

import { ProjectStatus, ProjectTaskPriority } from "../../../shared/config/enums.js";

export const Project = pgTable("project", t => ({
    id: t.serial().primaryKey(),
    authorId: t.uuid().notNull().references(() => User.id, { onDelete: "cascade" }),

    title: t.text().notNull().default("Untitled"),
    description: t.text(),
    subsystem: subsystems().notNull(),
    priority: projectPriority().notNull(),
    status: projectStatus().notNull().default(ProjectStatus.Draft),

    startDate: t.timestamp({ withTimezone: true }).notNull(),
    deadline: t.timestamp({ withTimezone: true }).notNull(),

    createdAt: t.timestamp({ withTimezone: true }).notNull().defaultNow(),
    updatedAt: t.timestamp({ withTimezone: true }).notNull().defaultNow()
}));

export const ProjectColumn = pgTable("project_column", t => ({
    id: t.uuid().primaryKey().defaultRandom(),
    projectId: t.serial().notNull().references(() => Project.id, { onDelete: "cascade" }),
    title: t.text().notNull().default("Untitled"),
    color: t.integer().notNull().default(0)
}));

export const ProjectTask = pgTable("project_task", t => ({
    id: t.uuid().primaryKey().defaultRandom(),
    projectId: t.serial().notNull().references(() => Project.id, { onDelete: "cascade" }),
    columnId: t.uuid().notNull().references(() => ProjectColumn.id, { onDelete: "cascade" }),
    authorId: t.uuid().notNull().references(() => User.id, { onDelete: "cascade" }),

    title: t.text().notNull().default("Untitled"),
    description: t.text(),
    priority: projectTaskPriority().notNull().default(ProjectTaskPriority.Medium),
    completed: t.boolean().notNull().default(false),
    deadline: t.timestamp({ withTimezone: true }),

    createdAt: t.timestamp({ withTimezone: true }).notNull().defaultNow(),
    updatedAt: t.timestamp({ withTimezone: true }).notNull().defaultNow()
}));

export const ProjectUser = pgTable("project_user", t => ({
    projectId: t.serial().notNull().references(() => Project.id, { onDelete: "cascade" }),
    userId: t.uuid().notNull().references(() => User.id, { onDelete: "cascade" })
}), t => [
    primaryKey({ columns: [t.projectId, t.userId] })
]);

export const ProjectTaskUser = pgTable("project_task_user", t => ({
    taskId: t.uuid().notNull().references(() => ProjectTask.id, { onDelete: "cascade" }),
    userId: t.uuid().notNull().references(() => User.id, { onDelete: "cascade" })
}));

export const ProjectRelations = relations(Project, ({ one, many }) => ({
    author: one(User, {
        fields: [Project.authorId],
        references: [User.id]
    }),
    columns: many(ProjectColumn),
    tasks: many(ProjectTask),
    members: many(ProjectUser)
}));

export const ProjectTaskRelations = relations(ProjectTask, ({ one, many }) => ({
    column: one(ProjectColumn, {
        fields: [ProjectTask.columnId],
        references: [ProjectColumn.id]
    }),
    assignees: many(ProjectTaskUser)
}));

export const ProjectUserRelations = relations(ProjectUser, ({ one }) => ({
    project: one(Project, {
        fields: [ProjectUser.projectId],
        references: [Project.id]
    }),
    user: one(User, {
        fields: [ProjectUser.userId],
        references: [User.id]
    })
}));

export const ProjectTaskUserRelations = relations(ProjectTaskUser, ({ one }) => ({
    task: one(ProjectTask, {
        fields: [ProjectTaskUser.taskId],
        references: [ProjectTask.id]
    }),
    user: one(User, {
        fields: [ProjectTaskUser.userId],
        references: [User.id]
    })
}));