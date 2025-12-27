using NotificationService.Domain.Enums;

namespace NotificationService.Application.Dtos;

public sealed record NotificationDto(
    Guid Id,
    NotificationType Type,
    string Payload,
    DateTime CreatedAt,
    NotificationStatus? NotificationStatus);
