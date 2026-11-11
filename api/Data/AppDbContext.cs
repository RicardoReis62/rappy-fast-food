using Microsoft.EntityFrameworkCore;
using RappyFastFood.API.Models;

namespace RappyFastFood.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<ClienteB2C> ClientesB2C => Set<ClienteB2C>();
    public DbSet<EmpresaB2B> EmpresasB2B => Set<EmpresaB2B>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<AlertaEstoque> AlertasEstoque => Set<AlertaEstoque>();

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<decimal>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        modelBuilder.Entity<Usuario>().Property(u => u.Email).HasField("_email");
        modelBuilder.Entity<Usuario>().Property(u => u.SenhaHash).UsePropertyAccessMode(PropertyAccessMode.Property);
        modelBuilder.Entity<ClienteB2C>().ToTable("ClientesB2C");
        modelBuilder.Entity<EmpresaB2B>().ToTable("EmpresasB2B");
        modelBuilder.Entity<Categoria>().ToTable("Categorias");
        modelBuilder.Entity<Produto>().ToTable("Produtos");
        modelBuilder.Entity<AlertaEstoque>().ToTable("AlertasEstoque");
        modelBuilder.Entity<Ingrediente>().ToTable("Ingredientes", t => t.HasTrigger("trg_Ingredientes_Alerta"));
        modelBuilder.Entity<Pedido>().ToTable("Pedidos", t => t.HasTrigger("trg_Pedidos_Cancelamento"));
        modelBuilder.Entity<ItemPedido>().ToTable("ItensPedido", t => t.HasTrigger("trg_ItensPedido_BaixaEstoque"));

        modelBuilder.Entity<ClienteB2C>().HasOne(c => c.Usuario).WithMany().HasForeignKey(c => c.IdUsuario);
        modelBuilder.Entity<EmpresaB2B>().HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.IdUsuario);
        modelBuilder.Entity<Produto>().HasOne(p => p.Categoria).WithMany().HasForeignKey(p => p.IdCategoria);
        modelBuilder.Entity<Pedido>().HasOne(p => p.ClienteB2C).WithMany().HasForeignKey(p => p.IdClienteB2C);
        modelBuilder.Entity<Pedido>().HasOne(p => p.EmpresaB2B).WithMany().HasForeignKey(p => p.IdEmpresaB2B);
        modelBuilder.Entity<Pedido>().HasMany(p => p.Itens).WithOne().HasForeignKey(i => i.IdPedido);
        modelBuilder.Entity<ItemPedido>().HasOne(i => i.Produto).WithMany().HasForeignKey(i => i.IdProduto);
    }
}
