using Microsoft.EntityFrameworkCore;
using RappyFastFood.API.Models;

namespace RappyFastFood.API.Data;

public static class DbSeeder
{
    public static async Task PopularAsync(AppDbContext db)
    {
        if (!await db.Usuarios.AnyAsync(u => u.Email == "admin@rappy.com"))
        {
            var admin = new Usuario { Email = "admin@rappy.com", Nome = "Administrador", TipoUsuario = "admin" };
            admin.DefinirSenha("Admin@123");
            db.Usuarios.Add(admin);
        }

        if (!await db.Usuarios.AnyAsync(u => u.Email == "cliente@rappy.com"))
        {
            var usuario = new Usuario { Email = "cliente@rappy.com", Nome = "Joao Silva", TipoUsuario = "cliente" };
            usuario.DefinirSenha("Cliente@123");
            db.ClientesB2C.Add(new ClienteB2C
            {
                Usuario = usuario,
                CPF = "123.456.789-09",
                Endereco = "Rua das Flores, 100",
                Cidade = "Goiania",
                Estado = "GO"
            });
        }

        if (!await db.Usuarios.AnyAsync(u => u.Email == "empresa@rappy.com"))
        {
            var usuario = new Usuario { Email = "empresa@rappy.com", Nome = "Maria Oliveira", TipoUsuario = "empresa" };
            usuario.DefinirSenha("Empresa@123");
            db.EmpresasB2B.Add(new EmpresaB2B
            {
                Usuario = usuario,
                RazaoSocial = "Tech Solucoes LTDA",
                NomeFantasia = "Tech Solucoes",
                CNPJ = "12.345.678/0001-90",
                QuantidadeFuncionarios = 80,
                EnderecoEntrega = "Av. Goias, 500"
            });
        }

        await db.SaveChangesAsync();
    }
}
