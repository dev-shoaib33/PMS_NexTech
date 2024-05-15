CREATE TABLE [dbo].[PMS_Product] (
    [ProductId]         INT            IDENTITY (1, 1) NOT NULL,
    [Product_Name]      NVARCHAR (255) NOT NULL,
    [Product_Code]      NVARCHAR (50)  NOT NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [Price]             FLOAT (53)     NOT NULL,
    [Category_lkp_Id]   INT            NOT NULL,
    [Image_Name]        NVARCHAR (255) NULL,
    [Uom]               NVARCHAR (50)  NULL,
    [Created_By]        NVARCHAR (50)  NOT NULL,
    [Created_Date]      DATETIME       NOT NULL,
    [Last_Updated_By]   NVARCHAR (50)  NOT NULL,
    [Last_Updated_Date] DATETIME       NOT NULL,
    [Active_Flag]       BIT            DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC),
    FOREIGN KEY ([Category_lkp_Id]) REFERENCES [dbo].[PMS_Lookup] ([Lookup_Id])
);

