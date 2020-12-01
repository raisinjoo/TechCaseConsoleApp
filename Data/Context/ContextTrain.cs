using System.Data.Entity;

namespace Data.Context
{
    public class ContextTrain : DbContext
    {
        public ContextTrain() : base("DBConnection")
        { }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Carrige> Carriges { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Station> Stations { get; set; }
    }
}
