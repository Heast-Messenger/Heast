﻿// <auto-generated />
using System.Collections;
using Chat.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chat.Migrations
{
    [DbContext(typeof(PermissionContext))]
    [Migration("20230308122536_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Chat.Model.PermissionChannel", b =>
                {
                    b.Property<int>("PermissionChannelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PermissionChannelId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PermissionChannelId");

                    b.ToTable("permissionchannels", (string)null);
                });

            modelBuilder.Entity("Chat.Model.PermissionChannelPermissions", b =>
                {
                    b.Property<int>("PermissionRoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionChannelId")
                        .HasColumnType("integer");

                    b.Property<BitArray>("Permissions")
                        .IsRequired()
                        .HasColumnType("bit varying");

                    b.HasKey("PermissionRoleId", "PermissionChannelId");

                    b.HasIndex("PermissionChannelId");

                    b.ToTable("permissionchannelpermissions");
                });

            modelBuilder.Entity("Chat.Model.PermissionClient", b =>
                {
                    b.Property<int>("PermissionClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PermissionClientId"));

                    b.HasKey("PermissionClientId");

                    b.ToTable("permissionclients", (string)null);
                });

            modelBuilder.Entity("Chat.Model.PermissionClientRoles", b =>
                {
                    b.Property<int>("PermissionRoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionClientId")
                        .HasColumnType("integer");

                    b.HasKey("PermissionRoleId", "PermissionClientId");

                    b.HasIndex("PermissionClientId");

                    b.ToTable("permissionclientroles");
                });

            modelBuilder.Entity("Chat.Model.PermissionRole", b =>
                {
                    b.Property<int>("PermissionRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PermissionRoleId"));

                    b.Property<int>("Hierarchy")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<BitArray>("Permissions")
                        .IsRequired()
                        .HasColumnType("bit varying");

                    b.HasKey("PermissionRoleId");

                    b.ToTable("permissionroles", (string)null);
                });

            modelBuilder.Entity("Chat.Model.PermissionChannelPermissions", b =>
                {
                    b.HasOne("Chat.Model.PermissionChannel", "PermissionChannel")
                        .WithMany()
                        .HasForeignKey("PermissionChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Model.PermissionRole", "PermissionRole")
                        .WithMany()
                        .HasForeignKey("PermissionRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PermissionChannel");

                    b.Navigation("PermissionRole");
                });

            modelBuilder.Entity("Chat.Model.PermissionClientRoles", b =>
                {
                    b.HasOne("Chat.Model.PermissionClient", "PermissionClient")
                        .WithMany()
                        .HasForeignKey("PermissionClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chat.Model.PermissionRole", "PermissionRole")
                        .WithMany()
                        .HasForeignKey("PermissionRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PermissionClient");

                    b.Navigation("PermissionRole");
                });
#pragma warning restore 612, 618
        }
    }
}