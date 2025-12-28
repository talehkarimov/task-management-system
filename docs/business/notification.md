# Notification – Business Rules & Behavior

## Purpose
The Notification Service is responsible for informing users about important
events that occur in the system, such as task lifecycle changes.

Notifications are delivered asynchronously and do not block the core business flow.

---

## Triggering Events
Notifications are triggered by integration events published by other services,
primarily the Task Service.

Examples:
- Task created
- Task assigned
- Task status changed
- Task completed
- Task blocked
- Task comment added

---

## Notification Types
Each triggering event results in a specific notification type.

Examples:
- TaskCreated
- TaskAssigned
- TaskStatusChanged
- TaskCompleted
- TaskBlocked
- TaskCommentAdded

Notification types are represented using enums to avoid magic strings.

---

## Delivery Channels
The system supports multiple notification channels:

- In-App notifications (stored and visible in the UI)
- Email notifications (optional, based on user preferences)

Each channel is processed independently.

---

## User Preferences
Users can control how they receive notifications:

- Enable or disable email notifications
- Always receive in-app notifications

Preferences are evaluated at the time of notification dispatch.

---

## Reliability & Failure Handling
- Notifications are created even if delivery fails
- Each delivery attempt is tracked per channel
- Failures in one channel do not affect others
- Duplicate integration events do not create duplicate notifications

The system is designed to fail gracefully.

---

## Consistency Model
Notifications are eventually consistent.

A task action is considered successful even if notification delivery fails.
