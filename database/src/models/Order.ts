import { pgTable, primaryKey } from "drizzle-orm/pg-core";

import { orderStatus, subsystems } from "./_enums.ts";
import { User } from "./User.ts";

import { OrderStatus } from "@/common/config/enums.ts";

export const Order = pgTable("order", t => ({
    id: t.uuid().primaryKey().defaultRandom(),
    requesterId: t
        .uuid()
        .notNull()
        .references(() => User.id),

    name: t.text().notNull(),
    subsystem: subsystems().notNull(),
    status: orderStatus().notNull().default(OrderStatus.Pending),
    deadline: t.timestamp({ withTimezone: true }).notNull(),
    notes: t.text(),

    createdAt: t.timestamp({ withTimezone: true }).notNull().defaultNow(),
    updatedAt: t.timestamp({ withTimezone: true }).notNull().defaultNow()
}));

export const OrderItem = pgTable("order_item", t => ({
    id: t.uuid().primaryKey().defaultRandom(),
    orderId: t
        .uuid()
        .notNull()
        .references(() => Order.id, { onDelete: "cascade" }),

    name: t.text().notNull(),
    partNumber: t.text().notNull(),
    supplier: t.text().notNull(),
    url: t.text().notNull(),
    quantity: t.integer().notNull(),
    price: t.numeric().notNull()
}));

export const OrderReview = pgTable(
    "order_review",
    t => ({
        userId: t
            .uuid()
            .notNull()
            .references(() => User.id, { onDelete: "cascade" }),
        orderId: t
            .uuid()
            .notNull()
            .references(() => Order.id, { onDelete: "cascade" }),
        value: t.boolean().notNull(),
        createdAt: t.timestamp({ withTimezone: true }).notNull().defaultNow()
    }),
    t => [primaryKey({ columns: [t.userId, t.orderId] })]
);
