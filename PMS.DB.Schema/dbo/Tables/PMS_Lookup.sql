CREATE TABLE [dbo].[PMS_Lookup] (
    [Lookup_Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Lookup_Type]   NVARCHAR (50) NOT NULL,
    [Hidden_Value]  NVARCHAR (50) NOT NULL,
    [Visible_Value] NVARCHAR (50) NOT NULL,
    [Active_Flag]   BIT           DEFAULT ((1)) NOT NULL,
    [Created_by]    NVARCHAR (50) NOT NULL,
    [Created_Date]  DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Lookup_Id] ASC)
);

