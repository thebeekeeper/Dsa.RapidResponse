--select Id from AspNetUsers;

insert into AspNetUserClaims (Id, ClaimType, ClaimValue, UserId) 
    values (0, 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 
        'Administrator',
        '34e8bf35-d59d-4dc1-a567-b42f07cda409');