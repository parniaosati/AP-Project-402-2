CREATE TABLE [dbo].[SpecialServices] (
    [service_id] INT IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR(50) NOT NULL,
    [price] DECIMAL(10, 2) NOT NULL,
    [reservations_per_month] INT NOT NULL,
    [reservation_time_limit] INT NOT NULL, -- in minutes
    [cancellation_threshold] INT NOT NULL, -- in minutes
    PRIMARY KEY CLUSTERED ([service_id] ASC)
);
