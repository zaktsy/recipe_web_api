﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace recipe_web_api.Models
{
    public partial class recipesdbContext : DbContext
    {
        public recipesdbContext()
        {
        }

        public recipesdbContext(DbContextOptions<recipesdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Fridge> Fridges { get; set; } = null!;
        public virtual DbSet<Kitchen> Kitchens { get; set; } = null!;
        public virtual DbSet<Meal> Meals { get; set; } = null!;
        public virtual DbSet<Measure> Measures { get; set; } = null!;
        public virtual DbSet<Peculiarity> Peculiarities { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductRecipe> ProductRecipes { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; } = null!;
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-16JKD4HS\\SQLEXPRESS;Initial Catalog=recipesdb;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Categories")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Fridge>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Productid, e.Measureid })
                    .HasName("PK_Fridge_1");

                entity.ToTable("Fridge");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Measureid).HasColumnName("measureid");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.HasOne(d => d.Measure)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.Measureid)
                    .HasConstraintName("FK_Fridge_Measures");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_Fridge_Products");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_Fridge_Users");
            });

            modelBuilder.Entity<Kitchen>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Kitchens")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Meals")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Measure>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Peculiarity>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_Peculiarities")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasMany(d => d.Recipes)
                    .WithMany(p => p.Peculiarities)
                    .UsingEntity<Dictionary<string, object>>(
                        "RecepePec",
                        l => l.HasOne<Recipe>().WithMany().HasForeignKey("Recipeid").HasConstraintName("FK_RecepePec_Recipes"),
                        r => r.HasOne<Peculiarity>().WithMany().HasForeignKey("Peculiarityid").HasConstraintName("FK_RecepePec_Peculiarities"),
                        j =>
                        {
                            j.HasKey("Peculiarityid", "Recipeid");

                            j.ToTable("RecepePec");

                            j.IndexerProperty<int>("Peculiarityid").HasColumnName("peculiarityid");

                            j.IndexerProperty<int>("Recipeid").HasColumnName("recipeid");
                        });
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Photo)
                    .HasMaxLength(50)
                    .HasColumnName("photo");
            });

            modelBuilder.Entity<ProductRecipe>(entity =>
            {
                entity.HasKey(e => new { e.Recipeid, e.Productid });

                entity.ToTable("ProductRecipe");

                entity.Property(e => e.Recipeid).HasColumnName("recipeid");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Mesureid).HasColumnName("mesureid");

                entity.HasOne(d => d.Mesure)
                    .WithMany(p => p.ProductRecipes)
                    .HasForeignKey(d => d.Mesureid)
                    .HasConstraintName("FK_ProductRecipe_Measures");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductRecipes)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_ProductRecipe_Products");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.ProductRecipes)
                    .HasForeignKey(d => d.Recipeid)
                    .HasConstraintName("FK_ProductRecipe_Recipes");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Fat).HasColumnName("fat");

                entity.Property(e => e.Kitchenid).HasColumnName("kitchenid");

                entity.Property(e => e.Mealid).HasColumnName("mealid");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Proteins).HasColumnName("proteins");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.Categoryid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Recipes_Categories");

                entity.HasOne(d => d.Kitchen)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.Kitchenid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Recipes_Kitchens");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.Mealid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Recipes_Meals");
            });

            modelBuilder.Entity<RecipeStep>(entity =>
            {
                entity.HasKey(e => new { e.Recipeid, e.Stepnumber });

                entity.ToTable("RecipeStep");

                entity.Property(e => e.Recipeid).HasColumnName("recipeid");

                entity.Property(e => e.Stepnumber).HasColumnName("stepnumber");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeSteps)
                    .HasForeignKey(d => d.Recipeid)
                    .HasConstraintName("FK_RecipeStep_Recipes");
            });

            modelBuilder.Entity<ShoppingList>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Productid, e.Measureid });

                entity.ToTable("ShoppingList");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Measureid).HasColumnName("measureid");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.HasOne(d => d.Measure)
                    .WithMany(p => p.ShoppingLists)
                    .HasForeignKey(d => d.Measureid)
                    .HasConstraintName("FK_ShoppingList_Measures");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingLists)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_ShoppingList_Products");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ShoppingLists)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_ShoppingList_Users1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "IX_Users")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Admin).HasColumnName("admin");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("surname");

                entity.HasMany(d => d.Recipes)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "Favorite",
                        l => l.HasOne<Recipe>().WithMany().HasForeignKey("Recipeid").HasConstraintName("FK_Favorites_Recipes"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("Userid").HasConstraintName("FK_Favorites_Users"),
                        j =>
                        {
                            j.HasKey("Userid", "Recipeid");

                            j.ToTable("Favorites");

                            j.IndexerProperty<int>("Userid").HasColumnName("userid");

                            j.IndexerProperty<int>("Recipeid").HasColumnName("recipeid");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
