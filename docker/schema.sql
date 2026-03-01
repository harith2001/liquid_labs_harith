CREATE DATABASE Harith_Local;
GO

USE Harith_Local;
GO

CREATE TABLE Owners (
    Id INT IDENTITY PRIMARY KEY,
    GithubId BIGINT NOT NULL UNIQUE,
    Login NVARCHAR(200) NOT NULL,
    Url NVARCHAR(500),
    Type NVARCHAR(50)
);

CREATE TABLE Repositories (
    Id INT IDENTITY PRIMARY KEY,
    GithubId BIGINT NOT NULL UNIQUE,
    Name NVARCHAR(200) NOT NULL,
    FullName NVARCHAR(300) NOT NULL,
    Description NVARCHAR(MAX),
    StargazersCount INT,
    ForksCount INT,
    OpenIssuesCount INT,
    Language NVARCHAR(100),
    CreatedAt DATETIME2,
    UpdatedAt DATETIME2,
    OwnerId INT NOT NULL,
    InsertedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Repository_Owner FOREIGN KEY (OwnerId)
        REFERENCES Owners(Id)
);