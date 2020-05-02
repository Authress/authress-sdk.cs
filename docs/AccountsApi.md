# Authress.SDK.Api.AccountsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**V1AccountsAccountIdGet**](AccountsApi.md#v1accountsaccountidget) | **GET** /v1/accounts/{accountId} | Get account information.
[**V1AccountsGet**](AccountsApi.md#v1accountsget) | **GET** /v1/accounts | Get all accounts user has access to
[**V1IdentitiesGet**](AccountsApi.md#v1identitiesget) | **GET** /v1/identities | Get all linked identities for this account.
[**V1IdentitiesPost**](AccountsApi.md#v1identitiespost) | **POST** /v1/identities | Link a new account identity.

<a name="v1accountsaccountidget"></a>
# **V1AccountsAccountIdGet**
> Account V1AccountsAccountIdGet (string accountId)

Get account information.

Includes the original configuration information.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1AccountsAccountIdGetExample
    {
        public void main()
        {


            var apiInstance = new AccountsApi();
            var accountId = accountId_example;  // string | The unique identifier for the account

            try
            {
                // Get account information.
                Account result = apiInstance.V1AccountsAccountIdGet(accountId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountsApi.V1AccountsAccountIdGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **accountId** | **string**| The unique identifier for the account |

### Return type

[**Account**](Account.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1accountsget"></a>
# **V1AccountsGet**
> AccountCollection V1AccountsGet ()

Get all accounts user has access to

Returns a list of accounts that the user has access to.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1AccountsGetExample
    {
        public void main()
        {


            var apiInstance = new AccountsApi();

            try
            {
                // Get all accounts user has access to
                AccountCollection result = apiInstance.V1AccountsGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountsApi.V1AccountsGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**AccountCollection**](AccountCollection.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1identitiesget"></a>
# **V1IdentitiesGet**
> IdentityCollection V1IdentitiesGet ()

Get all linked identities for this account.

Returns a list of identities linked for this account.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1IdentitiesGetExample
    {
        public void main()
        {


            var apiInstance = new AccountsApi();

            try
            {
                // Get all linked identities for this account.
                IdentityCollection result = apiInstance.V1IdentitiesGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountsApi.V1IdentitiesGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**IdentityCollection**](IdentityCollection.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1identitiespost"></a>
# **V1IdentitiesPost**
> Object V1IdentitiesPost (IdentityRequest body)

Link a new account identity.

An identity is a JWT subscriber *sub* and issuer *iss*. Only one account my be linked to a particular JWT combination. Allows calling the API with a federated token directly instead of using a client access key.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1IdentitiesPostExample
    {
        public void main()
        {


            var apiInstance = new AccountsApi();
            var body = new IdentityRequest(); // IdentityRequest |

            try
            {
                // Link a new account identity.
                Object result = apiInstance.V1IdentitiesPost(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccountsApi.V1IdentitiesPost: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**IdentityRequest**](IdentityRequest.md)|  |

### Return type

**Object**

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

