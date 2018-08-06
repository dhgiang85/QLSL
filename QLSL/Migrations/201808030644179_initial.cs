namespace QLSL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccidentType",
                c => new
                    {
                        AccidentTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AccidentTypeID);
            
            CreateTable(
                "dbo.TAccident",
                c => new
                    {
                        TAccidentID = c.Int(nullable: false, identity: true),
                        ContactName = c.String(nullable: false, maxLength: 200),
                        OperatorName = c.String(maxLength: 50),
                        Details = c.String(nullable: false),
                        Note = c.String(),
                        AccidentTypeID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TAccidentID)
                .ForeignKey("dbo.AccidentType", t => t.AccidentTypeID, cascadeDelete: true)
                .Index(t => t.AccidentTypeID);
            
            CreateTable(
                "dbo.AttachFile",
                c => new
                    {
                        VMSEventID = c.Int(nullable: false),
                        ID = c.Guid(nullable: false),
                        Extension = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.VMSEventID)
                .ForeignKey("dbo.VMSEvent", t => t.VMSEventID)
                .Index(t => t.VMSEventID);
            
            CreateTable(
                "dbo.VMSEvent",
                c => new
                    {
                        VMSEventID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 300),
                        IsAlways = c.Boolean(nullable: false),
                        Uploaded = c.Boolean(nullable: false),
                        Unloaded = c.Boolean(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VMSEventID);
            
            CreateTable(
                "dbo.CAMType",
                c => new
                    {
                        CAMTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.CAMTypeID);
            
            CreateTable(
                "dbo.CCTV",
                c => new
                    {
                        CCTVID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Address = c.String(),
                        IP = c.String(maxLength: 15),
                        Manufacturer = c.String(maxLength: 30),
                        Model = c.String(maxLength: 60),
                        YearInstall = c.Int(nullable: false),
                        Disable = c.Boolean(nullable: false),
                        Note = c.String(maxLength: 200),
                        Map = c.Boolean(nullable: false),
                        Lat = c.Decimal(precision: 11, scale: 6),
                        Lng = c.Decimal(precision: 11, scale: 6),
                        OwerCCTVID = c.Int(),
                        LocatyCCTVID = c.Int(),
                        CAMTypeID = c.Int(),
                    })
                .PrimaryKey(t => t.CCTVID)
                .ForeignKey("dbo.CAMType", t => t.CAMTypeID)
                .ForeignKey("dbo.OwerCCTV", t => t.LocatyCCTVID)
                .ForeignKey("dbo.OwerCCTV", t => t.OwerCCTVID)
                .Index(t => t.OwerCCTVID)
                .Index(t => t.LocatyCCTVID)
                .Index(t => t.CAMTypeID);
            
            CreateTable(
                "dbo.CCTVStatus",
                c => new
                    {
                        CCTVStatusID = c.Int(nullable: false, identity: true),
                        ContactName = c.String(maxLength: 200),
                        OperatorName = c.String(maxLength: 50),
                        Details = c.String(),
                        Processed = c.Boolean(nullable: false),
                        CCTVID = c.Int(nullable: false),
                        CCTVErrorID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CCTVStatusID)
                .ForeignKey("dbo.CCTV", t => t.CCTVID, cascadeDelete: true)
                .ForeignKey("dbo.CCTVError", t => t.CCTVErrorID, cascadeDelete: true)
                .Index(t => t.CCTVID)
                .Index(t => t.CCTVErrorID);
            
            CreateTable(
                "dbo.CCTVError",
                c => new
                    {
                        CCTVErrorID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.CCTVErrorID);
            
            CreateTable(
                "dbo.OwerCCTV",
                c => new
                    {
                        OwerCCTVID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.OwerCCTVID);
            
            CreateTable(
                "dbo.Zone",
                c => new
                    {
                        ZoneID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.ZoneID);
            
            CreateTable(
                "dbo.Information",
                c => new
                    {
                        InformationID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Note = c.String(),
                        OperatorName = c.String(maxLength: 50),
                        InformationSourceID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InformationID)
                .ForeignKey("dbo.InformationSource", t => t.InformationSourceID, cascadeDelete: true)
                .Index(t => t.InformationSourceID);
            
            CreateTable(
                "dbo.InformationSource",
                c => new
                    {
                        InformationSourceID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.InformationSourceID);
            
            CreateTable(
                "dbo.InformationVMS",
                c => new
                    {
                        InformationID = c.Int(nullable: false),
                        VMSID = c.Int(nullable: false),
                        Success = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.InformationID, t.VMSID })
                .ForeignKey("dbo.Information", t => t.InformationID, cascadeDelete: true)
                .ForeignKey("dbo.VMS", t => t.VMSID, cascadeDelete: true)
                .Index(t => t.InformationID)
                .Index(t => t.VMSID);
            
            CreateTable(
                "dbo.VMS",
                c => new
                    {
                        VMSID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        IP = c.String(maxLength: 15),
                        Disable = c.Boolean(nullable: false),
                        Map = c.Boolean(nullable: false),
                        Note = c.String(maxLength: 200),
                        Lat = c.Decimal(nullable: false, precision: 11, scale: 6),
                        Lng = c.Decimal(nullable: false, precision: 11, scale: 6),
                    })
                .PrimaryKey(t => t.VMSID);
            
            CreateTable(
                "dbo.VMSHistoryStatus",
                c => new
                    {
                        VMSHistoryStatusID = c.Int(nullable: false, identity: true),
                        ContactName = c.String(nullable: false, maxLength: 200),
                        OperatorName = c.String(maxLength: 50),
                        Details = c.String(),
                        Note = c.String(),
                        Processed = c.Boolean(nullable: false),
                        VMSID = c.Int(nullable: false),
                        VMSStatusID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VMSHistoryStatusID)
                .ForeignKey("dbo.VMS", t => t.VMSID, cascadeDelete: true)
                .ForeignKey("dbo.VMSStatus", t => t.VMSStatusID, cascadeDelete: true)
                .Index(t => t.VMSID)
                .Index(t => t.VMSStatusID);
            
            CreateTable(
                "dbo.VMSStatus",
                c => new
                    {
                        VMSStatusID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.VMSStatusID);
            
            CreateTable(
                "dbo.PrimaryW",
                c => new
                    {
                        PrimaryWID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Note = c.String(maxLength: 200),
                        Disable = c.Boolean(nullable: false),
                        Lat = c.Decimal(precision: 11, scale: 6),
                        Lng = c.Decimal(precision: 11, scale: 6),
                    })
                .PrimaryKey(t => t.PrimaryWID);
            
            CreateTable(
                "dbo.PrimaryWStatus",
                c => new
                    {
                        PrimaryWStatusID = c.Int(nullable: false, identity: true),
                        ContactName = c.String(maxLength: 200),
                        OperatorName = c.String(maxLength: 50),
                        Details = c.String(),
                        Processed = c.Boolean(nullable: false),
                        PrimaryWID = c.Int(nullable: false),
                        PrimaryWErrorID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PrimaryWStatusID)
                .ForeignKey("dbo.PrimaryW", t => t.PrimaryWID, cascadeDelete: true)
                .ForeignKey("dbo.PrimaryWError", t => t.PrimaryWErrorID, cascadeDelete: true)
                .Index(t => t.PrimaryWID)
                .Index(t => t.PrimaryWErrorID);
            
            CreateTable(
                "dbo.PrimaryWError",
                c => new
                    {
                        PrimaryWErrorID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.PrimaryWErrorID);
            
            CreateTable(
                "dbo.ReasonChangeSP",
                c => new
                    {
                        ReasonChangeSPID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ReasonChangeSPID);
            
            CreateTable(
                "dbo.TLSignalPlan",
                c => new
                    {
                        TLSignalPlanID = c.Int(nullable: false, identity: true),
                        SignalPlanCurrent = c.String(nullable: false),
                        SignalPlanChanged = c.String(nullable: false),
                        OperatorName = c.String(nullable: false, maxLength: 50),
                        Note = c.String(),
                        ReasonChangeSPID = c.Int(nullable: false),
                        Unforced = c.Boolean(nullable: false),
                        Changed = c.Boolean(nullable: false),
                        TLNodeID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TLSignalPlanID)
                .ForeignKey("dbo.ReasonChangeSP", t => t.ReasonChangeSPID)
                .ForeignKey("dbo.TLNode", t => t.TLNodeID, cascadeDelete: true)
                .Index(t => t.ReasonChangeSPID)
                .Index(t => t.TLNodeID);
            
            CreateTable(
                "dbo.TLNode",
                c => new
                    {
                        TLNodeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IP = c.String(maxLength: 15),
                        Disable = c.Boolean(nullable: false),
                        Map = c.Boolean(nullable: false),
                        Note = c.String(maxLength: 200),
                        LabelMarker = c.String(),
                        Lat = c.Decimal(nullable: false, precision: 11, scale: 6),
                        Lng = c.Decimal(nullable: false, precision: 11, scale: 6),
                    })
                .PrimaryKey(t => t.TLNodeID);
            
            CreateTable(
                "dbo.TLNodeHitoryStatus",
                c => new
                    {
                        TLNodeHitoryStatusID = c.Int(nullable: false, identity: true),
                        ContactName = c.String(maxLength: 200),
                        OperatorName = c.String(nullable: false, maxLength: 50),
                        Details = c.String(),
                        Processed = c.Boolean(nullable: false),
                        TLNodeID = c.Int(nullable: false),
                        TLNodeStatusID = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TLNodeHitoryStatusID)
                .ForeignKey("dbo.TLNode", t => t.TLNodeID, cascadeDelete: true)
                .ForeignKey("dbo.TLNodeStatus", t => t.TLNodeStatusID)
                .Index(t => t.TLNodeID)
                .Index(t => t.TLNodeStatusID);
            
            CreateTable(
                "dbo.TLNodeStatus",
                c => new
                    {
                        TLNodeStatusID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TLNodeStatusID);
            
            CreateTable(
                "dbo.TunnelError",
                c => new
                    {
                        TunnelErrorID = c.Int(nullable: false, identity: true),
                        Details = c.String(maxLength: 255),
                        Note = c.String(maxLength: 128),
                        OperatorName = c.String(nullable: false, maxLength: 128),
                        ContactName = c.String(maxLength: 128),
                        Maintenancer = c.String(maxLength: 128),
                        Measures = c.String(maxLength: 255),
                        Processed = c.Boolean(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateUpdate = c.DateTime(nullable: false),
                        DateOccur = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TunnelErrorID);
            
            CreateTable(
                "dbo.CCTVZone",
                c => new
                    {
                        CCTVID = c.Int(nullable: false),
                        ZoneID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CCTVID, t.ZoneID })
                .ForeignKey("dbo.CCTV", t => t.CCTVID, cascadeDelete: true)
                .ForeignKey("dbo.Zone", t => t.ZoneID, cascadeDelete: true)
                .Index(t => t.CCTVID)
                .Index(t => t.ZoneID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TLSignalPlan", "TLNodeID", "dbo.TLNode");
            DropForeignKey("dbo.TLNodeHitoryStatus", "TLNodeStatusID", "dbo.TLNodeStatus");
            DropForeignKey("dbo.TLNodeHitoryStatus", "TLNodeID", "dbo.TLNode");
            DropForeignKey("dbo.TLSignalPlan", "ReasonChangeSPID", "dbo.ReasonChangeSP");
            DropForeignKey("dbo.PrimaryWStatus", "PrimaryWErrorID", "dbo.PrimaryWError");
            DropForeignKey("dbo.PrimaryWStatus", "PrimaryWID", "dbo.PrimaryW");
            DropForeignKey("dbo.InformationVMS", "VMSID", "dbo.VMS");
            DropForeignKey("dbo.VMSHistoryStatus", "VMSStatusID", "dbo.VMSStatus");
            DropForeignKey("dbo.VMSHistoryStatus", "VMSID", "dbo.VMS");
            DropForeignKey("dbo.InformationVMS", "InformationID", "dbo.Information");
            DropForeignKey("dbo.Information", "InformationSourceID", "dbo.InformationSource");
            DropForeignKey("dbo.CCTVZone", "ZoneID", "dbo.Zone");
            DropForeignKey("dbo.CCTVZone", "CCTVID", "dbo.CCTV");
            DropForeignKey("dbo.CCTV", "OwerCCTVID", "dbo.OwerCCTV");
            DropForeignKey("dbo.CCTV", "LocatyCCTVID", "dbo.OwerCCTV");
            DropForeignKey("dbo.CCTVStatus", "CCTVErrorID", "dbo.CCTVError");
            DropForeignKey("dbo.CCTVStatus", "CCTVID", "dbo.CCTV");
            DropForeignKey("dbo.CCTV", "CAMTypeID", "dbo.CAMType");
            DropForeignKey("dbo.AttachFile", "VMSEventID", "dbo.VMSEvent");
            DropForeignKey("dbo.TAccident", "AccidentTypeID", "dbo.AccidentType");
            DropIndex("dbo.CCTVZone", new[] { "ZoneID" });
            DropIndex("dbo.CCTVZone", new[] { "CCTVID" });
            DropIndex("dbo.TLNodeHitoryStatus", new[] { "TLNodeStatusID" });
            DropIndex("dbo.TLNodeHitoryStatus", new[] { "TLNodeID" });
            DropIndex("dbo.TLSignalPlan", new[] { "TLNodeID" });
            DropIndex("dbo.TLSignalPlan", new[] { "ReasonChangeSPID" });
            DropIndex("dbo.PrimaryWStatus", new[] { "PrimaryWErrorID" });
            DropIndex("dbo.PrimaryWStatus", new[] { "PrimaryWID" });
            DropIndex("dbo.VMSHistoryStatus", new[] { "VMSStatusID" });
            DropIndex("dbo.VMSHistoryStatus", new[] { "VMSID" });
            DropIndex("dbo.InformationVMS", new[] { "VMSID" });
            DropIndex("dbo.InformationVMS", new[] { "InformationID" });
            DropIndex("dbo.Information", new[] { "InformationSourceID" });
            DropIndex("dbo.CCTVStatus", new[] { "CCTVErrorID" });
            DropIndex("dbo.CCTVStatus", new[] { "CCTVID" });
            DropIndex("dbo.CCTV", new[] { "CAMTypeID" });
            DropIndex("dbo.CCTV", new[] { "LocatyCCTVID" });
            DropIndex("dbo.CCTV", new[] { "OwerCCTVID" });
            DropIndex("dbo.AttachFile", new[] { "VMSEventID" });
            DropIndex("dbo.TAccident", new[] { "AccidentTypeID" });
            DropTable("dbo.CCTVZone");
            DropTable("dbo.TunnelError");
            DropTable("dbo.TLNodeStatus");
            DropTable("dbo.TLNodeHitoryStatus");
            DropTable("dbo.TLNode");
            DropTable("dbo.TLSignalPlan");
            DropTable("dbo.ReasonChangeSP");
            DropTable("dbo.PrimaryWError");
            DropTable("dbo.PrimaryWStatus");
            DropTable("dbo.PrimaryW");
            DropTable("dbo.VMSStatus");
            DropTable("dbo.VMSHistoryStatus");
            DropTable("dbo.VMS");
            DropTable("dbo.InformationVMS");
            DropTable("dbo.InformationSource");
            DropTable("dbo.Information");
            DropTable("dbo.Zone");
            DropTable("dbo.OwerCCTV");
            DropTable("dbo.CCTVError");
            DropTable("dbo.CCTVStatus");
            DropTable("dbo.CCTV");
            DropTable("dbo.CAMType");
            DropTable("dbo.VMSEvent");
            DropTable("dbo.AttachFile");
            DropTable("dbo.TAccident");
            DropTable("dbo.AccidentType");
        }
    }
}
