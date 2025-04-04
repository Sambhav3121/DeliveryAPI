﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using sambackend.Data;

#nullable disable

namespace sambackend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250124075144_FixDishSeedData")]
    partial class FixDishSeedData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("sambackend.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.Property<bool>("Vegetarian")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Dishes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a91c1b34-f8d1-4e5b-8334-5d0f8b845c5a"),
                            Category = 0,
                            Description = "4 сыра: «Моцарелла», «Гауда», «Фета», «Дор-блю», сливочно-сырный соус, пряные травы",
                            Image = "https://mistertako.ru/uploads/products/77888c7e-8327-11ec-8575-0050569dbef0.",
                            Name = "4 сыра",
                            Price = 360.0,
                            Rating = 4.0,
                            Vegetarian = true
                        },
                        new
                        {
                            Id = new Guid("f2a2b3e4-d715-4c58-a7bc-672978b58ab2"),
                            Category = 0,
                            Description = "Бекон, соленый огурчик, брусника, сыр «Моцарелла», сыр «Гауда», соус BBQ",
                            Image = "https://mistertako.ru/uploads/products/839d0250-8327-11ec-8575-0050569dbef0.",
                            Name = "Party BBQ",
                            Price = 480.0,
                            Rating = 0.0,
                            Vegetarian = false
                        },
                        new
                        {
                            Id = new Guid("c21d34f5-6b2c-4d7e-8fda-57e2f6492d93"),
                            Category = 0,
                            Description = "Пшеничная лапша обжаренная на воке с колбасками пепперони, маслинами, сладким перцем и перцем халапеньо в томатном соусе с добавлением петрушки.",
                            Image = "https://mistertako.ru/uploads/products/663ab868-85ec-11ea-a9ab-86b1f8341741.jpg",
                            Name = "Wok а-ля Диаблo",
                            Price = 330.0,
                            Rating = 5.0,
                            Vegetarian = false
                        },
                        new
                        {
                            Id = new Guid("d56e3b6e-78f3-43f5-85c9-f6218ba63c11"),
                            Category = 0,
                            Description = "Пшеничная лапша обжаренная на воке с фаршем (Говядина/свинина) и овощами.",
                            Image = "https://mistertako.ru/uploads/products/663ab866-85ec-11ea-a9ab-86b1f8341741.jpg",
                            Name = "Wok болоньезе",
                            Price = 290.0,
                            Rating = 0.0,
                            Vegetarian = false
                        },
                        new
                        {
                            Id = new Guid("b78d9f67-86c5-44eb-8128-a5a473b0d243"),
                            Category = 0,
                            Description = "Лапша пшеничная, куриное филе, шампиньоны, лук красный, заправка Том Ям.",
                            Image = "https://mistertako.ru/uploads/products/a41bd9fd-54ed-11ed-8575-0050569dbef0.jpg",
                            Name = "Wok том ям с курицей",
                            Price = 280.0,
                            Rating = 0.0,
                            Vegetarian = false
                        });
                });

            modelBuilder.Entity("sambackend.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
