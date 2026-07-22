using SistemaHostal.Application.Notificaciones;
using SistemaHostal.Domain.Notificaciones;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Notificaciones;

public class NotificacionRepository(SistemaHostalDbContext context)
    : Repository<Notificacion>(context), INotificacionRepository;