namespace BuyalotV1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddressModel",
                c => new
                    {
                        addressID = c.Int(nullable: false, identity: true),
                        customerID = c.Int(nullable: false),
                        address = c.String(),
                        city = c.String(),
                        postalCode = c.String(),
                    })
                .PrimaryKey(t => t.addressID)
                .ForeignKey("dbo.CustomerModel", t => t.customerID, cascadeDelete: true)
                .Index(t => t.customerID);
            
            CreateTable(
                "dbo.CustomerModel",
                c => new
                    {
                        customerID = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        phone = c.String(),
                        email = c.String(),
                        state = c.String(),
                        password = c.String(),
                        confirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.customerID);
            
            CreateTable(
                "dbo.BillingModel",
                c => new
                    {
                        billingID = c.Int(nullable: false, identity: true),
                        customerID = c.Int(nullable: false),
                        cardNumber = c.String(),
                        cardType = c.String(),
                        cvv = c.Int(nullable: false),
                        expDate = c.DateTime(nullable: false),
                        cardHolderName = c.String(),
                    })
                .PrimaryKey(t => t.billingID)
                .ForeignKey("dbo.CustomerModel", t => t.customerID, cascadeDelete: true)
                .Index(t => t.customerID);
            
            CreateTable(
                "dbo.OrderModel",
                c => new
                    {
                        orderID = c.Int(nullable: false, identity: true),
                        customerID = c.Int(nullable: false),
                        orderDate = c.DateTime(nullable: false),
                        shippingDate = c.DateTime(nullable: false),
                        shippingAddress = c.String(),
                        status = c.String(),
                        totalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.orderID)
                .ForeignKey("dbo.CustomerModel", t => t.customerID, cascadeDelete: true)
                .Index(t => t.customerID);
            
            CreateTable(
                "dbo.OrderDetailModel",
                c => new
                    {
                        orderDetailsID = c.Int(nullable: false, identity: true),
                        orderID = c.Int(nullable: false),
                        productID = c.Int(nullable: false),
                        quantityOrdered = c.Int(nullable: false),
                        priceEach = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.orderDetailsID)
                .ForeignKey("dbo.OrderModel", t => t.orderID, cascadeDelete: true)
                .ForeignKey("dbo.ProductModel", t => t.productID, cascadeDelete: true)
                .Index(t => t.orderID)
                .Index(t => t.productID);
            
            CreateTable(
                "dbo.ProductModel",
                c => new
                    {
                        productID = c.Int(nullable: false, identity: true),
                        productName = c.String(nullable: false, maxLength: 50),
                        productDescription = c.String(maxLength: 255),
                        prodCategoryID = c.Int(nullable: false),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        vendor = c.String(nullable: false),
                        quantityInStock = c.Int(nullable: false),
                        productImage = c.Binary(),
                        ProductCategoryModel_prodCategoryID = c.Int(),
                        ProductCategory_prodCategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.productID)
                .ForeignKey("dbo.ProductCategoryModel", t => t.ProductCategoryModel_prodCategoryID)
                .ForeignKey("dbo.ProductCategoryModel", t => t.ProductCategory_prodCategoryID)
                .Index(t => t.ProductCategoryModel_prodCategoryID)
                .Index(t => t.ProductCategory_prodCategoryID);
            
            CreateTable(
                "dbo.ProductCategoryModel",
                c => new
                    {
                        prodCategoryID = c.Int(nullable: false, identity: true),
                        categoryName = c.String(nullable: false, maxLength: 50),
                        ProductModel_productID = c.Int(),
                    })
                .PrimaryKey(t => t.prodCategoryID)
                .ForeignKey("dbo.ProductModel", t => t.ProductModel_productID)
                .Index(t => t.ProductModel_productID);
            
            CreateTable(
                "dbo.PaymentModel",
                c => new
                    {
                        paymentID = c.Int(nullable: false, identity: true),
                        paymentDate = c.DateTime(nullable: false),
                        customerID = c.Int(nullable: false),
                        orderID = c.Int(nullable: false),
                        paymentType = c.String(),
                        totalPrice = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.paymentID)
                .ForeignKey("dbo.CustomerModel", t => t.customerID, cascadeDelete: true)
                .ForeignKey("dbo.OrderModel", t => t.orderID, cascadeDelete: true)
                .Index(t => t.customerID)
                .Index(t => t.orderID);
            
            CreateTable(
                "dbo.AdminModel",
                c => new
                    {
                        adminID = c.Int(nullable: false, identity: true),
                        adminName = c.String(),
                        email = c.String(),
                        password = c.String(),
                        confirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.adminID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentModel", "orderID", "dbo.OrderModel");
            DropForeignKey("dbo.PaymentModel", "customerID", "dbo.CustomerModel");
            DropForeignKey("dbo.ProductModel", "ProductCategory_prodCategoryID", "dbo.ProductCategoryModel");
            DropForeignKey("dbo.OrderDetailModel", "productID", "dbo.ProductModel");
            DropForeignKey("dbo.ProductCategoryModel", "ProductModel_productID", "dbo.ProductModel");
            DropForeignKey("dbo.ProductModel", "ProductCategoryModel_prodCategoryID", "dbo.ProductCategoryModel");
            DropForeignKey("dbo.OrderDetailModel", "orderID", "dbo.OrderModel");
            DropForeignKey("dbo.OrderModel", "customerID", "dbo.CustomerModel");
            DropForeignKey("dbo.BillingModel", "customerID", "dbo.CustomerModel");
            DropForeignKey("dbo.AddressModel", "customerID", "dbo.CustomerModel");
            DropIndex("dbo.PaymentModel", new[] { "orderID" });
            DropIndex("dbo.PaymentModel", new[] { "customerID" });
            DropIndex("dbo.ProductCategoryModel", new[] { "ProductModel_productID" });
            DropIndex("dbo.ProductModel", new[] { "ProductCategory_prodCategoryID" });
            DropIndex("dbo.ProductModel", new[] { "ProductCategoryModel_prodCategoryID" });
            DropIndex("dbo.OrderDetailModel", new[] { "productID" });
            DropIndex("dbo.OrderDetailModel", new[] { "orderID" });
            DropIndex("dbo.OrderModel", new[] { "customerID" });
            DropIndex("dbo.BillingModel", new[] { "customerID" });
            DropIndex("dbo.AddressModel", new[] { "customerID" });
            DropTable("dbo.AdminModel");
            DropTable("dbo.PaymentModel");
            DropTable("dbo.ProductCategoryModel");
            DropTable("dbo.ProductModel");
            DropTable("dbo.OrderDetailModel");
            DropTable("dbo.OrderModel");
            DropTable("dbo.BillingModel");
            DropTable("dbo.CustomerModel");
            DropTable("dbo.AddressModel");
        }
    }
}
