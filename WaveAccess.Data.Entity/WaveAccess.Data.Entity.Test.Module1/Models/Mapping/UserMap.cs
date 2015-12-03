using System.Data.Entity.ModelConfiguration;

namespace WaveAccess.Data.Entity.Test.Module1.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {

            this.HasKey(t => t.Id);
            //
            this.ToTable("Users");

            this.HasMany(x => x.Groups)
                .WithMany(x => x.Users)
                .Map(m => m
                    .ToTable("UserGroups")
                    .MapLeftKey("UserId")
                    .MapRightKey("GroupId"));
        }
    }
}
