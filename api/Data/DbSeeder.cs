using Microsoft.EntityFrameworkCore;
using RappyFastFood.API.Models;

namespace RappyFastFood.API.Data;

public static class DbSeeder
{
    public static async Task PopularAsync(AppDbContext db, IConfiguration configuracao)
    {
        // Desativa contas de demonstração antigas que utilizavam senhas públicas no código.
        // Os registros e relacionamentos são preservados; apenas o acesso dessas contas é bloqueado.
        var emailsLegados = new[]
        {
            "admin@rappy.com",
            "cliente@rappy.com",
            "empresa@rappy.com"
        };

        var contasLegadas = await db.Usuarios
            .Where(u => emailsLegados.Contains(u.Email))
            .ToListAsync();

        foreach (var conta in contasLegadas)
        {
            conta.Ativo = false;
        }

        // A conta administrativa é opcional e só é criada/atualizada quando
        // credenciais locais forem fornecidas no arquivo .env.
        var emailAdmin = configuracao["DEMO_ADMIN_EMAIL"]?.Trim();
        var senhaAdmin = configuracao["DEMO_ADMIN_PASSWORD"];

        if (!string.IsNullOrWhiteSpace(emailAdmin) || !string.IsNullOrWhiteSpace(senhaAdmin))
        {
            if (string.IsNullOrWhiteSpace(emailAdmin) || !emailAdmin.Contains('@'))
                throw new InvalidOperationException("Configure um DEMO_ADMIN_EMAIL válido no arquivo .env.");

            if (string.IsNullOrWhiteSpace(senhaAdmin) || senhaAdmin.Length < 6)
                throw new InvalidOperationException("DEMO_ADMIN_PASSWORD deve ter pelo menos 6 caracteres.");

            var admin = await db.Usuarios.SingleOrDefaultAsync(u => u.Email == emailAdmin);
            if (admin == null)
            {
                admin = new Usuario
                {
                    Email = emailAdmin,
                    Nome = "Administrador",
                    TipoUsuario = "admin"
                };
                db.Usuarios.Add(admin);
            }
            else
            {
                admin.Email = emailAdmin;
                admin.Nome = "Administrador";
                admin.TipoUsuario = "admin";
            }

            admin.Ativo = true;
            admin.DefinirSenha(senhaAdmin);
        }

        await db.SaveChangesAsync();
    }
}
