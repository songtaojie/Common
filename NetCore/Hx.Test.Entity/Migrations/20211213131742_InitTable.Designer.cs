﻿//// <auto-generated />
//using System;
//using Hx.Test.Entity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

//namespace Hx.Test.Entity.Migrations
//{
//    [DbContext(typeof(DefaultDbContext))]
//    [Migration("20211213131742_InitTable")]
//    partial class InitTable
//    {
//        protected override void BuildTargetModel(ModelBuilder modelBuilder)
//        {
//#pragma warning disable 612, 618
//            modelBuilder
//                .HasAnnotation("Relational:MaxIdentifierLength", 64)
//                .HasAnnotation("ProductVersion", "5.0.9");

//            modelBuilder.Entity("Hx.Test.Entity.UserInfo", b =>
//                {
//                    b.Property<string>("Id")
//                        .ValueGeneratedOnAdd()
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<DateTime>("CreateTime")
//                        .HasColumnType("datetime");

//                    b.Property<string>("Creater")
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<string>("CreaterId")
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<string>("LastModifier")
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<string>("LastModifierId")
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<DateTime?>("LastModifyTime")
//                        .HasColumnType("datetime");

//                    b.Property<string>("NickName")
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<string>("PassWord")
//                        .IsRequired()
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.Property<string>("UserName")
//                        .IsRequired()
//                        .HasMaxLength(36)
//                        .HasColumnType("varchar(36)");

//                    b.HasKey("Id");

//                    b.ToTable("UserInfo");

//                    b.HasData(
//                        new
//                        {
//                            Id = "66227dc2-17fc-421a-9002-8f31d9431661",
//                            CreateTime = new DateTime(2021, 12, 13, 21, 17, 41, 701, DateTimeKind.Local).AddTicks(4279),
//                            Creater = "SuperAdmin",
//                            CreaterId = "SuperAdmin",
//                            LastModifier = "",
//                            LastModifierId = "",
//                            NickName = "宋",
//                            PassWord = "123456",
//                            UserName = "songtaojie"
//                        });
//                });
//#pragma warning restore 612, 618
//        }
//    }
//}
