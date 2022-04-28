CREATE TABLE [dbo].[Steps] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Description] VARCHAR (600) NULL,
    [Weight]      INT           NOT NULL,
    [DueDate]     DATETIME      NULL,
    [Status]      BIT           NOT NULL,
    [GoalId]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([GoalId]) REFERENCES [dbo].[Goals] ([Id])
);

