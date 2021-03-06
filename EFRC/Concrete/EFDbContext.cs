namespace EFRC.Concrete
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using EFRC.Entities;

    public partial class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("name=RailCars")
        {
        }

        public virtual DbSet<GDSTAIT> GDSTAIT { get; set; }
        public virtual DbSet<GRUZ_FRONTS> GRUZ_FRONTS { get; set; }
        public virtual DbSet<GRUZS> GRUZS { get; set; }
        public virtual DbSet<Loading_data_SAP> Loading_data_SAP { get; set; }
        public virtual DbSet<NAZN_COUNTRIES> NAZN_COUNTRIES { get; set; }
        public virtual DbSet<NeighborStations> NeighborStations { get; set; }
        public virtual DbSet<OWNERS> OWNERS { get; set; }
        public virtual DbSet<OWNERS_COUNTRIES> OWNERS_COUNTRIES { get; set; }
        public virtual DbSet<SHOPS> SHOPS { get; set; }
        public virtual DbSet<STATIONS> STATIONS { get; set; }
        public virtual DbSet<TUPIKI> TUPIKI { get; set; }
        public virtual DbSet<VAG_CONDITIONS> VAG_CONDITIONS { get; set; }
        public virtual DbSet<VAG_CONDITIONS2> VAG_CONDITIONS2 { get; set; }
        public virtual DbSet<VAGONS> VAGONS { get; set; }
        public virtual DbSet<WAYS> WAYS { get; set; }
        public virtual DbSet<PARK_STATE> PARK_STATE { get; set; }
        public virtual DbSet<PARKS> PARKS { get; set; }
        public virtual DbSet<VAGON_OPERATIONS> VAGON_OPERATIONS { get; set; }

        //SAP
        public virtual DbSet<SAPIncSupply> SAPIncSupply { get; set; }
        // ����������� ������� Railway
        public virtual DbSet<TypeCargo> TypeCargo { get; set; }
        public virtual DbSet<ReferenceCargo> ReferenceCargo { get; set; }
        public virtual DbSet<ReferenceCountry> ReferenceCountry { get; set; }
        public virtual DbSet<ReferenceStation> ReferenceStation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GRUZS>()
                .HasMany(e => e.VAGON_OPERATIONS)
                .WithOptional(e => e.GRUZS)
                .HasForeignKey(e => e.id_gruz_amkr);

            modelBuilder.Entity<Loading_data_SAP>()
                .Property(e => e.tonu5)
                .HasPrecision(18, 3);

            modelBuilder.Entity<NAZN_COUNTRIES>()
                .Property(e => e.id_ora)
                .IsFixedLength();

            modelBuilder.Entity<OWNERS_COUNTRIES>()
                .Property(e => e.name)
                .IsFixedLength();

            modelBuilder.Entity<OWNERS_COUNTRIES>()
                .HasMany(e => e.OWNERS)
                .WithOptional(e => e.OWNERS_COUNTRIES)
                .HasForeignKey(e => e.id_country);

            modelBuilder.Entity<SHOPS>()
                .HasMany(e => e.VAGON_OPERATIONS)
                .WithOptional(e => e.SHOPS)
                .HasForeignKey(e => e.id_shop_gruz_for);

            modelBuilder.Entity<VAG_CONDITIONS2>()
                .HasMany(e => e.VAG_CONDITIONS21)
                .WithOptional(e => e.VAG_CONDITIONS22)
                .HasForeignKey(e => e.id_cond_after);

            modelBuilder.Entity<VAG_CONDITIONS2>()
                .HasMany(e => e.VAGON_OPERATIONS)
                .WithOptional(e => e.VAG_CONDITIONS2)
                .HasForeignKey(e => e.id_cond2);

            modelBuilder.Entity<VAG_CONDITIONS2>()
                .HasMany(e => e.WAYS)
                .WithOptional(e => e.VAG_CONDITIONS2)
                .HasForeignKey(e => e.bind_id_cond);

            modelBuilder.Entity<VAGONS>()
                .HasMany(e => e.VAGON_OPERATIONS)
                .WithOptional(e => e.VAGONS)
                .HasForeignKey(e => e.id_vagon);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.N_DOC)
                .HasPrecision(8, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.N_VAG)
                .HasPrecision(8, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.ID_STRAN)
                .HasPrecision(8, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.ID_SOB)
                .HasPrecision(5, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.ID_GODN)
                .HasPrecision(1, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.ID_GRUZ)
                .HasPrecision(5, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.STATUS)
                .HasPrecision(1, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.ID_GRUZ2)
                .HasPrecision(5, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.PR_V)
                .HasPrecision(1, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.PR_S)
                .HasPrecision(1, 0);

            modelBuilder.Entity<PARK_STATE>()
                .Property(e => e.PR_GR)
                .HasPrecision(1, 0);

            modelBuilder.Entity<VAGON_OPERATIONS>()
                .Property(e => e.weight_gruz)
                .HasPrecision(18, 3);

            //SAP
            modelBuilder.Entity<SAPIncSupply>()
                .Property(e => e.WeightDoc)
                .HasPrecision(18, 3);

            modelBuilder.Entity<SAPIncSupply>()
                .Property(e => e.WeightReweighing)
                .HasPrecision(18, 3);
        }
    }
}
