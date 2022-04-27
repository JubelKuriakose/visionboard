CREATE TABLE [dbo].[Goals] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)    NOT NULL,
    [Description]   VARCHAR (600)   NULL,
    [StartOn]       DATETIME        NOT NULL,
    [EndingOn]      DATETIME        NULL,
    [Magnitude]     INT             NULL,
    [PictureUrl]    NVARCHAR (2000) NULL,
    [RewardId]      INT             NULL,
    [Status]        BIT             NOT NULL,
    [MeasurementId] INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Goals_Mesurement] FOREIGN KEY ([MeasurementId]) REFERENCES [dbo].[Mesurement] ([Id]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_Goals_Reward] FOREIGN KEY ([RewardId]) REFERENCES [dbo].[Reward] ([Id]) ON DELETE SET NULL ON UPDATE SET NULL
);

