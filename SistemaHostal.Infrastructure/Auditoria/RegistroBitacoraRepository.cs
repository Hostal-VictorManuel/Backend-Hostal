using SistemaHostal.Application.Auditoria;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Auditoria;

public class RegistroBitacoraRepository(SistemaHostalDbContext context)
    : Repository<RegistroBitacora>(context), IRegistroBitacoraRepository;