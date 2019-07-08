using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocumentManagement.Infrastructure.Security
{
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }
    public class ApplicationUserRole : IdentityUserRole<string> { }

    // Must be expressed in terms of our custom Role and other types:
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }

    }


    // Must be expressed in terms of our custom UserRole:
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name) : this()
        {
            this.Name = name;
        }
    }


    // Must be expressed in terms of our custom types:
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        // Add additional items here as needed
    }

    // Most likely won't need to customize these either, but they were needed because we implemented
    // custom versions of all the other types:
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, string,ApplicationUserLogin, ApplicationUserRole,ApplicationUserClaim>, IUserStore<ApplicationUser, string>,IDisposable
    {
        public ApplicationUserStore(): this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context): base(context)
        {
        }
    }


    public class ApplicationRoleStore : RoleStore<ApplicationRole, string, ApplicationUserRole>,IQueryableRoleStore<ApplicationRole, string>,IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore(): base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context): base(context)
        {
        }
    }

}
