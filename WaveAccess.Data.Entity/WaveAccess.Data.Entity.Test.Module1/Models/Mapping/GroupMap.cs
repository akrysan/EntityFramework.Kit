using System.Data.Entity.ModelConfiguration;

namespace WaveAccess.Data.Entity.Test.Module1.Models.Mapping
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {

            this.HasKey(t => t.Id);
            //
            this.ToTable("Groups");
            // users
            this.HasMany(x => x.Users)
                .WithMany(x => x.Groups)
                .Map(m => m
                    .ToTable("UserGroups")
                    .MapLeftKey("GroupId")
                    .MapRightKey("UserId"));
        }
    }
}
