using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Data.DatabaseProvider.Configurations
{
    internal class ListOfItemsConfiguration : IEntityTypeConfiguration<ListOfItems>
    {
        public void Configure(EntityTypeBuilder<ListOfItems> builder)
        {
            builder.HasKey(l => l.Id);
            builder.HasMany(l => l.Elements)
                .WithOne(e => e.CollectionOfLists)
                .HasForeignKey(e => e.CollectionOfListsId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
