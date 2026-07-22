using SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Identidad;

public class IntentoAccesoRepository(SistemaHostalDbContext context) : Repository<IntentoAcceso>(context), IIntentoAccesoRepository;