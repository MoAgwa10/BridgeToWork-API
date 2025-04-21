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

            




    }
}
