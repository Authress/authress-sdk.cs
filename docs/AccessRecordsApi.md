# Authress.SDK.Api.AccessRecordsApi

All URIs are relative to */*

Method | HTTP request | Description
------------- | ------------- | -------------
[**V1ClaimsPost**](AccessRecordsApi.md#v1claimspost) | **POST** /v1/claims | Claim a resource by an allowed user
[**V1RecordsGet**](AccessRecordsApi.md#v1recordsget) | **GET** /v1/records | Get all account records.
[**V1RecordsPost**](AccessRecordsApi.md#v1recordspost) | **POST** /v1/records | Create a new access record
[**V1RecordsRecordIdDelete**](AccessRecordsApi.md#v1recordsrecordiddelete) | **DELETE** /v1/records/{recordId} | Deletes an access record.
[**V1RecordsRecordIdGet**](AccessRecordsApi.md#v1recordsrecordidget) | **GET** /v1/records/{recordId} | Get an access record for the account.
[**V1RecordsRecordIdPut**](AccessRecordsApi.md#v1recordsrecordidput) | **PUT** /v1/records/{recordId} | Update an access record.

<a name="v1claimspost"></a>
# **V1ClaimsPost**
> ClaimResponse V1ClaimsPost (ClaimRequest body)

Claim a resource by an allowed user

Claim a resource by allowing a user to pick an identifier and recieve admin access to that resource if it hasn't already been claimed. This only works for resources specifically marked as <strong>CLAIM</strong>. The result will be a new access record listing that user as the admin for this resource.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1ClaimsPostExample
    {
        public void main()
        {


            var apiInstance = new AccessRecordsApi();
            var body = new ClaimRequest(); // ClaimRequest |

            try
            {
                // Claim a resource by an allowed user
                ClaimResponse result = apiInstance.V1ClaimsPost(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccessRecordsApi.V1ClaimsPost: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**ClaimRequest**](ClaimRequest.md)|  |

### Return type

[**ClaimResponse**](ClaimResponse.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1recordsget"></a>
# **V1RecordsGet**
> AccessRecord V1RecordsGet ()

Get all account records.

Returns a paginated records list for the account. Only records the user has access to are returned.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1RecordsGetExample
    {
        public void main()
        {


            var apiInstance = new AccessRecordsApi();

            try
            {
                // Get all account records.
                AccessRecord result = apiInstance.V1RecordsGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccessRecordsApi.V1RecordsGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**AccessRecord**](AccessRecord.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1recordspost"></a>
# **V1RecordsPost**
> AccessRecord V1RecordsPost (AccessRecord body)

Create a new access record

Specify user roles for specific resources.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1RecordsPostExample
    {
        public void main()
        {


            var apiInstance = new AccessRecordsApi();
            var body = new AccessRecord(); // AccessRecord |

            try
            {
                // Create a new access record
                AccessRecord result = apiInstance.V1RecordsPost(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccessRecordsApi.V1RecordsPost: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**AccessRecord**](AccessRecord.md)|  |

### Return type

[**AccessRecord**](AccessRecord.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1recordsrecordiddelete"></a>
# **V1RecordsRecordIdDelete**
> void V1RecordsRecordIdDelete (string recordId)

Deletes an access record.

Remove an access record, removing associated permissions from all users in record. If a user has a permission from another record, that permission will not be removed.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1RecordsRecordIdDeleteExample
    {
        public void main()
        {


            var apiInstance = new AccessRecordsApi();
            var recordId = recordId_example;  // string | The identifier of the access record.

            try
            {
                // Deletes an access record.
                apiInstance.V1RecordsRecordIdDelete(recordId);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccessRecordsApi.V1RecordsRecordIdDelete: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **recordId** | **string**| The identifier of the access record. |

### Return type

void (empty response body)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1recordsrecordidget"></a>
# **V1RecordsRecordIdGet**
> AccessRecord V1RecordsRecordIdGet (string recordId)

Get an access record for the account.

Access records contain information assigning permissions to users for resources.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1RecordsRecordIdGetExample
    {
        public void main()
        {


            var apiInstance = new AccessRecordsApi();
            var recordId = recordId_example;  // string | The identifier of the access record.

            try
            {
                // Get an access record for the account.
                AccessRecord result = apiInstance.V1RecordsRecordIdGet(recordId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccessRecordsApi.V1RecordsRecordIdGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **recordId** | **string**| The identifier of the access record. |

### Return type

[**AccessRecord**](AccessRecord.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="v1recordsrecordidput"></a>
# **V1RecordsRecordIdPut**
> AccessRecord V1RecordsRecordIdPut (AccessRecord body, string recordId)

Update an access record.

Updates an access record adding or removing user permissions to resources.

### Example
```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class V1RecordsRecordIdPutExample
    {
        public void main()
        {


            var apiInstance = new AccessRecordsApi();
            var body = new AccessRecord(); // AccessRecord |
            var recordId = recordId_example;  // string | The identifier of the access record.

            try
            {
                // Update an access record.
                AccessRecord result = apiInstance.V1RecordsRecordIdPut(body, recordId);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AccessRecordsApi.V1RecordsRecordIdPut: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**AccessRecord**](AccessRecord.md)|  |
 **recordId** | **string**| The identifier of the access record. |

### Return type

[**AccessRecord**](AccessRecord.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/links+json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

