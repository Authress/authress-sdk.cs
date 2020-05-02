# Authress.SDK.Api.ClientsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**V1ClientsClientIdAccessKeysKeyIdDelete**](ClientsApi.md#v1clientsclientidaccesskeyskeyiddelete) | **DELETE** /v1/clients/{clientId}/access-keys/{keyId} | Remove an access key for a client
[**V1ClientsClientIdAccessKeysPost**](ClientsApi.md#v1clientsclientidaccesskeyspost) | **POST** /v1/clients/{clientId}/access-keys | Request a new access key
[**V1ClientsClientIdDelete**](ClientsApi.md#v1clientsclientiddelete) | **DELETE** /v1/clients/{clientId} | Delete a client
[**V1ClientsClientIdGet**](ClientsApi.md#v1clientsclientidget) | **GET** /v1/clients/{clientId} | Get a client.
[**V1ClientsClientIdPut**](ClientsApi.md#v1clientsclientidput) | **PUT** /v1/clients/{clientId} | Update a client
[**V1ClientsGet**](ClientsApi.md#v1clientsget) | **GET** /v1/clients | Get clients collection
[**V1ClientsPost**](ClientsApi.md#v1clientspost) | **POST** /v1/clients | Create a new client.

<a name="v1clientsclientidaccesskeyskeyiddelete"></a>
# **V1ClientsClientIdAccessKeysKeyIdDelete**
> void V1ClientsClientIdAccessKeysKeyIdDelete (string clientId, string keyId)

Remove an access key for a client

Deletes an access key for a client prevent it from being used to authenticate with Authress.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsClientIdAccessKeysKeyIdDeleteExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();
            var clientId = clientId_example;  // string | The unique identifier of the client.
            var keyId = keyId_example;  // string | The id of the access key to remove from the client.

            try
            {
                // Remove an access key for a client
                apiInstance.V1ClientsClientIdAccessKeysKeyIdDelete(clientId, keyId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsClientIdAccessKeysKeyIdDelete: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **clientId** | **string**| The unique identifier of the client. |
 **keyId** | **string**| The id of the access key to remove from the client. |

### Return type

void (empty response body)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1clientsclientidaccesskeyspost"></a>
# **V1ClientsClientIdAccessKeysPost**
> ClientAccessKey V1ClientsClientIdAccessKeysPost (string clientId)

Request a new access key

Create a new access key for the client so that a service can authenticate with Authress as that client. Using the client will allow delegation of permission checking of users.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsClientIdAccessKeysPostExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();
            var clientId = clientId_example;  // string | The unique identifier of the client.

            try
            {
                // Request a new access key
                ClientAccessKey result = apiInstance.V1ClientsClientIdAccessKeysPost(clientId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsClientIdAccessKeysPost: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **clientId** | **string**| The unique identifier of the client. |

### Return type

[**ClientAccessKey**](ClientAccessKey.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1clientsclientiddelete"></a>
# **V1ClientsClientIdDelete**
> void V1ClientsClientIdDelete (string clientId)

Delete a client

This deletes the service client.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsClientIdDeleteExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();
            var clientId = clientId_example;  // string | The unique identifier for the client.

            try
            {
                // Delete a client
                apiInstance.V1ClientsClientIdDelete(clientId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsClientIdDelete: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **clientId** | **string**| The unique identifier for the client. |

### Return type

void (empty response body)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1clientsclientidget"></a>
# **V1ClientsClientIdGet**
> Client V1ClientsClientIdGet (string clientId)

Get a client.

Returns all information related to client except for the private access keys.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsClientIdGetExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();
            var clientId = clientId_example;  // string | The unique identifier for the client.

            try
            {
                // Get a client.
                Client result = apiInstance.V1ClientsClientIdGet(clientId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsClientIdGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **clientId** | **string**| The unique identifier for the client. |

### Return type

[**Client**](Client.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1clientsclientidput"></a>
# **V1ClientsClientIdPut**
> Client V1ClientsClientIdPut (Client body, string clientId)

Update a client

Updates a client information.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsClientIdPutExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();
            var body = new Client(); // Client |
            var clientId = clientId_example;  // string | The unique identifier for the client.

            try
            {
                // Update a client
                Client result = apiInstance.V1ClientsClientIdPut(body, clientId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsClientIdPut: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**Client**](Client.md)|  |
 **clientId** | **string**| The unique identifier for the client. |

### Return type

[**Client**](Client.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1clientsget"></a>
# **V1ClientsGet**
> ClientCollection V1ClientsGet ()

Get clients collection

Returns all clients that the user has access to in the account.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsGetExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();

            try
            {
                // Get clients collection
                ClientCollection result = apiInstance.V1ClientsGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**ClientCollection**](ClientCollection.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1clientspost"></a>
# **V1ClientsPost**
> Client V1ClientsPost (Client body)

Create a new client.

Creates a service client to interact with Authress or any other service on behalf of users. Each client has secret private keys used to authenticate with Authress. To use service clients created through other mechanisms, skip creating a client and create access records with the client identifier.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClientsPostExample
    {
        public void main()
        {


            var apiInstance = new ClientsApi();
            var body = new Client(); // Client |

            try
            {
                // Create a new client.
                Client result = apiInstance.V1ClientsPost(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ClientsApi.V1ClientsPost: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**Client**](Client.md)|  |

### Return type

[**Client**](Client.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

