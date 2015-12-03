using System.Data.Entity.ModelConfiguration;

namespace WaveAccess.Data.Entity.Test.Module2.Models.Mapping
{
    public class TaskMap : EntityTypeConfiguration<Task>
    {
        public TaskMap()
        {

            this.HasKey(t => t.Id);
            //
            this.ToTable("Tasks");

            this.HasRequired(x => x.User);
            }
    }
}
