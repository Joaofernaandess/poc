protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // A regra deve seguir EXATAMENTE a capitalização do seu DBeaver!
    modelBuilder.Entity<Cliente>().ToTable("Cliente"); // Note o 'C' maiúsculo

    // ... outras regras
    base.OnModelCreating(modelBuilder);
}