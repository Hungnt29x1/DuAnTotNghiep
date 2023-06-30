using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GProject.Data.Configurations;
using GProject.Data.DomainClass;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GProject.Data.Context
{
    public class GProjectContext : IdentityDbContext<Customer>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //@"Data Source=DESKTOP-VVGQQLR;Initial Catalog=DB_DATNChuan;Integrated Security=True"
            //@"Data Source=LAPTOP-2GTIBL55\SQLEXPRESS;Initial Catalog=ABC;Integrated Security=True"


            optionsBuilder.UseSqlServer(@"Data Source=HP\SQLEXPRESS;Initial Catalog=DB_Du_An;Integrated Security=True");



            //DB_Du_An

            //DESKTOP-M2OGLAO    DB_DATN
            //D-SDC6-HUYLQ   ABC
        }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<AppRoles> AppRoles { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Deliver> Delivers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Evaluate> Evaluates { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        //3. Khai báo các bảng DBSet sẽ được ánh xạ sang CSDL
        public DbSet<ProductVariation> ProductVariations { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionDetail> PromotionDetails { get; set; }
        public DbSet<SendMail> SendMails { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<ViewHistory> ViewHistories { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<DeliveryAddress>  DeliveryAddresses { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new AppUsers_Configuration());
            modelBuilder.ApplyConfiguration(new AppRoles_Configuration());
            modelBuilder.ApplyConfiguration(new Brand_Configuration());
            modelBuilder.ApplyConfiguration(new Cart_Configuration());
            modelBuilder.ApplyConfiguration(new CartDetail_Configuration());
            modelBuilder.ApplyConfiguration(new Category_Configuration());
            modelBuilder.ApplyConfiguration(new Color_Configuration());
            modelBuilder.ApplyConfiguration(new Contact_Configuration());
            modelBuilder.ApplyConfiguration(new Customer_Configuration());
            modelBuilder.ApplyConfiguration(new Deliver_Configuration());
            modelBuilder.ApplyConfiguration(new Employee_Configuration());
            modelBuilder.ApplyConfiguration(new Evaluate_Configuration());
            modelBuilder.ApplyConfiguration(new Order_Configuration());
            modelBuilder.ApplyConfiguration(new OrderDetail_Configuration());
            modelBuilder.ApplyConfiguration(new Posts_Configuration());
            modelBuilder.ApplyConfiguration(new Product_Configuration());
            modelBuilder.ApplyConfiguration(new ProductVariation_Configuration());
            modelBuilder.ApplyConfiguration(new Promotion_Configuration());
            modelBuilder.ApplyConfiguration(new PromotionDetail_Configuration());
            modelBuilder.ApplyConfiguration(new SendMail_Configuration());
            modelBuilder.ApplyConfiguration(new Size_Configuration());
            modelBuilder.ApplyConfiguration(new Slide_Configuration());
            modelBuilder.ApplyConfiguration(new ViewHistory_Configuration());
            modelBuilder.ApplyConfiguration(new FavoriteProduct_Configuration());
            modelBuilder.ApplyConfiguration(new DeliveryAddress_Configuration());
            modelBuilder.ApplyConfiguration(new Voucher_Configuration());

            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
        }
    }
}
