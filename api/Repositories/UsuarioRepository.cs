using Microsoft.EntityFrameworkCore;
using RappyFastFood.API.Data;
using RappyFastFood.API.Models;

namespace RappyFastFood.API.Repositories;

public class UsuarioRepository
{
    private readonly AppDbContext _db;

    public UsuarioRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        var normalizado = email.Trim().ToLowerInvariant();
        return _db.Usuarios.FirstOrDefaultAsync(u => u.Email == normalizado);
    }

    public Task<bool> EmailExisteAsync(string email)
    {
        var normalizado = email.Trim().ToLowerInvariant();
        return _db.Usuarios.AnyAsync(u => u.Email == normalizado);
    }

    public Task<bool> CnpjExisteAsync(string cnpj)
    {
        return _db.EmpresasB2B.AnyAsync(e => e.CNPJ == cnpj);
    }

    public async Task AdicionarClienteAsync(ClienteB2C cliente)
    {
        _db.ClientesB2C.Add(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task AdicionarEmpresaAsync(EmpresaB2B empresa)
    {
        _db.EmpresasB2B.Add(empresa);
        await _db.SaveChangesAsync();
    }

    public Task<ClienteB2C?> BuscarClientePorUsuarioAsync(int idUsuario)
    {
        return _db.ClientesB2C.FirstOrDefaultAsync(c => c.IdUsuario == idUsuario);
    }

    public Task<EmpresaB2B?> BuscarEmpresaPorUsuarioAsync(int idUsuario)
    {
        return _db.EmpresasB2B.FirstOrDefaultAsync(e => e.IdUsuario == idUsuario);
    }
}
