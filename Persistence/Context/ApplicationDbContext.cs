using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TypeDocument> TypeDocuments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Account__DA18132CE9DA5D83");

            entity.ToTable("Account");

            entity.Property(e => e.IdAccount).HasColumnName("idAccount");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .HasColumnName("password");

            entity.HasOne(d => d.IdPersonNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdPerson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Person");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.IdPerson).HasName("PK__Person__BAB33700CA3CCE43");

            entity.ToTable("Person");

            entity.Property(e => e.IdPerson).HasColumnName("idPerson");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.IdTypeDocument).HasColumnName("idTypeDocument");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.NumberIdentityDocument)
                .HasMaxLength(20)
                .HasColumnName("numberIdentityDocument");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(9)
                .HasColumnName("phoneNumber");

            entity.HasOne(d => d.IdTypeDocumentNavigation).WithMany(p => p.People)
                .HasForeignKey(d => d.IdTypeDocument)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Person_IdentityDocument");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role__E5045C5426B86C02");

            entity.ToTable("Role");

            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TypeDocument>(entity =>
        {
            entity.HasKey(e => e.IdTypeDocument).HasName("PK__TypeDocu__CF96EB7B9C6C6E17");

            entity.ToTable("TypeDocument");

            entity.Property(e => e.IdTypeDocument).HasColumnName("idTypeDocument");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
