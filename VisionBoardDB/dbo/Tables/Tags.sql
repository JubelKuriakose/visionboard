CREATE TABLE [dbo].[Tags] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    [Colour] VARCHAR (50) NULL,
    [Status] BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

