using Bassma.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bassma.Data
{
    public class BakeryDbContext : IdentityDbContext<IdentityUser>
    {
        public BakeryDbContext(DbContextOptions<BakeryDbContext> options)
            : base(options)
        { }
		public DbSet<User> Users { get; set; }
		public DbSet<Produit> Produits { get; set; }
		public DbSet<Commmentaire> Commentaires { get; set; }
		public DbSet<Commande> Commandes { get; set; }
		public DbSet<ProduitCommande> ProduitCommandes { get; set; }
		public DbSet<Paiement> Paiments { get; set; }
		public DbSet<Livraison> Livraisons { get; set; }
		public DbSet<Promotion> Promotions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Produit>(entity =>
			{
				entity.HasKey(p => p.Id); // Spécification explicite de la clé primaire
				entity.Property(p => p.Nom).IsRequired().HasMaxLength(100); // Nom obligatoire avec une limite de 100 caractères
				entity.Property(p => p.Description).HasMaxLength(500); // Description max 500 caractères
				entity.Property(p => p.Prix).HasPrecision(18, 2); // Précision pour le prix
			});

			modelBuilder.Entity<Commmentaire>(entity =>
			{
				entity.HasKey(c => c.Id);
				entity.Property(c => c.Description).IsRequired().HasMaxLength(500); // Description max 500 caractères
			});

			modelBuilder.Entity<Commande>(entity =>
			{
				entity.HasKey(c => c.Id);
				entity.Property(c => c.Adresse).IsRequired().HasMaxLength(500); // Adresse obligatoire avec une limite de 500 caractères
				entity.Property(c => c.Total).HasPrecision(18, 2); // Précision pour le total
			});

			modelBuilder.Entity<ProduitCommande>(entity =>
			{
				entity.HasKey(pc => pc.Id);
				entity.HasOne(pc => pc.Commande)
					  .WithMany(c => c.ProduitCommandes)
					  .HasForeignKey(pc => pc.CommandeId);
				entity.HasOne(pc => pc.Produit)
					  .WithMany(p => p.ProduitCommandes)
					  .HasForeignKey(pc => pc.ProduitId);
			});

			modelBuilder.Entity<Paiement>(entity =>
			{
				entity.HasKey(p => p.Id);
				entity.HasOne(p => p.Commande)
					  .WithOne(c => c.Paiement)
					  .HasForeignKey<Paiement>(p => p.CommandeId);
			});

			modelBuilder.Entity<Livraison>(entity =>
			{
				entity.HasKey(l => l.Id);
				entity.HasOne(l => l.User)
					  .WithMany(u => u.Livraisons)
					  .HasForeignKey(l => l.UserId);
				entity.HasOne(l => l.Commande)
					  .WithOne(c => c.Livraison)
					  .HasForeignKey<Livraison>(l => l.CommandeId);
			});

			modelBuilder.Entity<Promotion>(entity =>
			{
				entity.HasKey(p => p.Id);
				entity.HasOne(p => p.Produit)
					  .WithMany(pr => pr.Promotions)
					  .HasForeignKey(p => p.ProduitId);
			});

		}
}
}
