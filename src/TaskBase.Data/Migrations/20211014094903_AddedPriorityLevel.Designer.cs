﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskBase.Data;

namespace TaskBase.Data.Migrations
{
    [DbContext(typeof(TaskContext))]
    [Migration("20211014094903_AddedPriorityLevel")]
    partial class AddedPriorityLevel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TaskBase.Core.NotificationAggregate.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("taskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("taskId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.PriorityLevel", b =>
                {
                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Value");

                    b.ToTable("PriorityLevels");
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AssignToId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskState")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("priorityLevelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssignToId");

                    b.HasIndex("Id");

                    b.HasIndex("priorityLevelId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.Note", b =>
                {
                    b.HasOne("TaskBase.Core.TaskAggregate.Task", null)
                        .WithMany("Notes")
                        .HasForeignKey("taskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.Task", b =>
                {
                    b.HasOne("TaskBase.Core.TaskAggregate.User", "AssignTo")
                        .WithMany()
                        .HasForeignKey("AssignToId");

                    b.HasOne("TaskBase.Core.TaskAggregate.PriorityLevel", "PriorityLevel")
                        .WithMany()
                        .HasForeignKey("priorityLevelId");

                    b.Navigation("AssignTo");

                    b.Navigation("PriorityLevel");
                });

            modelBuilder.Entity("TaskBase.Core.TaskAggregate.Task", b =>
                {
                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
