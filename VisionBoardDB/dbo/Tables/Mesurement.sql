CREATE TABLE [dbo].[Mesurement] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Type]         VARCHAR (20) NOT NULL,
    [CurrentValue] INT          NOT NULL,
    [TotalValue]   INT          NOT NULL,
    [Unit]         VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

