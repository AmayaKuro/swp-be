﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using swp_be.Data;

#nullable disable

namespace swp_be.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("swp_be.Models.AddOn", b =>
                {
                    b.Property<int>("AddOnID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddOnID"));

                    b.Property<int?>("ConsignmentKoiID")
                        .HasColumnType("int");

                    b.Property<string>("HealthCertificate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("KoiID")
                        .HasColumnType("int");

                    b.Property<string>("OriginCertificate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnershipCertificate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddOnID");

                    b.HasIndex("ConsignmentKoiID")
                        .IsUnique()
                        .HasFilter("[ConsignmentKoiID] IS NOT NULL");

                    b.HasIndex("KoiID")
                        .IsUnique()
                        .HasFilter("[KoiID] IS NOT NULL");

                    b.ToTable("AddOn");
                });

            modelBuilder.Entity("swp_be.Models.Batch", b =>
                {
                    b.Property<int>("BatchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BatchID"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("PricePerBatch")
                        .HasColumnType("bigint");

                    b.Property<int>("QuantityPerBatch")
                        .HasColumnType("int");

                    b.Property<int>("RemainBatch")
                        .HasColumnType("int");

                    b.Property<string>("Species")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BatchID");

                    b.ToTable("Batches");
                });

            modelBuilder.Entity("swp_be.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogId"));

                    b.Property<string>("BlogSlug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("BlogId");

                    b.HasIndex("UserID");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("swp_be.Models.Consignment", b =>
                {
                    b.Property<int>("ConsignmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConsignmentID"));

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("FosterPrice")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ConsignmentID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Consignments");
                });

            modelBuilder.Entity("swp_be.Models.ConsignmentKoi", b =>
                {
                    b.Property<int>("ConsignmentKoiID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConsignmentKoiID"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("ConsignmentID")
                        .HasColumnType("int");

                    b.Property<string>("DailyFeedAmount")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("FosteringDays")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Origin")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Personality")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("SelectionRate")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Size")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ConsignmentKoiID");

                    b.HasIndex("ConsignmentID");

                    b.ToTable("ConsignmentKois");
                });

            modelBuilder.Entity("swp_be.Models.Customer", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("LoyaltyPoints")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("swp_be.Models.Delivery", b =>
                {
                    b.Property<int>("DeliveryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDeliDay")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDeliDay")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("DeliveryID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("OrderID");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("swp_be.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackID"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CustomerID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateFb")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("OrderID");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("swp_be.Models.Koi", b =>
                {
                    b.Property<int>("KoiID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KoiID"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DailyFeedAmount")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Origin")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Personality")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<string>("SelectionRate")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Size")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("KoiID");

                    b.ToTable("Kois");
                });

            modelBuilder.Entity("swp_be.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderID"));

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("PromotionID")
                        .HasColumnType("int");

                    b.Property<int?>("StaffID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasMaxLength(250)
                        .HasColumnType("int");

                    b.Property<long>("TotalAmount")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("PromotionID");

                    b.HasIndex("StaffID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("swp_be.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailID"));

                    b.Property<int?>("BatchID")
                        .HasColumnType("int");

                    b.Property<int?>("ConsignmentKoiID")
                        .HasColumnType("int");

                    b.Property<int?>("KoiID")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<long>("Price")
                        .HasColumnType("bigint");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.HasKey("OrderDetailID");

                    b.HasIndex("BatchID");

                    b.HasIndex("ConsignmentKoiID");

                    b.HasIndex("KoiID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("swp_be.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodID"));

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PaymentMethodID");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("swp_be.Models.Promotion", b =>
                {
                    b.Property<int>("PromotionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PromotionID"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DiscountRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RemainingRedeem")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PromotionID");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("swp_be.Models.Staff", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("swp_be.Models.Token", b =>
                {
                    b.Property<string>("TokenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("TokenID");

                    b.HasIndex("UserID");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("swp_be.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"));

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<int?>("ConsignmentID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("TransactionID");

                    b.HasIndex("ConsignmentID");

                    b.HasIndex("OrderID");

                    b.HasIndex("PaymentMethodID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("swp_be.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("swp_be.Models.AddOn", b =>
                {
                    b.HasOne("swp_be.Models.ConsignmentKoi", "ConsignmentKoi")
                        .WithOne("AddOn")
                        .HasForeignKey("swp_be.Models.AddOn", "ConsignmentKoiID");

                    b.HasOne("swp_be.Models.Koi", "Koi")
                        .WithOne("AddOn")
                        .HasForeignKey("swp_be.Models.AddOn", "KoiID");

                    b.Navigation("ConsignmentKoi");

                    b.Navigation("Koi");
                });

            modelBuilder.Entity("swp_be.Models.Blog", b =>
                {
                    b.HasOne("swp_be.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("swp_be.Models.Consignment", b =>
                {
                    b.HasOne("swp_be.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("swp_be.Models.ConsignmentKoi", b =>
                {
                    b.HasOne("swp_be.Models.Consignment", "Consignment")
                        .WithMany("ConsignmentKois")
                        .HasForeignKey("ConsignmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consignment");
                });

            modelBuilder.Entity("swp_be.Models.Customer", b =>
                {
                    b.HasOne("swp_be.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("swp_be.Models.Customer", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("swp_be.Models.Delivery", b =>
                {
                    b.HasOne("swp_be.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("swp_be.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("swp_be.Models.Feedback", b =>
                {
                    b.HasOne("swp_be.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("swp_be.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("swp_be.Models.Order", b =>
                {
                    b.HasOne("swp_be.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("swp_be.Models.Promotion", "Promotion")
                        .WithMany()
                        .HasForeignKey("PromotionID");

                    b.HasOne("swp_be.Models.Staff", "Staff")
                        .WithMany()
                        .HasForeignKey("StaffID");

                    b.Navigation("Customer");

                    b.Navigation("Promotion");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("swp_be.Models.OrderDetail", b =>
                {
                    b.HasOne("swp_be.Models.Batch", "Batch")
                        .WithMany()
                        .HasForeignKey("BatchID");

                    b.HasOne("swp_be.Models.ConsignmentKoi", "ConsignmentKoi")
                        .WithMany()
                        .HasForeignKey("ConsignmentKoiID");

                    b.HasOne("swp_be.Models.Koi", "Koi")
                        .WithMany()
                        .HasForeignKey("KoiID");

                    b.HasOne("swp_be.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");

                    b.Navigation("ConsignmentKoi");

                    b.Navigation("Koi");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("swp_be.Models.Staff", b =>
                {
                    b.HasOne("swp_be.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("swp_be.Models.Staff", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("swp_be.Models.Token", b =>
                {
                    b.HasOne("swp_be.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("swp_be.Models.Transaction", b =>
                {
                    b.HasOne("swp_be.Models.Consignment", "Consignment")
                        .WithMany()
                        .HasForeignKey("ConsignmentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("swp_be.Models.Order", "Order")
                        .WithMany("Transactions")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("swp_be.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Consignment");

                    b.Navigation("Order");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("swp_be.Models.Consignment", b =>
                {
                    b.Navigation("ConsignmentKois");
                });

            modelBuilder.Entity("swp_be.Models.ConsignmentKoi", b =>
                {
                    b.Navigation("AddOn")
                        .IsRequired();
                });

            modelBuilder.Entity("swp_be.Models.Koi", b =>
                {
                    b.Navigation("AddOn")
                        .IsRequired();
                });

            modelBuilder.Entity("swp_be.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
