CREATE TABLE [dbo].[Reward] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [PictureUrl]  NCHAR (2000)  NULL,
    [status]      BIT           NOT NULL,
    [Description] VARCHAR (600) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

