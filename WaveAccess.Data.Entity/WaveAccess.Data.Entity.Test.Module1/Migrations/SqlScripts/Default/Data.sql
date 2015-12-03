DELETE FROM [dbo].UserGroups
DELETE FROM [dbo].Users
DELETE FROM [dbo].Groups
GO

SET IDENTITY_INSERT [dbo].Users ON 
INSERT [dbo].Users (Id, [Login]) VALUES (1, N'User1')
INSERT [dbo].Users (Id, [Login]) VALUES (2, N'User2')
INSERT [dbo].Users (Id, [Login]) VALUES (3, N'User3')
INSERT [dbo].Users (Id, [Login]) VALUES (4, N'User4')
SET IDENTITY_INSERT [dbo].Users OFF

GO

SET IDENTITY_INSERT [dbo].Groups ON 
INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (1, N'Root1',null)
INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (2, N'Root2',null)

INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (3, N'Child 1-1',1)
INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (4, N'Child 1-2',1)
INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (5, N'Child 2-1',2)
INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (6, N'Child 2-2',2)

INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (7, N'Child 1-1-1',3)
INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (8, N'Child 2-2-1',6)

INSERT [dbo].Groups (Id, Name,ParentGroupId) VALUES (9, N'Child 2-2-1-1',8)
SET IDENTITY_INSERT [dbo].Groups OFF

GO 

INSERT [dbo].UserGroups (GroupId, UserId) VALUES (4,1)
INSERT [dbo].UserGroups (GroupId, UserId) VALUES (4,2)
INSERT [dbo].UserGroups (GroupId, UserId) VALUES (7,2)
INSERT [dbo].UserGroups (GroupId, UserId) VALUES (6,3)
INSERT [dbo].UserGroups (GroupId, UserId) VALUES (9,4)