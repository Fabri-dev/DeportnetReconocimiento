﻿// <auto-generated />
using System;
using DeportNetReconocimiento.Api.BD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeportNetReconocimiento.Migrations
{
    [DbContext(typeof(BdContext))]
    [Migration("20250512140321_MigracionDePrueba")]
    partial class MigracionDePrueba
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Acceso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("ActiveBranchId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("active_branch_id");

                    b.Property<int?>("ProcessId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("process_id");

                    b.HasKey("Id");

                    b.ToTable("accesos", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.AccesoSocio", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT")
                        .HasColumnName("id");

                    b.Property<int?>("AccesoId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("access_id");

                    b.Property<DateTime>("AccessDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("access_date");

                    b.Property<int>("CompanyMemberId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("company_member_id");

                    b.Property<bool>("IsSuccessful")
                        .HasColumnType("INTEGER")
                        .HasColumnName("isSuccessful");

                    b.Property<int>("MemberId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("member_id");

                    b.HasKey("Id");

                    b.HasIndex("AccesoId");

                    b.ToTable("accesos_socios", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Articulo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasMaxLength(50)
                        .HasColumnType("REAL")
                        .HasColumnName("amount");

                    b.Property<int>("IdDx")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_dx");

                    b.Property<char>("IsSaleItem")
                        .HasColumnType("TEXT")
                        .HasColumnName("isSaleItem");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("articulos", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.ConfiguracionDeAcceso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int?>("CardLength")
                        .HasColumnType("INTEGER")
                        .HasColumnName("cardLength");

                    b.Property<string>("EndCharacter")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("endCharacter");

                    b.Property<string>("SecondStartCharacter")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("secondStartCharacter");

                    b.Property<string>("StartCharacter")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("startCharacter");

                    b.HasKey("Id");

                    b.ToTable("configuracion_de_acceso", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.ConfiguracionGeneral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime?>("AnteriorFechaSincronizacion")
                        .HasColumnType("TEXT")
                        .HasColumnName("prior_last_syncro");

                    b.Property<int?>("CantMaxLotes")
                        .HasColumnType("INTEGER")
                        .HasColumnName("max_lot_quantity");

                    b.Property<int?>("CapacidadMaximaRostros")
                        .HasColumnType("INTEGER")
                        .HasColumnName("max_face_quantity");

                    b.Property<string>("ContraseniaBd")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<string>("LectorActual")
                        .HasColumnType("TEXT")
                        .HasColumnName("face_reader");

                    b.Property<string>("NombreSucursal")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("branch_name");

                    b.Property<int?>("RostrosActuales")
                        .HasColumnType("INTEGER")
                        .HasColumnName("current_face_quantity");

                    b.Property<DateTime?>("UltimaFechaSincronizacion")
                        .HasColumnType("TEXT")
                        .HasColumnName("last_syncro");

                    b.HasKey("Id");

                    b.ToTable("configuracion_general", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Credenciales", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("BranchId")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("branch_id");

                    b.Property<string>("BranchToken")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("branch_token");

                    b.Property<string>("CurrentCompanyMemberId")
                        .HasColumnType("TEXT")
                        .HasColumnName("current_company_member_id");

                    b.Property<string>("Ip")
                        .HasMaxLength(15)
                        .HasColumnType("TEXT")
                        .HasColumnName("ip");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<string>("Port")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT")
                        .HasColumnName("port");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("credenciales", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Empleado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("CompanyMemberId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("company_member_id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("first_name");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT")
                        .HasColumnName("full_name");

                    b.Property<string>("IsActive")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("is_active");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.ToTable("empleados", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Membresia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasMaxLength(50)
                        .HasColumnType("REAL")
                        .HasColumnName("amount");

                    b.Property<int>("Days")
                        .HasColumnType("INTEGER")
                        .HasColumnName("days");

                    b.Property<int>("IdDx")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_dx");

                    b.Property<char>("IsSaleItem")
                        .HasColumnType("TEXT")
                        .HasColumnName("isSaleItem");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<int>("Period")
                        .HasColumnType("INTEGER")
                        .HasColumnName("period");

                    b.HasKey("Id");

                    b.ToTable("membresias", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Socio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("address");

                    b.Property<string>("AddressFloor")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("address_floor");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("TEXT")
                        .HasColumnName("birth_date");

                    b.Property<string>("CardNumber")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("card_number");

                    b.Property<string>("Cellphone")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("cellphone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("first_name");

                    b.Property<string>("Gender")
                        .HasMaxLength(1)
                        .HasColumnType("TEXT")
                        .HasColumnName("gender");

                    b.Property<int?>("IdDx")
                        .HasColumnType("INTEGER")
                        .HasColumnName("id_dx");

                    b.Property<string>("IdNumber")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("id_number");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT")
                        .HasColumnName("image_url");

                    b.Property<string>("IsActive")
                        .HasMaxLength(1)
                        .HasColumnType("TEXT")
                        .HasColumnName("is_active");

                    b.Property<string>("IsValid")
                        .HasMaxLength(1)
                        .HasColumnType("TEXT")
                        .HasColumnName("is_valid");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("last_name");

                    b.HasKey("Id");

                    b.ToTable("socios", (string)null);
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.AccesoSocio", b =>
                {
                    b.HasOne("DeportNetReconocimiento.Api.Data.Domain.Acceso", "Acceso")
                        .WithMany("MemberAccess")
                        .HasForeignKey("AccesoId");

                    b.Navigation("Acceso");
                });

            modelBuilder.Entity("DeportNetReconocimiento.Api.Data.Domain.Acceso", b =>
                {
                    b.Navigation("MemberAccess");
                });
#pragma warning restore 612, 618
        }
    }
}
