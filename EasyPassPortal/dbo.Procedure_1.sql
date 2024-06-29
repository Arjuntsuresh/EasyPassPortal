CREATE PROCEDURE AddNewAdminDetail
@Email VARCHAR (MAX),
@Password VARCHAR (MAX)
as 
begin 
INSERT INTO AdminDataTable VALUES  (@Email,@Password,1)
end