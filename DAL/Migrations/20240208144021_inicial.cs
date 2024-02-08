using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gestion_logicanegocio");

            migrationBuilder.EnsureSchema(
                name: "gestion_usuarios");

            migrationBuilder.CreateTable(
                name: "quedadas",
                schema: "gestion_logicanegocio",
                columns: table => new
                {
                    id_quedada = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fch_hora_encuentro = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    usuario_organizador = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lugar = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    desc_quedada = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("quedadas_pkey", x => x.id_quedada);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                schema: "gestion_usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cuenta_confirmada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    fch_expiracion_token = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    fch_registro = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    tlf_movil = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    rol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nombre_apellidos = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    contraseña = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    foto = table.Column<byte[]>(type: "bytea", nullable: true),
                    token_recuperacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "motos",
                schema: "gestion_logicanegocio",
                columns: table => new
                {
                    id_moto = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    año = table.Column<int>(type: "integer", nullable: false),
                    id_usuario_propietario = table.Column<long>(type: "bigint", nullable: true),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    marca = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    modelo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    desc_modificaciones = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("motos_pkey", x => x.id_moto);
                    table.ForeignKey(
                        name: "fkrj104ma5k7u5q0815pwuvvkyl",
                        column: x => x.id_usuario_propietario,
                        principalSchema: "gestion_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "participantes",
                schema: "gestion_logicanegocio",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_quedada = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participantes", x => x.Id);
                    table.ForeignKey(
                        name: "fkd68ea19e1man3k5vcyjl4d8dl",
                        column: x => x.id_usuario,
                        principalSchema: "gestion_usuarios",
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "fks0eafcfbbgb26vqqajbldul8e",
                        column: x => x.id_quedada,
                        principalSchema: "gestion_logicanegocio",
                        principalTable: "quedadas",
                        principalColumn: "id_quedada");
                });

            migrationBuilder.CreateIndex(
                name: "IX_motos_id_usuario_propietario",
                schema: "gestion_logicanegocio",
                table: "motos",
                column: "id_usuario_propietario");

            migrationBuilder.CreateIndex(
                name: "IX_participantes_id_quedada",
                schema: "gestion_logicanegocio",
                table: "participantes",
                column: "id_quedada");

            migrationBuilder.CreateIndex(
                name: "IX_participantes_id_usuario",
                schema: "gestion_logicanegocio",
                table: "participantes",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "usuarios_email_key",
                schema: "gestion_usuarios",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "motos",
                schema: "gestion_logicanegocio");

            migrationBuilder.DropTable(
                name: "participantes",
                schema: "gestion_logicanegocio");

            migrationBuilder.DropTable(
                name: "usuarios",
                schema: "gestion_usuarios");

            migrationBuilder.DropTable(
                name: "quedadas",
                schema: "gestion_logicanegocio");
        }
    }
}
