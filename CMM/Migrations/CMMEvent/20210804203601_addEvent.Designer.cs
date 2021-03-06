// <auto-generated />
using System;
using CMM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CMM.Migrations.CMMEvent
{
    [DbContext(typeof(CMMEventContext))]
    [Migration("20210804203601_addEvent")]
    partial class addEvent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CMM.Models.Event", b =>
                {
                    b.Property<int>("ConcertID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ConcertDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcertDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("ConcertLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcertMusician")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ConcertName")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("ConcertPoster")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ConcertPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ConcertStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ConcertVisibility")
                        .HasColumnType("bit");

                    b.Property<int>("TicketLimit")
                        .HasColumnType("int");

                    b.Property<int>("TicketPurchased")
                        .HasColumnType("int");

                    b.HasKey("ConcertID");

                    b.ToTable("Event");
                });
#pragma warning restore 612, 618
        }
    }
}
