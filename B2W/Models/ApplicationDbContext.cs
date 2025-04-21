using B2W.Models.Authentication;
using B2W.Models.Jop;
using B2W.Models.User;
using B2W.Models.UserCertifications;
using B2W.Models.UserComment;
using B2W.Models.Userpost;
using B2W.Models.UserProfilePic;
using B2W.Models.UserRecations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using B2W.Models.CompanyProfile;

namespace B2W.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        public DbSet<Post> posts { get; set; }
        public DbSet<Comment>comments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserReaction> userReactions { get; set; }
        public DbSet<ReactionType> reactionTypes { get; set; }
        public DbSet<UserProfilePicture> userProfilePictures { get; set; }
        public DbSet<UserCertification> userCertifications { get; set; }
        public DbSet<Jop.Jop> Jops { get; set; } 
        public DbSet<JopApply> JopApplies { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<CompanyProfile.CompanyProfile> CompanyProfiles { get; set; }
       
        public DbSet<CompanyProfile.CompanyProfile> companyProfiles { get; set; }
        public DbSet<CompanyReview>CompanyReviews {  get; set; }               
        public DbSet<CompanyEmployee> CompanyEmployees { get; set; }
        public DbSet<AccessibilityFeature> AccessibilityFeatures { get; set; }
        public DbSet<MillStones> MillStones { get; set; }
        public DbSet<Cv> Cvs { get; set; }
        public DbSet<Projects> Projects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // العلاقة بين Cv و UserProfile (One to One)
            modelBuilder.Entity<Cv>()
                .HasOne(cv => cv.UserProfile)
                .WithOne(up => up.UserCv)
                .HasForeignKey<Cv>(cv => cv.UserProfileId);

            // العلاقة بين JopApply و Jop (Many to One)
            modelBuilder.Entity<JopApply>()
                .HasOne(ja => ja.JopSeeker)
                .WithMany(j => j.JopApplies)
                .HasForeignKey(ja => ja.JopId)
                .OnDelete(DeleteBehavior.Restrict); // منع الحذف التلقائي لتجنب التعارض

            // العلاقة بين JopApply و ApplicationUser (Many to One)
            modelBuilder.Entity<JopApply>()
                .HasOne(ja => ja.ApplicationUser)
                .WithMany()
                .HasForeignKey(ja => ja.UserId)
                .OnDelete(DeleteBehavior.NoAction); // لما اليوزر يتحذف، الابلايز تفضل موجودة

            // العلاقة بين Jop و CompanyProfile (Many to One)
            modelBuilder.Entity<Jop.Jop>()
                .HasOne(j => j.CompanyProfile)
                .WithMany(cp => cp.OpenedJobs)
                .HasForeignKey(j => j.CompanyProfileId)
                .OnDelete(DeleteBehavior.NoAction); // منع الحذف التلقائي لتفادي Multiple Cascade Paths


            modelBuilder.Entity<CompanyEmployee>()
                .HasOne(ce => ce.UserProfile)
                .WithMany()
                .HasForeignKey(ce => ce.UserProfileId)
              .OnDelete(DeleteBehavior.NoAction); //  كده منعنا الحذف التلقائي

            // العلاقة بين CompanyEmployee و CompanyProfile
            modelBuilder.Entity<CompanyEmployee>()
                .HasOne(e => e.CompanyProfile)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CompanyProfileId)
                .OnDelete(DeleteBehavior.Cascade); // لما الشركة تتحذف، الموظفين المرتبطين بيها يتمسحوا

            // العلاقة بين CompanyReview و UserProfile
            modelBuilder.Entity<CompanyReview>()
                .HasOne(r => r.UserProfile)
                .WithMany()
                .HasForeignKey(r => r.UserProfileId)
                .OnDelete(DeleteBehavior.NoAction); // علشان نتجنب مسارات الحذف المتعددة

            // العلاقة بين CompanyReview و CompanyProfile
            modelBuilder.Entity<CompanyReview>()
                .HasOne(r => r.CompanyProfile)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CompanyProfileId)
                .OnDelete(DeleteBehavior.Cascade); // لو الشركة اتحذفت، الريفيوهات تتحذف

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.CompanyProfile)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<CompanyProfile.CompanyProfile>(c => c.ApplicationUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.UserProfile)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey<UserProfile>(p => p.ApplicationUserId);
        }




    }
}
