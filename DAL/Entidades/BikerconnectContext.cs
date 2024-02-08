using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entidades;

public partial class BikerconnectContext : DbContext
{
    public BikerconnectContext()
    {
    }

    public BikerconnectContext(DbContextOptions<BikerconnectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Moto> Motos { get; set; }

    public virtual DbSet<Participante> Participantes { get; set; }

    public virtual DbSet<Quedada> Quedadas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=PostgresConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>(entity =>
        {
            entity.HasKey(e => e.IdMoto).HasName("motos_pkey");

            entity.ToTable("motos", "gestion_logicanegocio");

            entity.Property(e => e.IdMoto).HasColumnName("id_moto");
            entity.Property(e => e.Año).HasColumnName("año");
            entity.Property(e => e.Color)
                .HasMaxLength(20)
                .HasColumnName("color");
            entity.Property(e => e.DescModificaciones)
                .HasMaxLength(100)
                .HasColumnName("desc_modificaciones");
            entity.Property(e => e.IdUsuarioPropietario).HasColumnName("id_usuario_propietario");
            entity.Property(e => e.Marca)
                .HasMaxLength(20)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .HasColumnName("modelo");

            entity.HasOne(d => d.IdUsuarioPropietarioNavigation).WithMany(p => p.Motos)
                .HasForeignKey(d => d.IdUsuarioPropietario)
                .HasConstraintName("fkrj104ma5k7u5q0815pwuvvkyl");
        });

        modelBuilder.Entity<Participante>(entity =>
        {
            entity.HasKey(e => e.Id); 
            entity.ToTable("participantes", "gestion_logicanegocio");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd(); 
            entity.Property(e => e.IdQuedada).HasColumnName("id_quedada");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdQuedadaNavigation).WithMany()
                .HasForeignKey(d => d.IdQuedada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fks0eafcfbbgb26vqqajbldul8e");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkd68ea19e1man3k5vcyjl4d8dl");
        });

        modelBuilder.Entity<Quedada>(entity =>
        {
            entity.HasKey(e => e.IdQuedada).HasName("quedadas_pkey");

            entity.ToTable("quedadas", "gestion_logicanegocio");

            entity.Property(e => e.IdQuedada).HasColumnName("id_quedada");
            entity.Property(e => e.DescQuedada)
                .HasMaxLength(200)
                .HasColumnName("desc_quedada");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.FchHoraEncuentro)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_hora_encuentro");
            entity.Property(e => e.Lugar)
                .HasMaxLength(150)
                .HasColumnName("lugar");
            entity.Property(e => e.UsuarioOrganizador)
                .HasMaxLength(50)
                .HasColumnName("usuario_organizador");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios", "gestion_usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .HasColumnName("contraseña");
            entity.Property(e => e.CuentaConfirmada)
                .HasDefaultValue(false)
                .HasColumnName("cuenta_confirmada");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FchExpiracionToken)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_expiracion_token");
            entity.Property(e => e.FchRegistro)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_registro");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.NombreApellidos)
                .HasMaxLength(50)
                .HasColumnName("nombre_apellidos");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .HasColumnName("rol");
            entity.Property(e => e.TlfMovil)
                .HasMaxLength(9)
                .HasColumnName("tlf_movil");
            entity.Property(e => e.TokenRecuperacion)
                .HasMaxLength(100)
                .HasColumnName("token_recuperacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
