# ADuserManagement
Azure AD user management with Dotnet core 

## Steps to run code :

Step 1 : Open .sln folder in Visual Studio  
Step 2 : Change appsetting.json file with your AD account and APP registered Data.  
Step 3 : Build and try to Run the Code.  
Step 4 : You can use any API testing tool to check the api response.  

## Following are the API Endpoints to test 
1. **GET** : /api/user
2. **POST** : /api/user 
   ```
   body : {
    "AccountEnabled": true,
    "DisplayName": "test3",
    "MailNickname": "testNickName3",
    "PasswordProfile": {
        "Password": "AnyStrongPassword123",
        "ForceChangePasswordNextSignIn": false
    },
    "UserPrincipalName": "test_live3.com#EXT#@your_tenant_name.onmicrosoft.com",
    "GivenName": "test3",
    "PasswordPolicies": "DisablePasswordExpiration",
    "Surname": "TestSurname3"
    }
    ```
3. **Delete** : /api/user/{id}
