using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using QLSL.Models;

namespace QLSL.DAL
{
    public class QLSLContext : DbContext
    {

        public QLSLContext() : base("QLSLContext")
        {
            
        }
        public DbSet<TLSignalPlan> TLSignalPlans { get; set; }
        public DbSet<TLNode> TLNodes { get; set; }
        public DbSet<ReasonChangeSP> ReasonChangeSPs { get; set; }
        public DbSet<TLNodeHitoryStatus> TLNodeHitoryStatuses { get; set; }
        public DbSet<TLNodeStatus> TLNodeStatuses { get; set; }

        public DbSet<TAccident> TAccidents { get; set; }
        public DbSet<AccidentType> AccidentTypes { get; set; }

        public DbSet<VMS> VMSs { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<InformationSource> InformationSources { get; set; }
        public DbSet<InformationVMS> InformationVMSs { get; set; }
        public DbSet<VMSHistoryStatus> VMSHistoryStatuses { get; set; }
        public DbSet<VMSStatus> VMSStatuses { get; set; }
        public DbSet<VMSEvent> VMSEvents { get; set; }
        public DbSet<AttachFile> AttachFiles { get; set; }


        public DbSet<CCTV> CCTVs { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<OwerCCTV> OwerCCTVs { get; set; }

   
        public DbSet<CAMType> CAMTypes { get; set; }

        public DbSet<CCTVStatus> CCTVStatuses { get; set; }
        public DbSet<CCTVError> CCTVErrors { get; set; }


        public DbSet<TunnelError> TunnelErrors { get; set; }


        public DbSet<PrimaryW> PrimaryW { get; set; }
        public DbSet<PrimaryWStatus> PrimaryWStatus { get; set; }
        public DbSet<PrimaryWError> PrimaryWError { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<TLNode>().Property(x => x.Lat).HasPrecision(11, 6);
            modelBuilder.Entity<TLNode>().Property(x => x.Lng).HasPrecision(11, 6);

            modelBuilder.Entity<PrimaryW>().Property(x => x.Lat).HasPrecision(11, 6);
            modelBuilder.Entity<PrimaryW>().Property(x => x.Lng).HasPrecision(11, 6);

            modelBuilder.Entity<VMS>().Property(x => x.Lat).HasPrecision(11, 6);
            modelBuilder.Entity<VMS>().Property(x => x.Lng).HasPrecision(11, 6);

            modelBuilder.Entity<CCTV>().Property(x => x.Lat).HasPrecision(11, 6);
            modelBuilder.Entity<CCTV>().Property(x => x.Lng).HasPrecision(11, 6);

            modelBuilder.Entity<TLNodeHitoryStatus>()
                .HasRequired(c => c.TLNodeStatus)
                .WithMany(d => d.TLNodeHitoryStatuses)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TLSignalPlan>()
                .HasRequired(c => c.ReasonChangeSP)
                .WithMany(d => d.TSignalPlans)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InformationVMS>()
                .HasKey(ph => new {ph.VMSID, ph.InformationID});
            modelBuilder.Entity<InformationVMS>()
                .HasRequired(ph => ph.VMS)
                .WithMany(ph => ph.Informations)
                .HasForeignKey(ph => ph.VMSID);
            modelBuilder.Entity<InformationVMS>()
                .HasRequired(ph => ph.Information)
                .WithMany(ph => ph.VMSs)
                .HasForeignKey(ph => ph.InformationID);

            modelBuilder.Entity<AttachFile>()
                .HasKey(e => e.VMSEventID);
            modelBuilder.Entity<VMSEvent>()
                .HasOptional(s => s.AttachFile)
                .WithOptionalPrincipal(ad => ad.VMSEvent);

            modelBuilder.Entity<CCTV>()
             .HasMany(c => c.Zones).WithMany(i => i.CCTVs)
             .Map(t => t.MapLeftKey("CCTVID")
                 .MapRightKey("ZoneID")
                 .ToTable("CCTVZone"));

            //modelBuilder.Entity<CCTV>()
            //    .HasRequired(c => c.CAMType)
            //    .WithMany(d => d.CCTVs)
            //    .WillCascadeOnDelete(false);

      

        }
    }
}