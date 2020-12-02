namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cargoes",
                c => new
                    {
                        CargoId = c.Int(nullable: false, identity: true),
                        FreightEtsngName = c.String(),
                    })
                .PrimaryKey(t => t.CargoId);
            
            CreateTable(
                "dbo.Carriges",
                c => new
                    {
                        CarrigeId = c.Int(nullable: false, identity: true),
                        PositionInTrain = c.Int(nullable: false),
                        CarNumber = c.Int(nullable: false),
                        InvoiceNum = c.String(),
                        OperationId = c.Int(nullable: false),
                        CargoId = c.Int(nullable: false),
                        FreightTotalWeightKg = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarrigeId)
                .ForeignKey("dbo.Cargoes", t => t.CargoId, cascadeDelete: true)
                .ForeignKey("dbo.Operations", t => t.OperationId, cascadeDelete: true)
                .Index(t => t.OperationId)
                .Index(t => t.CargoId);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        OperationId = c.Int(nullable: false, identity: true),
                        WhenLastOperation = c.DateTime(nullable: false),
                        LastOperationName = c.String(),
                        LastStationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OperationId)
                .ForeignKey("dbo.Stations", t => t.LastStationId, cascadeDelete: true)
                .Index(t => t.LastStationId);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        StationId = c.Int(nullable: false, identity: true),
                        StationName = c.String(),
                    })
                .PrimaryKey(t => t.StationId);
            
            CreateTable(
                "dbo.Trains",
                c => new
                    {
                        TrainId = c.Int(nullable: false, identity: true),
                        TrainNumber = c.Int(nullable: false),
                        TrainIndex = c.Int(nullable: false),
                        TrainIndexCombined = c.String(),
                        FromStationId = c.Int(),
                        ToStationId = c.Int(),
                        CarrigeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrainId)
                .ForeignKey("dbo.Carriges", t => t.CarrigeId, cascadeDelete: true)
                .ForeignKey("dbo.Stations", t => t.FromStationId)
                .ForeignKey("dbo.Stations", t => t.ToStationId)
                .Index(t => t.FromStationId)
                .Index(t => t.ToStationId)
                .Index(t => t.CarrigeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trains", "ToStationId", "dbo.Stations");
            DropForeignKey("dbo.Trains", "FromStationId", "dbo.Stations");
            DropForeignKey("dbo.Trains", "CarrigeId", "dbo.Carriges");
            DropForeignKey("dbo.Carriges", "OperationId", "dbo.Operations");
            DropForeignKey("dbo.Operations", "LastStationId", "dbo.Stations");
            DropForeignKey("dbo.Carriges", "CargoId", "dbo.Cargoes");
            DropIndex("dbo.Trains", new[] { "CarrigeId" });
            DropIndex("dbo.Trains", new[] { "ToStationId" });
            DropIndex("dbo.Trains", new[] { "FromStationId" });
            DropIndex("dbo.Operations", new[] { "LastStationId" });
            DropIndex("dbo.Carriges", new[] { "CargoId" });
            DropIndex("dbo.Carriges", new[] { "OperationId" });
            DropTable("dbo.Trains");
            DropTable("dbo.Stations");
            DropTable("dbo.Operations");
            DropTable("dbo.Carriges");
            DropTable("dbo.Cargoes");
        }
    }
}
