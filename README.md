# authress-sdk.cs
The Authress SDK for C#

[![NuGet version](https://badge.fury.io/nu/Authress.Sdk.svg)](https://badge.fury.io/nu/Authress.Sdk)
[![Build Status](https://travis-ci.com/authress/authress-sdk.cs.svg?branch=master)](https://travis-ci.com/authress/authress-sdk.cs)


### Development

```csharp
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;
```
<a name="getting-started"></a>
## Getting Started

```csharp
using System;
using System.Diagnostics;
using Authress.SDK.Api;
using Authress.SDK;
using Authress.SDK.DTO;

namespace Example
{
    public class Example
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

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to */*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AccessRecordsApi* | [**V1ClaimsPost**](docs/AccessRecordsApi.md#v1claimspost) | **POST** /v1/claims | Claim a resource by an allowed user
*AccessRecordsApi* | [**V1RecordsGet**](docs/AccessRecordsApi.md#v1recordsget) | **GET** /v1/records | Get all account records.
*AccessRecordsApi* | [**V1RecordsPost**](docs/AccessRecordsApi.md#v1recordspost) | **POST** /v1/records | Create a new access record
*AccessRecordsApi* | [**V1RecordsRecordIdDelete**](docs/AccessRecordsApi.md#v1recordsrecordiddelete) | **DELETE** /v1/records/{recordId} | Deletes an access record.
*AccessRecordsApi* | [**V1RecordsRecordIdGet**](docs/AccessRecordsApi.md#v1recordsrecordidget) | **GET** /v1/records/{recordId} | Get an access record for the account.
*AccessRecordsApi* | [**V1RecordsRecordIdPut**](docs/AccessRecordsApi.md#v1recordsrecordidput) | **PUT** /v1/records/{recordId} | Update an access record.
*AccountsApi* | [**V1AccountsAccountIdGet**](docs/AccountsApi.md#v1accountsaccountidget) | **GET** /v1/accounts/{accountId} | Get account information.
*AccountsApi* | [**V1AccountsGet**](docs/AccountsApi.md#v1accountsget) | **GET** /v1/accounts | Get all accounts user has access to
*AccountsApi* | [**V1IdentitiesGet**](docs/AccountsApi.md#v1identitiesget) | **GET** /v1/identities | Get all linked identities for this account.
*AccountsApi* | [**V1IdentitiesPost**](docs/AccountsApi.md#v1identitiespost) | **POST** /v1/identities | Link a new account identity.
*ClientsApi* | [**V1ClientsClientIdAccessKeysKeyIdDelete**](docs/ClientsApi.md#v1clientsclientidaccesskeyskeyiddelete) | **DELETE** /v1/clients/{clientId}/access-keys/{keyId} | Remove an access key for a client
*ClientsApi* | [**V1ClientsClientIdAccessKeysPost**](docs/ClientsApi.md#v1clientsclientidaccesskeyspost) | **POST** /v1/clients/{clientId}/access-keys | Request a new access key
*ClientsApi* | [**V1ClientsClientIdDelete**](docs/ClientsApi.md#v1clientsclientiddelete) | **DELETE** /v1/clients/{clientId} | Delete a client
*ClientsApi* | [**V1ClientsClientIdGet**](docs/ClientsApi.md#v1clientsclientidget) | **GET** /v1/clients/{clientId} | Get a client.
*ClientsApi* | [**V1ClientsClientIdPut**](docs/ClientsApi.md#v1clientsclientidput) | **PUT** /v1/clients/{clientId} | Update a client
*ClientsApi* | [**V1ClientsGet**](docs/ClientsApi.md#v1clientsget) | **GET** /v1/clients | Get clients collection
*ClientsApi* | [**V1ClientsPost**](docs/ClientsApi.md#v1clientspost) | **POST** /v1/clients | Create a new client.
*ResourcePermissionsApi* | [**V1ResourcesGet**](docs/ResourcePermissionsApi.md#v1resourcesget) | **GET** /v1/resources | List resource configurations
*ResourcePermissionsApi* | [**V1ResourcesResourceUriGet**](docs/ResourcePermissionsApi.md#v1resourcesresourceuriget) | **GET** /v1/resources/{resourceUri} | Get a resource permissions object.
*ResourcePermissionsApi* | [**V1ResourcesResourceUriPut**](docs/ResourcePermissionsApi.md#v1resourcesresourceuriput) | **PUT** /v1/resources/{resourceUri} | Update a resource permissions object.
*UserPermissionsApi* | [**V1UsersUserIdResourcesResourceUriPermissionsGet**](docs/UserPermissionsApi.md#v1usersuseridresourcesresourceuripermissionsget) | **GET** /v1/users/{userId}/resources/{resourceUri}/permissions | Get the permissions a user has to a resource.
*UserPermissionsApi* | [**V1UsersUserIdResourcesResourceUriPermissionsPermissionGet**](docs/UserPermissionsApi.md#v1usersuseridresourcesresourceuripermissionspermissionget) | **GET** /v1/users/{userId}/resources/{resourceUri}/permissions/{permission} | Check to see if a user has permissions to a resource.

<a name="documentation-for-models"></a>
## Documentation for Models

 - [Authress.SDK.DTO.AccessRecord](docs/AccessRecord.md)
 - [Authress.SDK.DTO.AccessRecordAccount](docs/AccessRecordAccount.md)
 - [Authress.SDK.DTO.AccessRecordResources](docs/AccessRecordResources.md)
 - [Authress.SDK.DTO.AccessRecordStatements](docs/AccessRecordStatements.md)
 - [Authress.SDK.DTO.AccessRecordUsers](docs/AccessRecordUsers.md)
 - [Authress.SDK.DTO.Account](docs/Account.md)
 - [Authress.SDK.DTO.AccountCollection](docs/AccountCollection.md)
 - [Authress.SDK.DTO.ClaimRequest](docs/ClaimRequest.md)
 - [Authress.SDK.DTO.ClaimResponse](docs/ClaimResponse.md)
 - [Authress.SDK.DTO.Client](docs/Client.md)
 - [Authress.SDK.DTO.ClientAccessKey](docs/ClientAccessKey.md)
 - [Authress.SDK.DTO.ClientCollection](docs/ClientCollection.md)
 - [Authress.SDK.DTO.Identity](docs/Identity.md)
 - [Authress.SDK.DTO.IdentityCollection](docs/IdentityCollection.md)
 - [Authress.SDK.DTO.IdentityRequest](docs/IdentityRequest.md)
 - [Authress.SDK.DTO.InlineResponse200](docs/InlineResponse200.md)
 - [Authress.SDK.DTO.InlineResponse200Account](docs/InlineResponse200Account.md)
 - [Authress.SDK.DTO.PermissionObject](docs/PermissionObject.md)
 - [Authress.SDK.DTO.ResourcePermission](docs/ResourcePermission.md)
 - [Authress.SDK.DTO.ResourcePermissionCollection](docs/ResourcePermissionCollection.md)
 - [Authress.SDK.DTO.ResourcePermissionPermissions](docs/ResourcePermissionPermissions.md)

<a name="documentation-for-authorization"></a>
## Documentation for Authorization

<a name="oauth2"></a>
### oauth2





