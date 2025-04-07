﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RpgApi.Data;

#nullable disable

namespace RpgApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250407111704_MigracaoUsuario")]
    partial class MigracaoUsuario
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RpgApi.Models.Arma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Dano")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("Varchar");

                    b.HasKey("Id");

                    b.ToTable("TB_ARMAS", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Dano = 35,
                            Nome = "Arco e Flecha"
                        },
                        new
                        {
                            Id = 2,
                            Dano = 33,
                            Nome = "Espada Longa"
                        },
                        new
                        {
                            Id = 3,
                            Dano = 31,
                            Nome = "Machado"
                        },
                        new
                        {
                            Id = 4,
                            Dano = 30,
                            Nome = "Punho"
                        },
                        new
                        {
                            Id = 5,
                            Dano = 34,
                            Nome = "Chicote"
                        },
                        new
                        {
                            Id = 6,
                            Dano = 33,
                            Nome = "Foice"
                        },
                        new
                        {
                            Id = 7,
                            Dano = 32,
                            Nome = "Cajado"
                        });
                });

            modelBuilder.Entity("RpgApi.Models.Personagem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Classe")
                        .HasColumnType("int");

                    b.Property<int>("Defesa")
                        .HasColumnType("int");

                    b.Property<int>("Forca")
                        .HasColumnType("int");

                    b.Property<byte[]>("FotoPersonagem")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Inteligencia")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("Varchar");

                    b.Property<int>("PontosVida")
                        .HasColumnType("int");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("TB_PERSONAGENS", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Classe = 1,
                            Defesa = 23,
                            Forca = 17,
                            Inteligencia = 33,
                            Nome = "Frodo",
                            PontosVida = 100,
                            UsuarioId = 1
                        },
                        new
                        {
                            Id = 2,
                            Classe = 1,
                            Defesa = 25,
                            Forca = 15,
                            Inteligencia = 30,
                            Nome = "Sam",
                            PontosVida = 100,
                            UsuarioId = 1
                        },
                        new
                        {
                            Id = 3,
                            Classe = 3,
                            Defesa = 21,
                            Forca = 18,
                            Inteligencia = 35,
                            Nome = "Galadriel",
                            PontosVida = 100,
                            UsuarioId = 1
                        },
                        new
                        {
                            Id = 4,
                            Classe = 2,
                            Defesa = 18,
                            Forca = 18,
                            Inteligencia = 37,
                            Nome = "Gandalf",
                            PontosVida = 100,
                            UsuarioId = 1
                        },
                        new
                        {
                            Id = 5,
                            Classe = 1,
                            Defesa = 17,
                            Forca = 20,
                            Inteligencia = 31,
                            Nome = "Hobbit",
                            PontosVida = 100,
                            UsuarioId = 1
                        },
                        new
                        {
                            Id = 6,
                            Classe = 3,
                            Defesa = 13,
                            Forca = 21,
                            Inteligencia = 34,
                            Nome = "Celeborn",
                            PontosVida = 100,
                            UsuarioId = 1
                        },
                        new
                        {
                            Id = 7,
                            Classe = 2,
                            Defesa = 11,
                            Forca = 25,
                            Inteligencia = 35,
                            Nome = "Radagast",
                            PontosVida = 100,
                            UsuarioId = 1
                        });
                });

            modelBuilder.Entity("RpgApi.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DataAcesso")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .HasColumnType("Varchar");

                    b.Property<byte[]>("Foto")
                        .HasColumnType("varbinary(max)");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Perfil")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(200)
                        .HasColumnType("Varchar")
                        .HasDefaultValue("Jogador");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("Varchar");

                    b.HasKey("Id");

                    b.ToTable("TB_USUARIOS", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "seuEmailegmail.com",
                            Latitude = -23.520024100000001,
                            Longitude = -46.596497999999997,
                            PasswordHash = new byte[] { 119, 138, 157, 84, 158, 193, 46, 48, 211, 91, 252, 21, 128, 161, 26, 56, 121, 249, 137, 130, 192, 97, 70, 14, 14, 149, 68, 93, 60, 184, 97, 48, 233, 221, 200, 77, 161, 76, 182, 161, 46, 176, 214, 10, 25, 46, 3, 50, 224, 184, 242, 61, 108, 200, 179, 88, 56, 113, 63, 164, 145, 131, 68, 217 },
                            PasswordSalt = new byte[] { 125, 212, 116, 104, 251, 68, 56, 45, 81, 22, 172, 24, 114, 244, 224, 195, 221, 148, 9, 68, 18, 153, 163, 124, 77, 168, 92, 237, 38, 62, 4, 139, 99, 123, 7, 66, 71, 155, 135, 135, 143, 5, 50, 98, 158, 121, 22, 240, 73, 1, 204, 175, 121, 254, 226, 87, 204, 232, 249, 235, 123, 190, 162, 127, 110, 188, 91, 221, 99, 226, 44, 55, 178, 215, 28, 136, 205, 165, 169, 59, 105, 165, 135, 195, 226, 212, 10, 174, 238, 114, 36, 233, 206, 184, 202, 197, 4, 53, 23, 247, 4, 135, 254, 131, 11, 26, 8, 155, 120, 167, 3, 3, 107, 181, 56, 225, 206, 209, 210, 205, 55, 147, 236, 86, 254, 226, 173, 68 },
                            Perfil = "Admin",
                            Username = "UsuarioAdmin"
                        });
                });

            modelBuilder.Entity("RpgApi.Models.Personagem", b =>
                {
                    b.HasOne("RpgApi.Models.Usuario", "Usuario")
                        .WithMany("Personagens")
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("RpgApi.Models.Usuario", b =>
                {
                    b.Navigation("Personagens");
                });
#pragma warning restore 612, 618
        }
    }
}
