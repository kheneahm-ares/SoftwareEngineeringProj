using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CodingBlogDemo2.Data;

namespace CodingBlogDemo2.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20171130052209_FolderModel2")]
    partial class FolderModel2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodingBlogDemo2.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsAdmin");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.CodeSnippet", b =>
                {
                    b.Property<int>("CodeSnippetId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("PostId");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("CodeSnippetId");

                    b.ToTable("CodeSnippets");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.CodeSnippetNoAnswer", b =>
                {
                    b.Property<int>("CodeSnippetNoAnswerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("PostId");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("CodeSnippetNoAnswerId");

                    b.ToTable("CodeSnippetNoAnswers");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.CodeSnippetNoAnswerSubmission", b =>
                {
                    b.Property<int>("CodeSnippetNoAnswerSubmissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssignmentId");

                    b.Property<bool>("IsCorrect");

                    b.Property<string>("UserAnswer");

                    b.Property<string>("UserEmail");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("CodeSnippetNoAnswerSubmissionId");

                    b.ToTable("CodeSnippetNoAnswerSubmissions");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.CodeSnippetSubmission", b =>
                {
                    b.Property<int>("CodeSnippetSubmissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssignmentId");

                    b.Property<bool>("IsCorrect");

                    b.Property<string>("UserCode");

                    b.Property<string>("UserEmail");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("CodeSnippetSubmissionId");

                    b.ToTable("CodeSnippetSubmissions");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserEmail");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.Folder", b =>
                {
                    b.Property<int>("FolderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("Name");

                    b.HasKey("FolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.MultipleChoice", b =>
                {
                    b.Property<int>("MultipleChoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("A");

                    b.Property<string>("Answer");

                    b.Property<string>("B");

                    b.Property<string>("C");

                    b.Property<string>("D");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("PostId");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("MultipleChoiceId");

                    b.ToTable("MultipleChoices");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.MultipleChoiceSubmission", b =>
                {
                    b.Property<int>("MultipleChoiceSubmissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer");

                    b.Property<int>("AssignmentId");

                    b.Property<bool>("IsCorrect");

                    b.Property<string>("UserEmail");

                    b.Property<DateTime>("WhenCreated");

                    b.Property<DateTime>("WhenEdited");

                    b.HasKey("MultipleChoiceSubmissionId");

                    b.ToTable("MultipleChoiceSubmissions");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssignmentId");

                    b.Property<int>("CourseId");

                    b.Property<int>("FolderId");

                    b.Property<int>("PostCategory");

                    b.HasKey("PostId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("CodingBlogDemo2.Models.Register", b =>
                {
                    b.Property<int>("RegisterId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseId");

                    b.Property<string>("UserEmail");

                    b.HasKey("RegisterId");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CodingBlogDemo2.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CodingBlogDemo2.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodingBlogDemo2.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
