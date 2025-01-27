using Microsoft.EntityFrameworkCore;
using sambackend;
using sambackend.Models;

namespace sambackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; } // ✅ Register BasketItems

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BasketItem relationships
            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Basket)
                .WithMany(b => b.Items)
                .HasForeignKey(bi => bi.BasketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BasketItem>()
                .HasOne(bi => bi.Dish)
                .WithMany()
                .HasForeignKey(bi => bi.DishId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Seed Dish data (unchanged from your original code)
            modelBuilder.Entity<Dish>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Dish>().HasData(
                new Dish
                {
                    Id = Guid.Parse("a91c1b34-f8d1-4e5b-8334-5d0f8b845c5a"),
                    Name = "4 сыра",
                    Description = "4 сыра: «Моцарелла», «Гауда», «Фета», «Дор-блю», сливочно-сырный соус, пряные травы",
                    Price = 360,
                    Image = "https://mistertako.ru/uploads/products/77888c7e-8327-11ec-8575-0050569dbef0.",
                    Vegetarian = true,
                    Rating = 4
                },
                new Dish
                {
                    Id = Guid.Parse("f2a2b3e4-d715-4c58-a7bc-672978b58ab2"),
                    Name = "Party BBQ",
                    Description = "Бекон, соленый огурчик, брусника, сыр «Моцарелла», сыр «Гауда», соус BBQ",
                    Price = 480,
                    Image = "https://mistertako.ru/uploads/products/839d0250-8327-11ec-8575-0050569dbef0.",
                    Vegetarian = false,
                    Rating = 0
                },
                new Dish
                {
                    Id = Guid.Parse("c21d34f5-6b2c-4d7e-8fda-57e2f6492d93"),
                    Name = "Wok а-ля Диаблo",
                    Description = "Пшеничная лапша обжаренная на воке с колбасками пепперони, маслинами, сладким перцем и перцем халапеньо в томатном соусе с добавлением петрушки.",
                    Price = 330,
                    Image = "https://mistertako.ru/uploads/products/663ab868-85ec-11ea-a9ab-86b1f8341741.jpg",
                    Vegetarian = false,
                    Rating = 5
                },
                new Dish
                {
                    Id = Guid.Parse("d56e3b6e-78f3-43f5-85c9-f6218ba63c11"),
                    Name = "Wok болоньезе",
                    Description = "Пшеничная лапша обжаренная на воке с фаршем (Говядина/свинина) и овощами.",
                    Price = 290,
                    Image = "https://mistertako.ru/uploads/products/663ab866-85ec-11ea-a9ab-86b1f8341741.jpg",
                    Vegetarian = false,
                    Rating = 0
                },
                new Dish
                {
                    Id = Guid.Parse("b78d9f67-86c5-44eb-8128-a5a473b0d243"),
                    Name = "Wok том ям с курицей",
                    Description = "Лапша пшеничная, куриное филе, шампиньоны, лук красный, заправка Том Ям.",
                    Price = 280,
                    Image = "https://mistertako.ru/uploads/products/a41bd9fd-54ed-11ed-8575-0050569dbef0.jpg",
                    Vegetarian = false,
                    Rating = 0
                }
            );
        }
    }
}
