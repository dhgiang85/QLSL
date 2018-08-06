using System;
using QLSL.Models;

namespace QLSL.DAL
{
    public class UnitOfWork : IDisposable
    {
        private QLSLContext context = new QLSLContext();

        private GenericRepository<TLNode> tLNodeRepository;
        public GenericRepository<TLNode> TLNodeRepository
        {
            get
            {

                if (this.tLNodeRepository == null)
                {
                    this.tLNodeRepository = new GenericRepository<TLNode>(context);
                }
                return tLNodeRepository;
            }
        }

        private GenericRepository<TLNodeHitoryStatus> tLNodeHitoryStatusRepository;
        public GenericRepository<TLNodeHitoryStatus> TLNodeHitoryStatusRepository
        {
            get
            {

                if (this.tLNodeHitoryStatusRepository == null)
                {
                    this.tLNodeHitoryStatusRepository = new GenericRepository<TLNodeHitoryStatus>(context);
                }
                return tLNodeHitoryStatusRepository;
            }
        }

        private GenericRepository<TLNodeStatus> tLNodeStatusRepository;
        public GenericRepository<TLNodeStatus> TLNodeStatusRepository
        {
            get
            {

                if (this.tLNodeStatusRepository == null)
                {
                    this.tLNodeStatusRepository = new GenericRepository<TLNodeStatus>(context);
                }
                return tLNodeStatusRepository;
            }
        }

        private GenericRepository<TLSignalPlan> tLSignalPlanRepository;
        public GenericRepository<TLSignalPlan> TLSignalPlanRepository
        {
            get
            {

                if (this.tLSignalPlanRepository == null)
                {
                    this.tLSignalPlanRepository = new GenericRepository<TLSignalPlan>(context);
                }
                return tLSignalPlanRepository;
            }
        }

        private GenericRepository<ReasonChangeSP> reasonChangeSPRepository;
        public GenericRepository<ReasonChangeSP> ReasonChangeSPRepository
        {
            get
            {

                if (this.reasonChangeSPRepository == null)
                {
                    this.reasonChangeSPRepository = new GenericRepository<ReasonChangeSP>(context);
                }
                return reasonChangeSPRepository;
            }
        }

        private GenericRepository<TAccident> tAccidentRepository;
        public GenericRepository<TAccident> TAccidentRepository
        {
            get
            {

                if (this.tAccidentRepository == null)
                {
                    this.tAccidentRepository = new GenericRepository<TAccident>(context);
                }
                return tAccidentRepository;
            }
        }

        private GenericRepository<AccidentType> accidentTypeRepository;
        public GenericRepository<AccidentType> AccidentTypeRepository
        {
            get
            {

                if (this.accidentTypeRepository == null)
                {
                    this.accidentTypeRepository = new GenericRepository<AccidentType>(context);
                }
                return accidentTypeRepository;
            }
        }

        private GenericRepository<VMS> vMSRepository;
        public GenericRepository<VMS> VMSRepository
        {
            get
            {

                if (this.vMSRepository == null)
                {
                    this.vMSRepository = new GenericRepository<VMS>(context);
                }
                return vMSRepository;
            }
        }

        private GenericRepository<VMSStatus> vMSStatusRepository;
        public GenericRepository<VMSStatus> VMSStatusRepository
        {
            get
            {

                if (this.vMSStatusRepository == null)
                {
                    this.vMSStatusRepository = new GenericRepository<VMSStatus>(context);
                }
                return vMSStatusRepository;
            }
        }

        private GenericRepository<VMSHistoryStatus> vMSHistoryStatusRepository;
        public GenericRepository<VMSHistoryStatus> VMSHistoryStatusRepository
        {
            get
            {

                if (this.vMSHistoryStatusRepository == null)
                {
                    this.vMSHistoryStatusRepository = new GenericRepository<VMSHistoryStatus>(context);
                }
                return vMSHistoryStatusRepository;
            }
        }
        private GenericRepository<Information> informationRepository;
        public GenericRepository<Information> InformationRepository
        {
            get
            {

                if (this.informationRepository == null)
                {
                    this.informationRepository = new GenericRepository<Information>(context);
                }
                return informationRepository;
            }
        }

        private GenericRepository<InformationSource> informationSourceRepository;
        public GenericRepository<InformationSource> InformationSourceRepository
        {
            get
            {

                if (this.informationSourceRepository == null)
                {
                    this.informationSourceRepository = new GenericRepository<InformationSource>(context);
                }
                return informationSourceRepository;
            }
        }
        private GenericRepository<InformationVMS> informationVMSRepository;
        public GenericRepository<InformationVMS> InformationVMSRepository
        {
            get
            {

                if (this.informationVMSRepository == null)
                {
                    this.informationVMSRepository = new GenericRepository<InformationVMS>(context);
                }
                return informationVMSRepository;
            }
        }
        private GenericRepository<VMSEvent> vMSEventRepository;
        public GenericRepository<VMSEvent> VMSEventRepository
        {
            get
            {

                if (this.vMSEventRepository == null)
                {
                    this.vMSEventRepository = new GenericRepository<VMSEvent>(context);
                }
                return vMSEventRepository;
            }
        }

        private GenericRepository<CCTV> cCTVRepository;
        public GenericRepository<CCTV> CCTVRepository
        {
            get
            {

                if (this.cCTVRepository == null)
                {
                    this.cCTVRepository = new GenericRepository<CCTV>(context);
                }
                return cCTVRepository;
            }
        }
        private GenericRepository<Zone> zoneRepository;
        public GenericRepository<Zone> ZoneRepository
        {
            get
            {

                if (this.zoneRepository == null)
                {
                    this.zoneRepository = new GenericRepository<Zone>(context);
                }
                return zoneRepository;
            }
        }
        private GenericRepository<OwerCCTV> owerCCTVRepository;
        public GenericRepository<OwerCCTV> OwerCCTVRepository
        {
            get
            {

                if (this.owerCCTVRepository == null)
                {
                    this.owerCCTVRepository = new GenericRepository<OwerCCTV>(context);
                }
                return owerCCTVRepository;
            }
        }

        private GenericRepository<CAMType> cAMTypeRepository;
        public GenericRepository<CAMType> CAMTypeRepository
        {
            get
            {

                if (this.cAMTypeRepository == null)
                {
                    this.cAMTypeRepository = new GenericRepository<CAMType>(context);
                }
                return cAMTypeRepository;
            }
        }

        

        private GenericRepository<CCTVStatus> cCTVStatusRepository;
        public GenericRepository<CCTVStatus> CCTVStatusRepository
        {
            get
            {

                if (this.cCTVStatusRepository == null)
                {
                    this.cCTVStatusRepository = new GenericRepository<CCTVStatus>(context);
                }
                return cCTVStatusRepository;
            }
        }
        private GenericRepository<CCTVError> cCTVErrorRepository;
        public GenericRepository<CCTVError> CCTVErrorRepository
        {
            get
            {

                if (this.cCTVErrorRepository == null)
                {
                    this.cCTVErrorRepository = new GenericRepository<CCTVError>(context);
                }
                return cCTVErrorRepository;
            }
        }
        private GenericRepository<TunnelError> tunnelErrorRepository;
        public GenericRepository<TunnelError> TunnelErrorRepository
        {
            get
            {

                if (this.tunnelErrorRepository == null)
                {
                    this.tunnelErrorRepository = new GenericRepository<TunnelError>(context);
                }
                return tunnelErrorRepository;
            }
        }

        private GenericRepository<PrimaryW> primaryWRepository;
        public GenericRepository<PrimaryW> PrimaryWRepository
        {
            get
            {

                if (this.primaryWRepository == null)
                {
                    this.primaryWRepository = new GenericRepository<PrimaryW>(context);
                }
                return primaryWRepository;
            }
        }

        private GenericRepository<PrimaryWError> primaryWErrorRepository;
        public GenericRepository<PrimaryWError> PrimaryWErrorRepository
        {
            get
            {

                if (this.primaryWErrorRepository == null)
                {
                    this.primaryWErrorRepository = new GenericRepository<PrimaryWError>(context);
                }
                return primaryWErrorRepository;
            }
        }
        private GenericRepository<PrimaryWStatus> primaryWStatusRepository;
        public GenericRepository<PrimaryWStatus> PrimaryWStatusRepository
        {
            get
            {

                if (this.primaryWStatusRepository == null)
                {
                    this.primaryWStatusRepository = new GenericRepository<PrimaryWStatus>(context);
                }
                return primaryWStatusRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}