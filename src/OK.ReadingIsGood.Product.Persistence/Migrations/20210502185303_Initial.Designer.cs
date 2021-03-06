// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OK.ReadingIsGood.Product.Persistence.Contexts;

namespace OK.ReadingIsGood.Product.Persistence.Migrations
{
    [DbContext(typeof(ProductDataContext))]
    [Migration("20210502185303_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("OK.ReadingIsGood.Product.Persistence.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StockCount")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Products", "rig_product");
                });

            modelBuilder.Entity("OK.ReadingIsGood.Shared.Persistence.Entities.ChangeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ChangesetId")
                        .HasColumnType("integer");

                    b.Property<string>("NewValue")
                        .HasColumnType("text");

                    b.Property<string>("OldValue")
                        .HasColumnType("text");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChangesetId");

                    b.ToTable("Changes", "rig_product");
                });

            modelBuilder.Entity("OK.ReadingIsGood.Shared.Persistence.Entities.ChangesetEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("ChangedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Changesets", "rig_product");
                });

            modelBuilder.Entity("OK.ReadingIsGood.Shared.Persistence.Entities.ChangeEntity", b =>
                {
                    b.HasOne("OK.ReadingIsGood.Shared.Persistence.Entities.ChangesetEntity", "Changeset")
                        .WithMany("Changes")
                        .HasForeignKey("ChangesetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Changeset");
                });

            modelBuilder.Entity("OK.ReadingIsGood.Shared.Persistence.Entities.ChangesetEntity", b =>
                {
                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}
