using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Data.DatabaseProvider.Configurations
{
    internal class ElementConfiguration : IEntityTypeConfiguration<Element>
    {
        public void Configure(EntityTypeBuilder<Element> builder)
        {
            builder.HasKey(e => e.Id);  
        }
    }
}
