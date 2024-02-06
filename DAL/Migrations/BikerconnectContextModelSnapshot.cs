﻿// <auto-generated />
using System;
using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(BikerconnectContext))]
    partial class BikerconnectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entidades.Moto", b =>
                {
                    b.Property<long>("IdMoto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_moto");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdMoto"));

                    b.Property<int>("Año")
                        .HasColumnType("integer")
                        .HasColumnName("año");

                    b.Property<string>("Color")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("color");

                    b.Property<string>("DescModificaciones")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("desc_modificaciones");

                    b.Property<long?>("IdUsuarioPropietario")
                        .HasColumnType("bigint")
                        .HasColumnName("id_usuario_propietario");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("marca");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("modelo");

                    b.HasKey("IdMoto")
                        .HasName("motos_pkey");

                    b.HasIndex("IdUsuarioPropietario");

                    b.ToTable("motos", "gestion_logicanegocio");
                });

            modelBuilder.Entity("DAL.Entidades.Participante", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IdQuedada")
                        .HasColumnType("bigint")
                        .HasColumnName("id_quedada");

                    b.Property<long>("IdUsuario")
                        .HasColumnType("bigint")
                        .HasColumnName("id_usuario");

                    b.HasKey("Id");

                    b.HasIndex("IdQuedada");

                    b.HasIndex("IdUsuario");

                    b.ToTable("participantes", "gestion_logicanegocio");
                });

            modelBuilder.Entity("DAL.Entidades.Quedada", b =>
                {
                    b.Property<long>("IdQuedada")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_quedada");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdQuedada"));

                    b.Property<string>("DescQuedada")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("desc_quedada");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("estado");

                    b.Property<DateTime>("FchHoraEncuentro")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("fch_hora_encuentro");

                    b.Property<string>("Lugar")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("lugar");

                    b.Property<string>("UsuarioOrganizador")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("usuario_organizador");

                    b.HasKey("IdQuedada")
                        .HasName("quedadas_pkey");

                    b.ToTable("quedadas", "gestion_logicanegocio");
                });

            modelBuilder.Entity("DAL.Entidades.Usuario", b =>
                {
                    b.Property<long>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id_usuario");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("IdUsuario"));

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("contraseña");

                    b.Property<bool>("CuentaConfirmada")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("cuenta_confirmada");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("FchExpiracionToken")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("fch_expiracion_token");

                    b.Property<DateTime?>("FchRegistro")
                        .HasColumnType("timestamp(6) without time zone")
                        .HasColumnName("fch_registro");

                    b.Property<string>("Foto")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("foto");

                    b.Property<string>("NombreApellidos")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("nombre_apellidos");

                    b.Property<string>("Rol")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("rol");

                    b.Property<string>("TlfMovil")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character varying(9)")
                        .HasColumnName("tlf_movil");

                    b.Property<string>("TokenRecuperacion")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("token_recuperacion");

                    b.HasKey("IdUsuario")
                        .HasName("usuarios_pkey");

                    b.HasIndex(new[] { "Email" }, "usuarios_email_key")
                        .IsUnique();

                    b.ToTable("usuarios", "gestion_usuarios");
                });

            modelBuilder.Entity("DAL.Entidades.Moto", b =>
                {
                    b.HasOne("DAL.Entidades.Usuario", "IdUsuarioPropietarioNavigation")
                        .WithMany("Motos")
                        .HasForeignKey("IdUsuarioPropietario")
                        .HasConstraintName("fkrj104ma5k7u5q0815pwuvvkyl");

                    b.Navigation("IdUsuarioPropietarioNavigation");
                });

            modelBuilder.Entity("DAL.Entidades.Participante", b =>
                {
                    b.HasOne("DAL.Entidades.Quedada", "IdQuedadaNavigation")
                        .WithMany()
                        .HasForeignKey("IdQuedada")
                        .IsRequired()
                        .HasConstraintName("fks0eafcfbbgb26vqqajbldul8e");

                    b.HasOne("DAL.Entidades.Usuario", "IdUsuarioNavigation")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .IsRequired()
                        .HasConstraintName("fkd68ea19e1man3k5vcyjl4d8dl");

                    b.Navigation("IdQuedadaNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("DAL.Entidades.Usuario", b =>
                {
                    b.Navigation("Motos");
                });
#pragma warning restore 612, 618
        }
    }
}