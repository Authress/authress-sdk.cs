# Authress.SDK.Api.ResourcePermissionsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**V1ResourcesGet**](ResourcePermissionsApi.md#v1resourcesget) | **GET** /v1/resources | List resource configurations
[**V1ResourcesResourceUriGet**](ResourcePermissionsApi.md#v1resourcesresourceuriget) | **GET** /v1/resources/{resourceUri} | Get a resource permissions object.
[**V1ResourcesResourceUriPut**](ResourcePermissionsApi.md#v1resourcesresourceuriput) | **PUT** /v1/resources/{resourceUri} | Update a resource permissions object.

<a name="v1resourcesget"></a>
# **V1ResourcesGet**
> ResourcePermissionCollection V1ResourcesGet ()

List resource configurations

Permissions can be set globally at a resource level. Lists any resources with a globally set resource policy

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ResourcesGetExample
    {
        public void main()
        {


            var apiInstance = new ResourcePermissionsApi();

            try
            {
                // List resource configurations
                ResourcePermissionCollection result = apiInstance.V1ResourcesGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ResourcePermissionsApi.V1ResourcesGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**ResourcePermissionCollection**](ResourcePermissionCollection.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1resourcesresourceuriget"></a>
# **V1ResourcesResourceUriGet**
> ResourcePermission V1ResourcesResourceUriGet (string resourceUri)

Get a resource permissions object.

Permissions can be set globally at a resource level. This will apply to all users in an account.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ResourcesResourceUriGetExample
    {
        public void main()
        {


            var apiInstance = new ResourcePermissionsApi();
            var resourceUri = resourceUri_example;  // string | The uri path of a resource to validate, must be URL encoded, uri segments are allowed.

            try
            {
                // Get a resource permissions object.
                ResourcePermission result = apiInstance.V1ResourcesResourceUriGet(resourceUri);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ResourcePermissionsApi.V1ResourcesResourceUriGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **resourceUri** | **string**| The uri path of a resource to validate, must be URL encoded, uri segments are allowed. |

### Return type

[**ResourcePermission**](ResourcePermission.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1resourcesresourceuriput"></a>
# **V1ResourcesResourceUriPut**
> Object V1ResourcesResourceUriPut (ResourcePermission body, string resourceUri)

Update a resource permissions object.

Updates the global permissions on a resource. This applies to all users in an account.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ResourcesResourceUriPutExample
    {
        public void main()
        {


            var apiInstance = new ResourcePermissionsApi();
            var body = new ResourcePermission(); // ResourcePermission | The contents of the permission to set on the resource. Overwrites existing data.
            var resourceUri = resourceUri_example;  // string | The uri path of a resource to validate, must be URL encoded, uri segments are allowed.

            try
            {
                // Update a resource permissions object.
                Object result = apiInstance.V1ResourcesResourceUriPut(body, resourceUri);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ResourcePermissionsApi.V1ResourcesResourceUriPut: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**ResourcePermission**](ResourcePermission.md)| The contents of the permission to set on the resource. Overwrites existing data. |
 **resourceUri** | **string**| The uri path of a resource to validate, must be URL encoded, uri segments are allowed. |

### Return type

**Object**

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

