import { defineRelations } from "drizzle-orm";

import { Order, OrderItem, OrderReview } from "./Order.ts";
import { Project, ProjectColumn, ProjectTask, ProjectTaskUser, ProjectUser } from "./Project.ts";
import { User } from "./User.ts";

export const relations = defineRelations(
    { Order, OrderItem, OrderReview, Project, ProjectColumn, ProjectTask, ProjectTaskUser, ProjectUser, User },
    r => ({
        Order: {
            items: r.many.OrderItem(),
            reviews: r.many.OrderReview(),
            requester: r.one.User({
                from: r.Order.requesterId,
                to: r.User.id
            })
        },
        OrderItem: {
            order: r.one.Order({
                from: r.OrderItem.orderId,
                to: r.Order.id
            })
        },
        OrderReview: {
            order: r.one.Order({
                from: r.OrderReview.orderId,
                to: r.Order.id
            }),
            user: r.one.User({
                from: r.OrderReview.userId,
                to: r.User.id
            })
        },
        Project: {
            author: r.one.User({
                from: r.Project.authorId,
                to: r.User.id
            }),
            columns: r.many.ProjectColumn(),
            tasks: r.many.ProjectTask(),
            members: r.many.ProjectUser()
        },
        ProjectColumn: {
            project: r.one.Project({
                from: r.ProjectColumn.projectId,
                to: r.Project.id
            }),
            tasks: r.many.ProjectTask()
        },
        ProjectTask: {
            assignees: r.many.ProjectTaskUser(),
            column: r.one.ProjectColumn({
                from: r.ProjectTask.columnId,
                to: r.ProjectColumn.id
            }),
            project: r.one.Project({
                from: r.ProjectTask.projectId,
                to: r.Project.id
            })
        },
        ProjectTaskUser: {
            task: r.one.ProjectTask({
                from: r.ProjectTaskUser.taskId,
                to: r.ProjectTask.id
            }),
            user: r.one.ProjectUser({
                from: r.ProjectTaskUser.userId,
                to: r.ProjectUser.userId
            })
        },
        ProjectUser: {
            project: r.one.Project({
                from: r.ProjectUser.projectId,
                to: r.Project.id
            }),
            user: r.one.User({
                from: r.ProjectUser.userId,
                to: r.User.id
            })
        },
        User: {
            orders: r.many.Order(),
            orderReviews: r.many.OrderReview(),
            projects: r.many.ProjectUser()
        }
    })
);
