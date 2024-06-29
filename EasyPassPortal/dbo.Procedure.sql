CREATE PROCEDURE AddNewAdminDetails
@Email VARCHAR (MAX),
@Password VARCHAR (MAX)
as 
begin 
INSERT INTO AdminDataTable VALUES  (@Email,@Password)
end
