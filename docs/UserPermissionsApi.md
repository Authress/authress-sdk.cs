# Authress.SDK.Api.UserPermissionsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**V1UsersUserIdResourcesResourceUriPermissionsGet**](UserPermissionsApi.md#v1usersuseridresourcesresourceuripermissionsget) | **GET** /v1/users/{userId}/resources/{resourceUri}/permissions | Get the permissions a user has to a resource.
[**V1UsersUserIdResourcesResourceUriPermissionsPermissionGet**](UserPermissionsApi.md#v1usersuseridresourcesresourceuripermissionspermissionget) | **GET** /v1/users/{userId}/resources/{resourceUri}/permissions/{permission} | Check to see if a user has permissions to a resource.

<a name="v1usersuseridresourcesresourceuripermissionsget"></a>
# **V1UsersUserIdResourcesResourceUriPermissionsGet**
> InlineResponse200 V1UsersUserIdResourcesResourceUriPermissionsGet (string userId, string resourceUri)

Get the permissions a user has to a resource.

Get a summary of the permissions a user has to a particular resource.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1UsersUserIdResourcesResourceUriPermissionsGetExample
    {
        public void main()
        {


            var apiInstance = new UserPermissionsApi();
            var userId = userId_example;  // string | The user to check permissions on
            var resourceUri = resourceUri_example;  // string | The uri path of a resource to validate, must be URL encoded, uri segments are allowed.

            try
            {
                // Get the permissions a user has to a resource.
                InlineResponse200 result = apiInstance.V1UsersUserIdResourcesResourceUriPermissionsGet(userId, resourceUri);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserPermissionsApi.V1UsersUserIdResourcesResourceUriPermissionsGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **string**| The user to check permissions on |
 **resourceUri** | **string**| The uri path of a resource to validate, must be URL encoded, uri segments are allowed. |

### Return type

[**InlineResponse200**](InlineResponse200.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1usersuseridresourcesresourceuripermissionspermissionget"></a>
# **V1UsersUserIdResourcesResourceUriPermissionsPermissionGet**
> Object V1UsersUserIdResourcesResourceUriPermissionsPermissionGet (string userId, string resourceUri, string permission)

Check to see if a user has permissions to a resource.

Does the user have the specified permissions to the resource?

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1UsersUserIdResourcesResourceUriPermissionsPermissionGetExample
    {
        public void main()
        {


            var apiInstance = new UserPermissionsApi();
            var userId = userId_example;  // string | The user to check permissions on
            var resourceUri = resourceUri_example;  // string | The uri path of a resource to validate, must be URL encoded, uri segments are allowed, the resource must be a full path, and permissions are not inhereted by sub resources.
            var permission = permission_example;  // string | Permission to check, '*' and scoped permissions can also be checked here.

            try
            {
                // Check to see if a user has permissions to a resource.
                Object result = apiInstance.V1UsersUserIdResourcesResourceUriPermissionsPermissionGet(userId, resourceUri, permission);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling UserPermissionsApi.V1UsersUserIdResourcesResourceUriPermissionsPermissionGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **userId** | **string**| The user to check permissions on |
 **resourceUri** | **string**| The uri path of a resource to validate, must be URL encoded, uri segments are allowed, the resource must be a full path, and permissions are not inhereted by sub resources. |
 **permission** | **string**| Permission to check, &#x27;*&#x27; and scoped permissions can also be checked here. |

### Return type

**Object**

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

