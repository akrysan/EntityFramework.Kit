using System.Data.Entity.ModelConfiguration;

namespace WaveAccess.Data.Entity.Test.Module1.Models.Mapping
{
    public class GroupHierarchyMap : EntityTypeConfiguration<GroupHierarchy>
    {
        public GroupHierarchyMap()
        {

            this.HasKey(g => new { g.ChildId, g.ParentId });

            this.ToTable("v_GroupHierarchy");

            this.HasRequired(g => g.Child)
                .WithMany(m => m.Parents)
                .HasForeignKey(f => f.ChildId);

            this.HasRequired(g => g.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(f => f.ParentId);
        }
    }
}