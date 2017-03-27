use hello;

insert into AspNetUserClaims (ClaimType, ClaimValue, UserId) 
    values ('http://schemas.microsoft.com/ws/2008/06/identity/claims/role', 
        'Administrator',
        (select Id from AspNetUsers where email='Admin@test.com'));

